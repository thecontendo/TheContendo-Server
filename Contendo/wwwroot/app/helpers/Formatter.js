sap.ui.define(["vcs/helpers/Constants"
], function (Contants) {
    "use strict";
    return {

        getEmailTemplateIcon: function (type) {
            switch (type) {
                case Contants.emailTemplateType.Text:
                    return "sap-icon://attachment-text-file";
                    break;
                case Contants.emailTemplateType.HTML:
                    return "sap-icon://attachment-html";
                    break;
            }
            return undefined;
        },

        setDateValue: function (string) {
            return new Date(string).toDateString();
        },

        setStatusText: function (id) {
            switch (id) {
                case 0:
                    return this.getText("active");
                    break;
                case 1:
                    return this.getText("inactive");
                    break;
                case 2:
                    return this.getText("new");
                    break;
                default:
                    return "";
            }
        },

        setStatusState: function (id) {
            switch (id) {
                case 0:
                    return "Success";
                    break;
                case 1:
                    return "Warning";
                    break;
                case 2:
                    return "Error";
                    break;
                default:
                    return "Error";
            }
        }

    }
}, true);