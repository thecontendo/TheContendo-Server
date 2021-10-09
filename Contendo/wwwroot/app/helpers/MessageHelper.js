sap.ui.define([
    "sap/m/MessageBox",
    'sap/m/MessageToast',
    'vcs/helpers/Constants'
],
    function (MessageBox, MessageToast, Constants) {
        "use strict";

        return {

            checkServerResponseForSuccess: function (oData) {
                if (oData && oData.message && oData.message.code === 0) {
                    return 0;
                } else if (oData && oData.message && oData.message.code) {
                    for (var sProp in Constants.serverErrors) {
                        if (Constants.serverErrors[sProp] == oData.message.code) {
                            return oData.message.code;
                        }
                    }
                } else {
                    console.error("Unknown server message type");
                    return Constants.serverErrors.UnKnownError;
                }
            },

            showSystemError: function (oError, fnCloseCallback) {

                MessageBox.error(
                    "Status: " + oError.status + (oError.responseText ? ". Message " + oError.responseText : ""),
                    {
                        actions: [MessageBox.Action.CLOSE],
                        onClose: function () {
                            typeof (fnCloseCallback) === "function" && fnCloseCallback();
                        }
                    }
                );
            },

            showToastOutServerResponse: function (oData) {
                if (oData && oData.message && oData.message.text) {
                    MessageToast.show(oData.message.text);
                } else {
                    console.error("Can not show Toast");
                }
            },

            showToast: function (sText) {
                MessageToast.show(sText);
            },

            showMessageOutServerResponse: function (oData, fnCloseCallback) {
                if (oData && oData.message && oData.message.code === 0 && oData.message.text) {
                    MessageBox.success(
                        oData.message.text,
                        {
                            actions: [MessageBox.Action.CLOSE],
                            onClose: function () {
                                typeof (fnCloseCallback) === "function" && fnCloseCallback();
                            }
                        }
                    );
                    return;
                } else if (oData && oData.message && oData.message.code !== 0 && oData.message.text) {
                    MessageBox.error(
                        oData.message.text,
                        {
                            actions: [MessageBox.Action.CLOSE],
                            onClose: function () {
                                typeof (fnCloseCallback) === "function" && fnCloseCallback();
                            }
                        }
                    );
                    return;
                }

                MessageBox.error(
                    "Unknown error occurred",
                    {
                        actions: [MessageBox.Action.CLOSE],
                        onClose: function () {
                            typeof (fnCloseCallback) === "function" && fnCloseCallback();
                        }
                    }
                );
            },
            showConfirmationDialog: function (oMessage) {
                var oMessage = (oMessage) ? oMessage : "Would you like to proceed ?";
                var oPromise = new Promise(function (fnResolve, fnReject) {
                    MessageBox.show(
                        oMessage, {
                            icon: MessageBox.Icon.INFORMATION,
                            title: "",
                            actions: [MessageBox.Action.YES, MessageBox.Action.NO],
                            onClose: function (oAction) { fnResolve(oAction == "YES"); }
                        }
                    );
                });
                return oPromise;
            },

            showSuccessMessage: function (sText, fnOnClose) {

                MessageBox.success(sText, {
                    actions: [MessageBox.Action.OK],
                    onClose: function () {
                        if (fnOnClose && typeof fnOnClose === "function") {
                            fnOnClose();
                        }

                    }
                });

            },

            showErrorMessage: function (sText, fnOnClose) {
                MessageBox.error(sText, {
                    actions: [MessageBox.Action.OK],
                    onClose: function () {
                        if (fnOnClose && typeof fnOnClose === "function") {
                            fnOnClose();
                        }
                    }
                });
            }

        }
    });
