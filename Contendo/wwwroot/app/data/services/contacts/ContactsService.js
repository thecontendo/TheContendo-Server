sap.ui.define([
    "vcs/data/BaseService"
], function (BaseService) {
    "use strict";

    let ContactServiceervice = BaseService.extend("vcs.data.services.contacts.ContactsService", {});

    ContactServiceervice.getContacts = function () {
        return BaseService.callServer("GET", "/Users/GetContacts");
    };
    
    ContactServiceervice.checkContactRequests = function () {
        return BaseService.callServer("GET", `/ContactRequest/CheckContactRequests`);
    };

    ContactServiceervice.sendContactRequest = function (model) {
        return BaseService.callServer("POST", "/ContactRequest/ContactRequest", model);
    }; 
    
    ContactServiceervice.sendContactRequestByEmail = function (model) {
        return BaseService.callServer("POST", "/ContactRequest/ContactRequestByEmail", model);
    };
 
    
    ContactServiceervice.sendContactRequestResponse = function (model) {
        return BaseService.callServer("POST", "/ContactRequest/AcceptRequest", model);
    };

   

    return ContactServiceervice;
});