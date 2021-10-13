sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/ui/model/json/JSONModel",
    'sap/ui/core/Fragment',
    "vcs/data/services/global/GlobalService",

], function (Controller, JSONModel, Fragment, GlobalService) {
    "use strict";

    return Controller.extend("vcs.controller.App", {
        onInit: function () {
            this.getOwnerComponent().setModel(new JSONModel({name: "Admin", email: "", userId: ""}), "globalModel");
            if(localStorage.getItem("vcs.name"))
            {
                this.getOwnerComponent().getModel("globalModel").setProperty('/name', localStorage.getItem("vcs.name"));
            }
        },

        onAfterRendering: function(){
        
        },

        onHomePressed: function () {
            this.getOwnerComponent().getRouter().navTo("home", {});
        },

        handleMenu: function (oEvent) {
            let sKey = oEvent.getParameter('item').getKey();
            switch (sKey) {
                case "Logout":
                    localStorage.removeItem("vcs.token");
                    localStorage.removeItem("vcs.name");
                    localStorage.removeItem("vcs.email");
                    localStorage.removeItem("vcs.userId");
                    window.location = "";
                    break;
                default:
                    break;
            }
        },

        onUserButtonPressed: function (oEvent) {
        //    if (!this._UserMenu) {
        //        this._UserMenu = Fragment.load({
        //            id: this.getView().getId(),
        //            name: "vcs.view.fragments.UserMenu",
        //            controller: this
        //        }).then(function (oMenu) {
        //            oMenu.openBy(oEvent.getSource());
        //            this._UserMenu = oMenu;
        //            return this._UserMenu;
        //        }.bind(this));
        //    } else {
        //        this._UserMenu.openBy(oEvent.getSource());
        //    }
        },

        onUserMenuPressed: function (oEvent) {
            var oItem = oEvent.getParameter("item"),
                sItemPath = "";

            while (oItem instanceof MenuItem) {
                sItemPath = oItem.getText() + " > " + sItemPath;
                oItem = oItem.getParent();
            }

            sItemPath = sItemPath.substr(0, sItemPath.lastIndexOf(" > "));

            MessageToast.show("Action triggered on item: " + sItemPath);
        }

    });
});
