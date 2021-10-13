sap.ui.define([
    "vcs/controller/BaseController",
    "sap/ui/model/json/JSONModel",
    "vcs/data/services/contacts/ContactsService",
    "sap/m/MessageBox",
    "sap/m/MessageToast",
    "vcs/helpers/Constants",
    "sap/ui/core/Core",
    "sap/m/Dialog",
    "sap/m/DialogType",
    "sap/m/Button",
    "sap/m/ButtonType",
    "sap/m/Label",
    "sap/m/Input", 'sap/ui/core/Fragment',
],

    function (BaseController, JSONModel, ContactsService, MessageBox, MessageToast, Constants, Core, Dialog, DialogType, Button, ButtonType, Label, 
              Input, Fragment) {
        "use strict";
        return BaseController.extend("vcs.controller.contacts.contactList.ContactList", {
            onInit: function () {
                BaseController.prototype.onInit.apply(this, arguments);
                this.getRouter().getRoute("contacts").attachPatternMatched(this.onRouteMatched, this);
                this.setModel(this.createViewModel(), "mainModel");
                this.getModel("mainModel").setProperty("/filters", { systemId: "", name: "" });
                this.setModel(new JSONModel({items: []}), "requestModel");
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

                ContactsService.getContacts(this.getFilterValues()).then(response => {
                    this.setBusy(false);
                    this.getModel("mainModel").setProperty("/data", response.data);
                });
            },

            onItemPress: function (oEvent) {
                let object = oEvent.getSource().getBindingContext('mainModel').getObject();
                if (object.id) {
                    this.getRouter().navTo('contactDetail', {mode: "display", id: object.id});
                }
            },

            onAddPress: function () {
                if (!this.oAddDialog) {
                    this.oAddDialog = new Dialog({
                        type: DialogType.Message,
                        title: "New Contact",
                        content: [
                            new Label({
                                text: "Contact Email",
                                labelFor: "contactIdInput"
                            }),
                            new Input("contactIdInput", {
                                width: "100%",
                                liveChange:  (oEvent) =>  {
                                    let sText = oEvent.getParameter("value");
                                    this.oAddDialog.getBeginButton().setEnabled(sText.length > 0);
                                }
                            }),
                            new sap.m.VBox({items:[
                                   new sap.m.Link({text:'Generate link to invite', press: () => {
                                         
                                       }}),
                                    new sap.m.Link({})
                                ]})
                        ],
                        beginButton: new Button({
                            type: ButtonType.Emphasized,
                            text: "Send",
                            enabled: false,
                            press: function () {
                                let oInput = this.oAddDialog.getContent()[1];
                                let sText = oInput.getValue();
                                this.onSendRequest(sText);
                                oInput.setValue('');
                                // this.getRouter().navTo('tenantDetail', { mode: "edit", id: "new", systemId: sText });
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
            
            onSendRequest: function(email){
                this.setBusy(true);
                ContactsService.sendContactRequestByEmail({
                    email:email
                }).then(response => {
                    this.setBusy(false);
                });
            },

            onCheckRequest: function(oEvent) {
                let oView = this.getView(),
                    mainModel = this.getModel('mainModel');
    
                // create value help dialog
                if (!this._pValueHelpDialog) {
                    this._pValueHelpDialog = Fragment.load({
                        id: oView.getId(),
                        name: "vcs.view.contacts.contactList.fragments.ContactRequests",
                        controller: this
                    }).then(dlg => {
                        oView.addDependent(dlg);
                        dlg.setModel(mainModel, "mainModel")
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
                ContactsService.checkContactRequests().then(response => {
                    dialog.setBusy(false);
                    this.getModel("requestModel").setProperty("/items", response.data);
                });
            },

            onRespondPress: function(oEvent, response){
               let payLoad = {
                   requestId: oEvent.getSource().getBindingContext('requestModel').getProperty('id'),
                   accepted: response
               };
                this.oRequestTable.setBusy(true);
                ContactsService.sendContactRequestResponse(payLoad).then(response => {
                    this.oRequestTable.setBusy(false);
                    this.onRequestDialogCancel();
                    this.loadView();
                });
            },

            onDeletePress: function (oEvent) {
                let oTable = this.getObj("tenantsTableId");
                if (oTable.getSelectedItem()) {
                    let oSelectedItem = oTable.getSelectedItem();
                    let sId = oSelectedItem.getBindingContext("mainModel").getProperty("id");
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
                            let oTable = this.getObj("tenantsTableId"),
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
                let oFilters = this.getModel('mainModel').getProperty('/filters');
                let oData = { ...oFilters };
                oData.systemId = oData.systemId ?? "";
                oData.name = oData.name ?? "";

                return oData;
            },

            onClear: function () {
                this.getModel('mainModel').setProperty('/filters', { systemId: '', name: '' });
            },

            removeTableSelections: function () {
                this.getObj("tenantsTableId").removeSelections();
            }

        });
    });
