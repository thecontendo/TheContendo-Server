sap.ui.define([
    "vcs/data/BaseService"
], function (BaseService) {
    "use strict";

    let AuditService = BaseService.extend("vcs.data.services.global.GlobalService", {});

    AuditService.getByFilter = function (model) {
        return BaseService.callServer("POST", "/Audit/GetByFilter", model);
    };

    return AuditService;
});