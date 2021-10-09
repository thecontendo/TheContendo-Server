sap.ui.define([
    "sap/ui/base/ManagedObject",
    "vcs/data/DbContext"
], function (ManagedObject, DbContext) {
    "use strict";

    var BaseService = ManagedObject.extend("vcs.data.BaseService", {
        metadata: {
            properties: {},
            events: {}
        }
    });

    BaseService.callServer = function (method, apiUrl, payload, handleErrors) {
        return DbContext.callServer(method, apiUrl, payload, handleErrors);
    };

    return BaseService;
});
