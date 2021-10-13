sap.ui.define([
    "sap/ui/model/json/JSONModel",
    "vcs/helpers/MessageHelper",
    "sap/m/MessageBox"
],
    function (JSONModel, MessageHelper, MessageBox) {
        "use strict";

        return {
            callServer: function (method, apiUrl, payload, handleErrors) {
                let currentManager = this;
                return new Promise( function (fnResolve, fnReject) {

                    let token = localStorage.getItem( "vcs.token" );

                    if (!token && apiUrl.indexOf( "Token" ) === -1) {
                        sap.ui.core.BusyIndicator.hide();
                        MessageHelper.showErrorMessage( "Session timeout, Please login again...", () => {
                            window.location = "";
                        } );

                        return;
                    }

                    let reqHeader = new Headers();
                    reqHeader.append( "Content-Type", "application/json" );
                    reqHeader.append( "Authorization", `Bearer ${token}` );

                    let options = {
                        method: method ?? 'GET',
                        headers: reqHeader,
                        body: method === "GET" ? null : JSON.stringify( payload ?? {} )
                    };

                    let request = new Request( `/api${apiUrl}`, options );

                    fetch( request )
                        .then( response => {
                            currentManager.handleErrors( response );
                            return response.ok ? response.json() : fnReject( response );
                        } )
                        .then( function (oResponse) {
                            if (!handleErrors && !oResponse.success && oResponse.message) {
                                sap.ui.core.BusyIndicator.hide();
                                MessageHelper.showErrorMessage( oResponse.message.text );
                            } else {
                                fnResolve( oResponse );
                            }
                        } )
                        .catch( function (err) {
                            fnReject( err );
                            if (err?.message?.text) {
                                sap.ui.core.BusyIndicator.hide();
                                MessageHelper.showErrorMessage( err.message.text );
                            }
                        } );


                } );
            },

            handleErrors: function (response) {
                switch (response.status) {
                    case 500: {
                        MessageBox.error(`${response.status}: Server Error`);
                        sap.ui.core.BusyIndicator.hide();
                    }
                        break;
                    case 401: {
                        localStorage.removeItem(vcs.token);
                        controller.getRouter().navTo("login");
                        sap.ui.core.BusyIndicator.hide();
                    }
                        break;
                    case 403: {
                        MessageBox.warning(`${response.status}: You don't have permission to perform this action, please contact your administrator...`);
                        sap.ui.core.BusyIndicator.hide();
                    }
                        break;
                    case 404: {
                        MessageBox.error("not Found Error...");
                        sap.ui.core.BusyIndicator.hide();
                    }
                        break;
                    default:
                }
            }
        };
    });
