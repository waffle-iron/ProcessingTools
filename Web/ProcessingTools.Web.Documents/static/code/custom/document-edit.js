﻿(function (window, document, $) {
    'use strict';

    const
        LAST_GET_TIME_KEY = 'LAST_GET_TIME_KEY_EDIT',
        LAST_SAVED_TIME_KEY = 'LAST_SAVED_TIME_KEY_EDIT',
        CONTENT_HASH_KEY = 'CONTENT_HASH_KEY_EDIT',
        EDITOR_CONTAINER_ID = 'editor-container',
        GET_LINK_ID = 'get-link',
        SAVE_LINK_ID = 'save-link',
        SAVE_BUTTON_ID = 'save-button',
        REFRESH_BUTTON_ID = 'refresh-button';

    var app = window.app,
        sessionStorage = window.sessionStorage,
        monacoEditor = new app.configurations.MonacoEditor(window, document),
        jsonRequester = new app.services.JsonRequester($),
        documentController = new app.controllers.DocumentController(sessionStorage, LAST_GET_TIME_KEY, LAST_SAVED_TIME_KEY, CONTENT_HASH_KEY, jsonRequester),
        sha1 = window.CryptoJS.SHA1;

    // Register get-content
    window.getLinkAddress = document.getElementById(GET_LINK_ID).href;
    documentController.registerGetAction(function (content) {
        var contentHash;
        if (content) {
            window.editor.setValue(content);
            contentHash = sha1(window.editor.getValue()).toString();
            sessionStorage.setItem(CONTENT_HASH_KEY, contentHash);
        }
    });

    // Register save-content
    window.saveLinkAddress = document.getElementById(SAVE_LINK_ID).href;
    documentController.registerSaveAction(function () {
        return window.editor.getValue();
    });

    monacoEditor.init(EDITOR_CONTAINER_ID, '../../../node_modules')
        .then(function (data) {
            var i, len, option, modes, themes;

            if (data) {
                modes = data.modes;
                themes = data.themes;
            }

            // Fetch content
            window.get();

            // Populate modes and themes
            if (themes) {
                len = themes.length;
                for (i = 0; i < len; i += 1) {
                    option = document.createElement('option');
                    option.textContent = themes[i].display;
                    option.selected = themes[i].selected;
                    $(".theme-picker").append(option);
                }

                $(".theme-picker").change(function () {
                    var index = this.selectedIndex,
                        $body = $('body'),
                        $monacoEditor = $('.monaco-editor');
                    monacoEditor.changeTheme(themes[index]);
                    if (index > 0) {
                        // Not the default theme
                        $('.navbar-fixed-bottom').removeClass('navbar-default').addClass('navbar-inverse');
                    } else {
                        $('.navbar-fixed-bottom').removeClass('navbar-inverse').addClass('navbar-default');
                    }

                    if ($monacoEditor) {
                        $body.css({
                            'color': $monacoEditor.css('color'),
                            'background-color': $monacoEditor.css('background-color')
                        });
                    }
                });
            }

            if (modes) {
                len = modes.length;
                for (i = 0; i < len; i += 1) {
                    option = document.createElement('option');
                    option.textContent = modes[i].modeId;
                    option.selected = modes[i].selected;
                    $(".language-picker").append(option);
                }

                $(".language-picker").change(function () {
                    monacoEditor.changeMode(modes[this.selectedIndex]);
                });
            }
        });

    // Event handlers
    function getContentEventHandler() {
        window.get();
    }

    function saveContentEventHandler() {
        window.save();
    }

    // Events registration
    document
        .getElementById(SAVE_BUTTON_ID)
        .addEventListener('click', saveContentEventHandler, false);
    document
        .getElementById(REFRESH_BUTTON_ID)
        .addEventListener('click', getContentEventHandler, false);

}(window, window.document, window.jQuery));