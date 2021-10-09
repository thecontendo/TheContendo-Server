sap.ui.define([
    "vcs/data/BaseService"
], function (BaseService) {
    "use strict";

    var TenantsService = BaseService.extend("vcs.data.services.tenant.TenantsService", {});

    TenantsService.getAllTenants = function () {
        return BaseService.callServer("GET", "/Tenants");
    };

    TenantsService.getTenantById = function (sId) {
        return BaseService.callServer("GET", "/Tenants/" + sId);
    };

    TenantsService.getNewTenant = function (systemId) {
        return BaseService.callServer("GET", "/Tenants/GetNew/" + systemId);
    };

    TenantsService.getByFilter = function (model) {
        return BaseService.callServer("POST", "/Tenants/GetByFilter", model);
    };

    TenantsService.addTenant = function (model) {
        return BaseService.callServer("POST", "/Tenants", model);
    };

    TenantsService.deleteTenant = function (sId) {
        return BaseService.callServer("DELETE", "/Tenants/" + sId);
    };

    TenantsService.updateTenant = function (model) {
        return BaseService.callServer("PUT", "/Tenants", model);
    };

    TenantsService.getTenantDto = function (model) {

        return {
            id: model.id ? model.id : null,
            name: model.name ? model.name : "",
            timeZoneId: model.timeZoneId ? model.timeZoneId : "",
            defaultSenderEmail: model.defaultSenderEmail ? model.defaultSenderEmail : "",
            emailSignature: model.emailSignature ? model.emailSignature : "",
            customPolicy: model.customPolicy ? model.customPolicy : false,
            passwordExpiresIn: model.passwordExpiresIn ? parseInt(model.passwordExpiresIn) : 0,
            enforcePasswordHistory: model.enforcePasswordHistory ? parseInt(model.enforcePasswordHistory) : 0,
            minimumPasswordLength: model.minimumPasswordLength ? parseInt(model.minimumPasswordLength) : 0,
            maximumInvalidLoginAttempts: model.maximumInvalidLoginAttempts ? parseInt(model.maximumInvalidLoginAttempts) : 0,
            accountLockDuration: model.accountLockDuration ? parseInt(model.accountLockDuration) : 0,
            minimumOneDayPasswordLifetime: model.minimumOneDayPasswordLifetime ? model.minimumOneDayPasswordLifetime : false,
            rememberMe: model.rememberMe ? model.rememberMe : false,
            sites: model.sites ? model.sites : [],
            services: model.services ? model.services : []
        };
    };

    return TenantsService;
});