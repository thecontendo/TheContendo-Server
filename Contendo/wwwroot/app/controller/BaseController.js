sap.ui.define([
    "sap/ui/core/mvc/Controller",
    "sap/ui/core/routing/History",
    "sap/ui/model/json/JSONModel",
    "vcs/helpers/MessageHelper",
    "vcs/model/Constants"
],
    function (Controller, History, JSONModel, MessageHelper, Constants) {
        "use strict";

        return Controller.extend("vcs.controller.BaseController", {
            onInit: function () {
                this.messageHandler = MessageHelper;
            },

            getComponent: function () {
                return this.getOwnerComponent();
            },

            getRouter: function () {
                return this.getOwnerComponent().getRouter();
            },

            getModel: function (sName) {
                return this.getView().getModel(sName);
            },

            getGlobalModel: function () {
                return this.getOwnerComponent().getModel("globalModel");
            },

            setModel: function (oModel, sName) {
                return this.getView().setModel(oModel, sName);
            },

            getResourceBundle: function () {
                return this.getOwnerComponent().getModel("i18n").getResourceBundle();
            },

            getText: function (key, args) {
                var resourceBundle = this.getResourceBundle();
                return resourceBundle.getText(key, args);
            },

            getObj: function (sName) {
                return this.getView().byId(sName);
            },

            createViewModel: function (oModel) {
                var oData = {
                    data: {},
                    dictionaries: {},
                    filters: {},
                    busy: false,
                    ext: oModel
                };

                return new JSONModel(oData);
            },

            onNavBack: function () {
                var sPreviousHash = History.getInstance().getPreviousHash();

                if (sPreviousHash !== undefined) {
                    window.history.go(-1);
                }
                else {
                    this.getRouter().navTo("", {}, true);
                    window.location.reload();
                }
            },

            checkRequiredFormFields: function (oForm) {
                let result = true;
                if (!oForm)
                    return;

                oForm.getContent().forEach(control => {
                    let controlType = control.getMetadata().getName();

                    switch (controlType) {
                        case "sap.m.Input": {
                            if (control.getRequired() && control.getValue() == "") {
                                control.setValueState(sap.ui.core.ValueState.Error);
                                control.setValueStateText(this.getText("valueIsRequired"));
                                control.attachLiveChange(this.formValidationChangeHandler.bind(this));
                                result = false;
                            }
                        };
                            break;

                        case "sap.m.ComboBox": {
                            if (control.getRequired() && (control.getSelectedKey() == "" || control.getSelectedKey() == Constants.dataModels.emptyGuid)) {
                                control.setValueState(sap.ui.core.ValueState.Error);
                                control.setValueStateText(this.getText("valueIsRequired"));
                                control.attachSelectionChange(this.formValidationChangeHandler.bind(this));
                                result = false;
                            }
                        };

                        default:
                    }
                });

                return result;
            },

            formValidationChangeHandler: function (control) {
                control = control.getSource();
                let controlType = control.getMetadata().getName();

                switch (controlType) {
                    case "sap.m.Input": {
                        if (control.getValue() !== "") {
                            control.setValueState(sap.ui.core.ValueState.None);
                            control.setValueStateText("");
                            control.detachLiveChange(this.formValidationChangeHandler.bind(this));
                        }
                    };
                        break;

                    case "sap.m.ComboBox": {
                        if (control.getSelectedKey() !== "" && control.getSelectedKey() !== Constants.dataModels.emptyGuid) {
                            control.setValueState(sap.ui.core.ValueState.None);
                            control.setValueStateText("");
                            control.detachSelectionChange(this.formValidationChangeHandler.bind(this));
                        }
                    };

                    default:
                }
            },

            securityTokenIsAvailable: function () {
                return sessionStorage.getItem("vcs.token");
            },

            setBusy: function (value, noDelay) {
                if(noDelay && value){
                    sap.ui.core.BusyIndicator.show(0);
                } else if (value) {
                    sap.ui.core.BusyIndicator.show(1500);
                } else {
                    sap.ui.core.BusyIndicator.hide();
                }
            },

            setVBusy: function(oControl, flag) {
                let oElement = oControl.oView ? oControl.oView : oControl;
                if(flag) {
                    oElement.setBusyIndicatorDelay( 0 );
                }
                oElement.setBusy(flag);
            }
        });
    });
