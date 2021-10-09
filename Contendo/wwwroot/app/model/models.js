sap.ui.define([
    "sap/ui/model/json/JSONModel",
    "sap/ui/Device"
], function (JSONModel, Device) {
    "use strict";

    return {
        createDeviceModel: function () {
            var oModel = new JSONModel(Device);
            oModel.setDefaultBindingMode("OneWay");
            return oModel;
        },

        createGlobalModel: function () {
            var oData = {
                public: {
                }
            };

            return new JSONModel(oData);
        },

        createPasswordPolicyDictionaryModel: function () {
            var oData = {
                accountLockDuration: [
                    { text: "0", key: 0 },
                    { text: "15", key: 15 },
                    { text: "30", key: 30 },
                    { text: "45", key: 45 }
                ],

                passwordExpiresIn: [
                    { text: "Never Expires", key: 0 },
                    { text: "30 days", key: 30 },
                    { text: "60 days", key: 60 },
                    { text: "90 days", key: 90 },
                    { text: "120 days", key: 120 }
                ],

                invalidLoginAttempts: [
                    { text: "No Limit", key: 0 },
                    { text: "3", key: 3 },
                    { text: "5", key: 5 },
                    { text: "10", key: 10 }
                ]
            }

            return new JSONModel(oData);
        }

    };
});