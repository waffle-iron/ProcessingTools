﻿(function (window) {
    'use strict';
    var app, controllers;

    window.app = window.app || {};
    app = window.app;

    app.controllers = app.controllers || {};
    controllers = app.controllers;

    controllers.BiotaxonomicBlackListController = function BiotaxonomicBlackListController(dataSet, searchService) {
        var self = this,
            BlackListItem = app.models.BlackListItem;

        self.items = dataSet.data;

        self.addItem = function () {
            var items, text = self.textArea || '';
            text = text.replace(/\s+/g, ' ').trim();
            if (text === '') {
                return;
            }

            text = text.replace(/(\S+)\s+/g, '$1\n');
            items = text.split('\n');

            dataSet.addMulti(items, (e) => new BlackListItem(e));

            self.textArea = '';
        };

        self.removeItem = function (id) {
            dataSet.remove(id);
        };

        self.clearList = function () {
            dataSet.removeAll();
        };

        self.search = function (url) {
            var searchString = self.searchString || '';
            searchString = searchString.replace(/\s+/g, ' ').trim();
            if (!url || searchString === '') {
                return;
            }

            searchService.search(url, searchString)
                .then(function successCallback(response) {
                    if (response.status === 200) {
                        dataSet.addMulti(response.data.Items, (e) => new BlackListItem(e.Content));
                    }
                }, function errorCallback(response) { });
        };
    };
}(window));
