sap.ui.define([
    "vcs/controller/BaseController",
    "sap/ui/model/json/JSONModel",
    "vcs/data/services/tenant/TenantsService",
    "sap/m/MessageBox",
    'sap/m/MessageToast',
    "sap/ui/core/Fragment",
    "sap/ui/model/Filter",
    "sap/ui/model/FilterOperator",
    "vcs/helpers/Constants",
    "vcs/model/models",
],

    function (BaseController, JSONModel, TenantsService, MessageBox, MessageToast, Fragment, Filter, FilterOperator, Constants, models) {
        "use strict";
        return BaseController.extend("vcs.controller.tenants.tenantDetail.TenantDetail", {

            onInit: function () {
                BaseController.prototype.onInit.apply(this, arguments);

                this.getRouter().getRoute("tenantDetail").attachPatternMatched(this.onRouteMatched, this);

                this.setModel(this.createViewModel({ viewMode: "display" }), "mainModel");
                this.setModel(models.createPasswordPolicyDictionaryModel(), "passwordPolicyModel");
                this.setModel(new JSONModel(), "dialogModel");
                this.setModel(new JSONModel(), "dialogEditModel");
            },

            onRouteMatched: function (oEvent) {
                if (!this.securityTokenIsAvailable()) {
                    this.getRouter().navTo("login");
                    return;
                }

                this.getObj('editButton').setVisible(!(oEvent.getParameter('arguments').mode === 'edit'));
                //this.getObj('deleteButton').setVisible(!(oEvent.getParameter('arguments').mode === 'edit'));

                this.setViewMode(oEvent.getParameter('arguments').mode);
                this.tenantId = oEvent.getParameter('arguments').id;
                this.systemId = oEvent.getParameter('arguments').systemId;

                this.loadView(oEvent);
            },

            setViewMode: function (value) {
                this.viewMode = value;
                this.getModel("mainModel").setProperty("/ext/viewMode", value);
            },

            loadView: function () {
                let sId;
                this.setBusy(true, true);
                if (this.tenantId === "new") {
                    sId = Constants.dataModels.emptyGuid;
                    this._setHeaderTitle("new");
                } else {
                    sId = this.tenantId;
                    this._setHeaderTitle();
                }
                this._getTenantById(sId);
            },

            _getTenantById: function (sId) {


                if (sId === Constants.dataModels.emptyGuid) {
                    if (!this.systemId)
                        this.systemId = "tenantId";

                    TenantsService.getNewTenant(this.systemId).then(response => {
                        this.setBusy(false);

                        this.getModel("mainModel").setProperty("/dictionaries", response.dictionaries);
                        this.getModel("mainModel").setProperty("/data", response.data);
                    });

                } else {
                    TenantsService.getTenantById(sId).then(response => {
                        this.setBusy(false);

                        /*if(!response.data.passwordPolicy){
                            response.data.passwordPolicy = {
                                "accountLockoutFailedLoginAttempts": 5,
                                "accountLockoutDurationMinutes": 30,
                                "allowRememberMe": false,
                                "passwordMinimumLength": 0,
                                "passwordResetFrequency": 30,
                                "previousPasswordsToRemember": 0,
                                "requireMinimumOneDayPasswordLifetime": false
                            }
                        }*/

                        this.getModel("mainModel").setProperty("/dictionaries", response.dictionaries);
                        this.getModel("mainModel").setProperty("/data", response.data);
                    });
                }
            },

            //onAfterRendering: function () {
            //    if (!"mainModel")
            //        return;

            //    this.getModel("mainModel").refresh();
            //},

            _setHeaderTitle: function (mode) {
                if (mode && mode === "new") {
                    this.getModel('mainModel').setProperty("/header", {
                        name: this.getText("createNewTenant"),
                        id: ""
                    });
                } else {
                    var oData = this.getModel('mainModel').getProperty("/data");
                    this.getModel('mainModel').setProperty("/header", {
                        name: oData['name'],
                        id: oData['systemId']
                    });
                }
            },

            onCancelPress: function () {
                MessageBox.warning(this.getText("confirmDiscardChanges"), {
                    title: "Confirm",
                    emphasizedAction: MessageBox.Action.NO,
                    actions: [MessageBox.Action.YES, MessageBox.Action.NO],
                    onClose: function (oAction) {
                        if (oAction === "YES") {
                            if (this.tenantId === "new") {
                                this.getRouter().navTo("tenants");
                            } else {
                                this.getRouter().navTo("tenantDetail", { mode: "display", id: this.tenantId });
                            }
                        }
                    }.bind(this)
                });
            },

            onEditPress: function () {
                if (this.tenantId) {
                    this.getRouter().navTo('tenantDetail', { mode: "edit", id: this.tenantId });
                }
            },

            onSavePress: function () {
                let oForm = this.getObj("tenantForm");

                if (!this.checkRequiredFormFields(oForm))
                    return;

                if (this.tenantId === "new") {
                    this._addTenant();
                } else {
                    this._editTenant();
                }
            },

            _addTenant: function () {
                var oData = this.getModel("mainModel").getProperty("/data");
                var objectPageLayout = this.getObj("objectPageLayout");

                this.setBusy(true);

                TenantsService.addTenant(oData).then(
                    response => {
                        this.setBusy(false);
                        if (response.success && response.message && response.message.text) {
                            this.messageHandler.showToast(response.message.text);
                            this.getRouter().navTo("tenantDetail", { mode: "display", id: response.data });
                        } else {
                            this.messageHandler.showMessageOutServerResponse(response);
                        }
                    },
                    oError => {
                        this.setBusy(false);
                        this.messageHandler.showSystemError(oError);
                    });
            },

            _editTenant: function () {
                var oData = this.getModel("mainModel").getProperty("/data");
                var objectPageLayout = this.getObj("objectPageLayout");

                //objectPageLayout.setBusy(true);

                TenantsService.updateTenant(oData).then(
                    function (response) {
                        objectPageLayout.setBusy(false);
                        if (response.success && response.message && response.message.text) {
                            this.messageHandler.showToast(response.message.text);
                            if(this.tenantId)
                            {
                                this.getRouter().navTo("tenantDetail", { mode: "display", id: this.tenantId });
                            } else {
                                this.getRouter().navTo("tenants", {});
                            }
                        } else {
                            this.messageHandler.showMessageOutServerResponse(response);
                        }
                    }.bind(this),
                    function (oError) {
                        this.messageHandler.showSystemError(oError);
                    }.bind(this));
            },

            onDeletePress: function () {
                var objectPageLayout = this.getObj("objectPageLayout");

                MessageBox.warning(this.getText("confirmationMsg"), {
                    title: "Alert",
                    emphasizedAction: MessageBox.Action.NO,
                    actions: [MessageBox.Action.YES, MessageBox.Action.NO],
                    onClose: function (oAction) {
                        if (oAction === "YES") {
                            objectPageLayout.setBusy(true, true);
                            TenantsService.deleteTenant(this.tenantId).then(
                                function (response) {
                                    objectPageLayout.setBusy(false);
                                    if (response.success && response.message && response.message.text) {
                                        this.getRouter().navTo("tenants");
                                       /* this.messageHandler.showToast(response.message.text);
                                        this.onCancelPress();*/
                                    } else {
                                        this.messageHandler.showMessageOutServerResponse(response);
                                    }
                                }.bind(this),
                                function (oError) {
                                    this.messageHandler.showSystemError(oError);
                                }.bind(this));
                        }
                    }.bind(this)
                });
            },

            onCustomPolicyChange: function (oEvent) {
                this.getModel('mainModel').setProperty('/customPolicyEnabled', oEvent.getParameter('state'));
            },

            /**
             * *** Sites Section ***
             */

            onAddSitePress: function () {
                if (!this._oAddSiteDialog) {
                    this._oAddSiteDialog = sap.ui.xmlfragment(this.getView().getId(),
                        "vcs.view.tenants.tenantDetail.fragments.SiteDialog", this);
                    this.getView().addDependent(this._oAddSiteDialog);
                }

                this.getModel("dialogModel").setData({ action: this.getText("add") });

                this._oAddSiteDialog.open();
            },

            onEditSitePress: function (oEvent) {

                if (!this._oEditSiteDialog) {
                    this._oEditSiteDialog = sap.ui.xmlfragment(this.getView().getId(),
                        "vcs.view.tenants.tenantDetail.fragments.SiteEditDialog", this);
                    this.getView().addDependent(this._oEditSiteDialog);
                }

                var object = oEvent.getSource().getBindingContext('mainModel').getObject();
                var oData = this.getModel("dialogEditModel").getData();

                oData.id = object.id;
                oData.name = object.name;
                oData.url = object.url;
                oData.clientId = object.clientId;
                oData.siteTypeId = object.siteTypeId;
                oData.action = this.getText("edit");
                oData.siteTypes = this.getModel("mainModel").getProperty("/dictionaries/siteTypes");
                oData.sPath = oEvent.getSource().getBindingContext('mainModel').getPath()

                this.getModel("dialogEditModel").setData(oData);

                this._oEditSiteDialog.open();
            },

            onSiteDialogSave: function (oEvent) {
                let oForm = this.getObj("siteDialogForm");

                if (!this.checkRequiredFormFields(oForm))
                    return;

                var oData = this.getModel('dialogModel').getData();
                var data = this.getModel('mainModel').getProperty("/data");

                if (oData.action === this.getText("add")) {
                    var sites = [];
                    if (oData) {
                        oData.action = 2;
                        sites = data.sites ? data.sites : [];
                        sites.push(oData);
                        data.sites = sites;
                        this.getModel('mainModel').setProperty("/data", data);
                    }
                }

                this.onSiteDialogCancel();
            },

            onSiteEditDialogSave: function (oEvent) {
                let oForm = this.getObj("siteDialogForm");

                if (!this.checkRequiredFormFields(oForm))
                    return;

                var oData = this.getModel('dialogEditModel').getData();


                if (oData.action === this.getText("edit")) {
                    var model = {};
                    model.id = oData.id;
                    model.name = oData.name;
                    model.url = oData.url;
                    model.clientId = oData.clientId;
                    model.siteTypeId = oData.siteTypeId;
                    if(!!model.id) {
                        model.action = 1;
                    }
                    this.getModel("mainModel").setProperty(oData.sPath, model);
                }

                this.onSiteEditDialogCancel();
            },

            onSiteEditDialogCancel: function () {
                this._oEditSiteDialog.close();
                this._oEditSiteDialog.destroy();
                this._oEditSiteDialog = undefined;
            },

            onSiteDialogCancel: function () {
                this._oAddSiteDialog.close();
                this._oAddSiteDialog.destroy();
                this._oAddSiteDialog = undefined;
            },

            onDeleteSitePress: function () {
                var oTable = this.getObj("sitesTableId");
                var oModel = this.getModel("mainModel");
                var oData = oModel.getData();

                oTable.getSelectedItems().forEach((item, index) => {
                    var path = item.getBindingContext("mainModel").getPath().split("/");
                    var index = path[path.length - 1];

                    oData.data.sites.splice(index, 1);
                });

                oModel.setData(oData);
            },

            /**
             * *** Services Section ***
             */

            onAddServicePress: function () {
                if (!this._oAddServiceDialog) {
                    this._oAddServiceDialog = sap.ui.xmlfragment(this.getView().getId(),
                        "vcs.view.tenants.tenantDetail.fragments.ServiceDialog", this);
                    this.getView().addDependent(this._oAddServiceDialog);
                }

                this.getModel("dialogModel").setData({ action: this.getText("add") });
                this._oAddServiceDialog.open();
            },

            onEditServicePress: function (oEvent) {

                if (!this._oAddServiceDialog) {
                    this._oAddServiceDialog = sap.ui.xmlfragment(this.getView().getId(),
                        "vcs.view.tenants.tenantDetail.fragments.ServiceEditDialog", this);
                    this.getView().addDependent(this._oAddServiceDialog);
                }

                var object = oEvent.getSource().getBindingContext('mainModel').getObject();
                var oData = this.getModel("dialogModel").getData();

                oData.id = object.id;
                oData.name = object.name;
                oData.serviceUrl = object.serviceUrl;
                oData.clientId = object.clientId;
                oData.clientSecret = object.clientSecret;
                oData.serviceTypeId = object.serviceTypeId;
                oData.action = this.getText("edit");
                oData.siteTypes = this.getModel("mainModel").getProperty("/dictionaries/serviceTypes");
                oData.sPath = oEvent.getSource().getBindingContext('mainModel').getPath()

                this.getModel("dialogModel").setData(oData);

                this._oAddServiceDialog.open();
            },

            onServiceDialogSave: function () {
                let oForm = this.getObj("siteDialogForm");

                if (!this.checkRequiredFormFields(oForm))
                    return;

                var oData = this.getModel('dialogModel').getData();
                var data = this.getModel('mainModel').getProperty("/data");

                if (oData.action === this.getText("add")) {
                    var services = [];
                    if (oData) {
                        oData.action = 2;
                        services = data.services ? data.services : [];
                        services.push(oData);
                        data.services = services;
                        this.getModel('mainModel').setProperty("/data", data);
                    }
                }

                if (oData.action === this.getText("edit")) {
                    var model = {};
                    model.id = oData.id;
                    model.name = oData.name;
                    model.serviceUrl = oData.serviceUrl;
                    model.clientId = oData.clientId;
                    model.clientSecret = oData.clientSecret;
                    model.serviceTypeId = oData.serviceTypeId;
                    if(!!model.id) {
                        model.action = 1;
                    }
                    this.getModel("mainModel").setProperty(oData.sPath, model);
                }
                this.onServiceDialogCancel();
            },

            onServiceDialogCancel: function () {
                this._oAddServiceDialog.close();
                this._oAddServiceDialog.destroy();
                this._oAddServiceDialog = undefined;
            },

            onDeleteServicePress: function () {
                var oTable = this.getObj("servicesTableId");
                var oModel = this.getModel("mainModel");
                var oData = oModel.getData();

                oTable.getSelectedItems().forEach((item, index) => {
                    var path = item.getBindingContext("mainModel").getPath().split("/");
                    var index = path[path.length - 1];

                    oData.data.services.splice(index, 1);
                });

                oModel.setData(oData);
            }
        });
    });
