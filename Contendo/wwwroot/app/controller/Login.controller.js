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

                if(localStorage.getItem("vcs.name") && this.getModel("globalModel"))
                {
                    this.getModel("globalModel").setProperty('/name', localStorage.getItem("vcs.name"));
                }
            },

            onRouteMatched: function () {
                if (this.securityTokenIsAvailable()) {
                    this.getRouter().navTo("home");
                }
            },

            onRegister: function(){
              this.getObj('regForm').setVisible(true);  
              this.getObj('logForm').setVisible(false);  
            },

            onSubmitRegister: function(){
                
            },

            onBackLogin: function(){
                this.getObj('regForm').setVisible(false);
                this.getObj('logForm').setVisible(true);
            },
            
            onLogin: function () {
                var oData = this.getModel("mainModel").getData();

                this.getObj('username').setValueState(!!oData.username ? 'None' : 'Error');
                if (!oData.username) {
                    return;
                }

                this.setBusy(true, true);

                AuthService.getToken(oData).then(response => {
                    if (response.data.token) {
                        localStorage.setItem("vcs.token", response.data.token);
                        localStorage.setItem("vcs.name", response.data.name);
                        localStorage.setItem("vcs.userId", response.data.userId);
                        localStorage.setItem("vcs.email", response.data.email);
                        this.getModel("globalModel").setProperty('/name', response.data.name);
                        this.getModel("globalModel").setProperty('/email', response.data.email);
                        this.getModel("globalModel").setProperty('/userId', response.data.userId);
                        
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
