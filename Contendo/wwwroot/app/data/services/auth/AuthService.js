sap.ui.define([
    "vcs/data/BaseService"
], function (BaseService) {
    "use strict";

    var AuthService = BaseService.extend("vcs.data.services.auth.AuthService", {});

    AuthService.getToken = function (model) {
        return BaseService.callServer("POST", "/InternalAuth/Token", model);
    }

    return AuthService;
});
