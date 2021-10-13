sap.ui.define([
    "vcs/data/BaseService"
], function (BaseService) {
    "use strict";

    let ChallengeService = BaseService.extend("vcs.data.services.challenge.ChallengeService", {});

    ChallengeService.getAllChallenges = function () {
        return BaseService.callServer("GET", "/Challenge");
    };

    ChallengeService.getChallengeById = function (sId) {
        return BaseService.callServer("GET", "/Challenge/" + sId);
    };

    ChallengeService.sendChallenge = function (model) {
        return BaseService.callServer("POST", "/Challenge", model);
    };

    ChallengeService.getAllShots = function () {
        return BaseService.callServer("GET", "/Shots");
    };

    ChallengeService.getShotById = function (sId) {
        return BaseService.callServer("GET", "/Shots/" + sId);
    };
    
    return ChallengeService;
});