sap.ui.define([
    "vcs/data/BaseService"
], function (BaseService) {
    "use strict";

    var GlobalService = BaseService.extend("vcs.data.services.global.GlobalService", {});

    GlobalService.getLocalizations = function () {
        return BaseService.callServer("GET", "/Global/GetLocalizations");
    };

    return GlobalService;
});