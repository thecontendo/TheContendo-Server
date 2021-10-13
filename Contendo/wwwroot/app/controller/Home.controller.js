sap.ui.define([
    "vcs/controller/BaseController",
    "sap/ui/model/json/JSONModel",
],

    function (BaseController, JSONModel) {
        "use strict";
        return BaseController.extend("vcs.controller.Home", {
            onInit: function () {
                BaseController.prototype.onInit.apply(this, arguments);
                this.getRouter().getRoute("home").attachPatternMatched(this.onRouteMatched, this);
            },

            onRouteMatched: function () {
                if (!this.securityTokenIsAvailable()) {
                    this.getRouter().navTo("login");
                    return;
                }
            },

            contactsPressed: function () {
                this.getRouter().navTo("contacts");
            },

            challengePressed: function () {
                this.getRouter().navTo("challenge");
            }

        });
    });
