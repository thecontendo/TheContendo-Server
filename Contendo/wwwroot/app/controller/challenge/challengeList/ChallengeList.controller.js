sap.ui.define([
    "vcs/controller/BaseController",
    "sap/ui/model/json/JSONModel",
    "vcs/data/services/challenge/ChallengeService",
    "sap/m/MessageBox",
    "sap/m/MessageToast",
    "vcs/helpers/Constants",
    "sap/ui/core/Core",
    "sap/m/Dialog",
    "sap/m/DialogType",
    "sap/m/Button",
    "sap/m/ButtonType",
    "sap/m/Label",
    "sap/m/Input",
        'sap/ui/core/Fragment',
        "vcs/data/services/contacts/ContactsService",
],

    function (BaseController, JSONModel, ChallengeService, MessageBox, MessageToast, 
              Constants, Core, Dialog, DialogType, Button, ButtonType, Label, Input, Fragment, ContactsService) {
        "use strict";
        return BaseController.extend("vcs.controller.challenge.challengeList.ChallengeList", {
            onInit: function () {
                BaseController.prototype.onInit.apply(this, arguments);
                this.getRouter().getRoute("challenge").attachPatternMatched(this.onRouteMatched, this);
                this.setModel(this.createViewModel(), "mainModel");
                this.getModel("mainModel").setProperty("/filters", { systemId: "", name: "" });
                this.setModel(new JSONModel({items: []}), "requestModel");
                this.setModel(new JSONModel({items: []}), "contactModel");
            },

            onBeforeRendering: function () {

            },

            onRouteMatched: function () {
                if (!this.securityTokenIsAvailable()) {
                    this.getRouter().navTo("login");
                    return;
                }
                this.removeTableSelections();
                this.loadView();
            },

            loadView: function () {
                this.setBusy(true, true);
                ChallengeService.getAllChallenges().then(response => {
                    this.setBusy(false);
                    this.getModel("mainModel").setProperty("/data", response.data);
                });
            },

            onItemPress: function (oEvent) {
                var object = oEvent.getSource().getBindingContext('mainModel').getObject();
                if (object.id) {
                    this.getRouter().navTo('tenantDetail', {mode: "display", id: object.id});
                }
            },

            onAddPress: function () {
                if (!this.oAddDialog) {
                    this.oAddDialog = new Dialog({
                        type: DialogType.Message,
                        title: "New Tenant",
                        content: [
                            new Label({
                                text: "Tenant Id",
                                labelFor: "tenantIdInput"
                            }),
                            new Input("tenantIdInput", {
                                width: "100%",
                                liveChange: function (oEvent) {
                                    var sText = oEvent.getParameter("value");
                                    this.oAddDialog.getBeginButton().setEnabled(sText.length > 0);
                                }.bind(this)
                            })
                        ],
                        beginButton: new Button({
                            type: ButtonType.Emphasized,
                            text: "Ok",
                            enabled: false,
                            press: function () {
                                var sText = Core.byId("tenantIdInput").getValue();
                                this.getRouter().navTo('tenantDetail', { mode: "edit", id: "new", systemId: sText });
                                this.oAddDialog.close();
                            }.bind(this)
                        }),
                        endButton: new Button({
                            text: "Cancel",
                            press: function () {
                                this.oAddDialog.close();
                            }.bind(this)
                        })
                    });
                }

                this.oAddDialog.open();
            },

            onDeletePress: function (oEvent) {
                var oTable = this.getObj("tenantsTableId");
                if (oTable.getSelectedItem()) {
                    var oSelectedItem = oTable.getSelectedItem();
                    var sId = oSelectedItem.getBindingContext("mainModel").getProperty("id");
                    if (sId !== "") {
                        this._proceedDelete();
                    }
                } else {
                    MessageToast.show(this.getText("selectRowFirst"));
                }
            },

            _proceedDelete: function (sId) {
                MessageBox.warning(this.getText("confirmationMsg"), {
                    title: "Alert",
                    emphasizedAction: MessageBox.Action.NO,
                    actions: [MessageBox.Action.YES, MessageBox.Action.NO],
                    onClose: function (oAction) {
                        if (oAction === "YES") {
                            var oTable = this.getObj("tenantsTableId"),
                                oModel = oTable.getModel("mainModel"),
                                data = oModel.getData(),
                                oSelectedItem = oTable.getSelectedItem(),
                                aPath = oSelectedItem.getBindingContext("mainModel").getPath().split("/"),
                                sIndex = aPath[aPath.length - 1],
                                sId = oSelectedItem.getBindingContext("mainModel").getProperty("id");
                            TenantsService.deleteTenant(sId).then(
                                function (oData) {
                                    if (this.messageHandler.checkServerResponseForSuccess(oData) === Constants.serverErrors.Success) {
                                        this.loadView();
                                       /* data.splice(sIndex, 1);
                                        oModel.setProperty("/", data);
                                        oTable.getBinding("items").refresh(true);
                                        oTable.rerender();*/
                                    } else {
                                        this.messageHandler.showMessageOutServerResponse(oData);
                                    }
                                }.bind(this),
                                function (oError) {
                                    this.messageHandler.showSystemError(oError);
                                }.bind(this));
                        }
                    }.bind(this)
                });
            },

            onSearch: function () {
                this.loadView();
            },

            getFilterValues: function () {
                var oFilters = this.getModel('mainModel').getProperty('/filters');
                var oData = { ...oFilters };
                oData.systemId = oData.systemId ?? "";
                oData.name = oData.name ?? "";

                return oData;
            },

            onClear: function () {
                this.getModel('mainModel').setProperty('/filters', { systemId: '', name: '' });
            },

            removeTableSelections: function () {
                this.getObj("tenantsTableId").removeSelections();
            },

            onCheckRequest: function(oEvent) {
                let oView = this.getView(),
                    mainModel = this.getModel('requestModel');

                // create value help dialog
                if (!this._pValueHelpDialog) {
                    this._pValueHelpDialog = Fragment.load({
                        id: oView.getId(),
                        name: "vcs.view.challenge.challengeList.fragments.ChallengeRequest",
                        controller: this
                    }).then(dlg => {
                        oView.addDependent(dlg);
                        dlg.setModel(mainModel, "requestModel")
                        this.oRequestTable = dlg.getContent()[0];
                        this.oRequestTable.setBusyIndicatorDelay(0);
                        dlg.setBusyIndicatorDelay(0);
                        return dlg;
                    });
                }

                this._pValueHelpDialog.then(dlg => dlg.open());
            },

            onRequestDialogCancel: function(){
                this._pValueHelpDialog.then(dlg => dlg.close());
            },

            openRequests: function(oEvent){
                let dialog = oEvent.getSource();
                dialog.setBusy(true);
                // ChallengeService
                /*ContactsService.checkContactRequests().then(response => {
                    dialog.setBusy(false);
                    this.getModel("requestModel").setProperty("/items", response.data);
                });*/
                ChallengeService.getAllShots().then(response => {
                    dialog.setBusy(false);
                    this.getModel("requestModel").setProperty("/items", response.data);
                });
            },

            onRespondPress: function(oEvent){
                this.oRequestTable.setBusy(true);
                this.selectedShot = oEvent.getSource().getBindingContext('requestModel').getProperty('id');
                this.selectedCounter = oEvent.getSource().getBindingContext('requestModel').getProperty('value');
                this.onRequestDialogCancel();
                this.onOpenContact();
            },

            onOpenContact: function(oEvent) {
                let oView = this.getView();

                // create value help dialog
                if (!this._pContactDialog) {
                    this._pContactDialog = Fragment.load({
                        id: oView.getId(),
                        name: "vcs.view.challenge.challengeList.fragments.Contacts",
                        controller: this
                    }).then(dlg => {
                        oView.addDependent(dlg);
                        //dlg.setModel(mainModel, "contactModel")
                        this.oContactTable = dlg.getContent()[0];
                        this.oContactTable.setBusyIndicatorDelay(0);
                        dlg.setBusyIndicatorDelay(0);
                        return dlg;
                    });
                }

                this._pContactDialog.then(dlg => dlg.open());
            },

            onContactDialogCancel: function(){
                this._pContactDialog.then(dlg => dlg.close());
            },

            openContacts: function(oEvent){
                let dialog = oEvent.getSource();
                dialog.setBusy(true);
                ContactsService.getContacts(this.getFilterValues()).then(response => {
                    dialog.setBusy(false);
                    this.getModel("contactModel").setProperty("/items", response.data);
                });
            },
            

            onContactPress: function(oEvent, response){
                let payLoad = {
                    "points": this.selectedCounter,
                    "duration": 60,
                    "shotId": this.selectedShot,
                    "defenderId":  this.selectedShot = oEvent.getSource().getBindingContext('contactModel').getProperty('id')
                }
                
                this.oContactTable.setBusy(true);

                ChallengeService.sendChallenge(payLoad).then(response => {
                      this.oContactTable.setBusy(false);
                      this.onRequestDialogCancel();
                      this.loadView();
                  });
            },

        });
    });
