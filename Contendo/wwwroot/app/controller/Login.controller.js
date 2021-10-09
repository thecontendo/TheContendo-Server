sap.ui.define([
    "vcs/controller/BaseController",
    "sap/ui/model/json/JSONModel",
    "vcs/data/services/auth/AuthService"
],

    function (BaseController, JSONModel, AuthService) {
        "use strict";
        return BaseController.extend("vcs.controller.Login", {
            onInit: function () {
                BaseController.prototype.onInit.apply(this, arguments);

                this.getRouter().getRoute("login").attachPatternMatched(this.onRouteMatched, this);
                this.setModel(new JSONModel({username: "", password: ""}), "mainModel");
            },

            onRouteMatched: function () {
                if (this.securityTokenIsAvailable()) {
                    this.getRouter().navTo("home");
                }
            },

            onLogin: function () {
                var oData = this.getModel("mainModel").getData();

                this.getObj('username').setValueState(!!oData.username ? 'None' : 'Error');
                this.getObj('password').setValueState(!!oData.password ? 'None' : 'Error');
                if (!oData.username || !oData.password) {
                    return;
                }

                this.setBusy(true, true);

                AuthService.getToken(oData).then(response => {
                    if (response.data.token) {
                        sessionStorage.setItem("vcs.token", response.data.token);
                        this.setBusy(false);
                        this.getRouter().navTo("home");
                    } else {
                        this.setBusy(false);
                        alert("error...");
                    }
                });


            }
        });
    });
