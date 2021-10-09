sap.ui.define([
    "vcs/controller/BaseController",
    "sap/ui/model/json/JSONModel",
    "vcs/data/services/tenant/TenantsService",
    "sap/m/MessageBox",
    "sap/m/MessageToast",
    "vcs/helpers/Constants",
    "sap/ui/core/Core",
    "sap/m/Dialog",
    "sap/m/DialogType",
    "sap/m/Button",
    "sap/m/ButtonType",
    "sap/m/Label",
    "sap/m/Input"
],

    function (BaseController, JSONModel, TenantsService, MessageBox, MessageToast, Constants, Core, Dialog, DialogType, Button, ButtonType, Label, Input) {
        "use strict";
        return BaseController.extend("vcs.controller.tenants.tenantList.TenantList", {
            onInit: function () {
                BaseController.prototype.onInit.apply(this, arguments);
                this.getRouter().getRoute("tenants").attachPatternMatched(this.onRouteMatched, this);
                this.setModel(this.createViewModel(), "mainModel");
                this.getModel("mainModel").setProperty("/filters", { systemId: "", name: "" });
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

                TenantsService.getByFilter(this.getFilterValues()).then(response => {
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
            }

        });
    });
