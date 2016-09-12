﻿(function (angular, app) {
    'use strict';

    if (angular.version.major > 1) {
        throw 'Angular 1 is needed for this application';
    }

    angular.module('taxaranksApp', [])
        .service('DataSet', [
            app.data.DataSet
        ])
        .factory('SearchStringService', [
            '$http',
            app.services.SearchStringService
        ])
        .controller('TaxaRanksController', [
            'DataSet',
            'SearchStringService',
            app.controllers.TaxaRanksController
        ]);
}(window.angular, window.app));
