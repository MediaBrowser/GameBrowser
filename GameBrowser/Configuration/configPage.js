define(['baseView', 'loading', 'emby-input', 'emby-button', 'emby-checkbox', 'emby-scroller', 'emby-select'], function (BaseView, loading) {
    'use strict';

    function View(view, params) {
        BaseView.apply(this, arguments);

        var instance = this;

        view.addEventListener('click', function (e) {
            var btnDeleteSystem = e.target.closest('.btnDeleteSystem');

            if (btnDeleteSystem) {
                instance.deleteConsoleFolder(btnDeleteSystem.getAttribute('data-systempath'));
            }

            var btnAddGameSystem = e.target.closest('.btnAddGameSystem');

            if (btnAddGameSystem) {
                instance.addGameSystem();
            }
        });
    }

    Object.assign(View.prototype, BaseView.prototype);

    View.prototype.loadConfiguration = function () {

        var instance = this;

        ApiClient.getPluginConfiguration("4C2FDA1C-FD5E-433A-AD2B-718E0B73E9A9").then(function (pluginConfig) {

            // Just in case it's empty
            pluginConfig.RootLocations = pluginConfig.RootLocations || [];

            instance.loadConsoleFolders(pluginConfig);
        });

        loading.hide();
    };

    View.prototype.loadConsoleFolders = function (pluginConfig) {

        var html = "";

        for (var i = 0, length = pluginConfig.GameSystems.length; i < length; i++) {

            var gameSystem = pluginConfig.GameSystems[i];

            html += '<div class="listItem listItem-border">';

            html += '<i class="md-icon listItemIcon">sports_esports</i>';

            html += '<div class="listItemBody two-line">';
            html += "<h3 class='listItemBodyText'>" + gameSystem.ConsoleType + "</h3>";
            html += "<div class='listItemBodyText secondary'>" + gameSystem.Path + "</div>";
            html += '</div>';

            html += '<button type="button" is="paper-icon-button-light" data-systempath="' + gameSystem.Path + '" class="btnDeleteSystem"><i class="md-icon">delete</i></button>';

            html += '</div>';
        }

        this.view.querySelector('.gameSystemList').innerHTML = html;
    };

    View.prototype.showGameSystemEditor = function (dialogHelper) {

        var instance = this;

        var dialogOptions = {
            removeOnClose: true,
            scrollY: false
        };

        dialogOptions.size = 'small';

        var dlg = dialogHelper.createDialog(dialogOptions);

        dlg.classList.add('formDialog');

        var html = '';
        var title = 'New Game System';

        html += '<div class="formDialogHeader">';
        html += '<button is="paper-icon-button-light" class="btnCancel autoSize" tabindex="-1"><i class="md-icon">&#xE5C4;</i></button>';
        html += '<h3 class="formDialogHeaderTitle">';
        html += title;
        html += '</h3>';

        html += '</div>';

        html += '<div is="emby-scroller" data-horizontal="false" data-centerfocus="card" class="formDialogContent">';
        html += '<div class="scrollSlider">';
        html += '<form class="dialogContentInner dialog-content-centered padded-left padded-right">';

        html += '<div class="selectContainer">';
        html += '<select class="selectGameSystem" is="emby-select" label="Game system:">';
        html += '<option value="3DO">3DO</option>\
                                                <option value="Amiga">Amiga</option>\
                                                <option value="Arcade">Arcade</option>\
                                                <option value="Atari 2600">Atari 2600</option>\
                                                <option value="Atari 5200">Atari 5200</option>\
                                                <option value="Atari 7800">Atari 7800</option>\
                                                <option value="Atari Jaguar">Atari Jaguar</option>\
                                                <option value="Atari Jaguar CD">Atari Jaguar CD</option>\
                                                <option value="Atari XE">Atari XE</option>\
                                                <option value="Colecovision">Colecovision</option>\
                                                <option value="Commodore 64">Commodore 64</option>\
                                                <option value="Commodore Vic-20">Commodore Vic 20</option>\
                                                <option value="DOS">DOS</option>\
                                                <option value="Intellivision">Intellivision</option>\
                                                <option value="Xbox">Microsoft XBox</option>\
                                                <option value="Xbox 360">Microsoft XBox 360</option>\
                                                <option value="Xbox One">Microsoft XBox One</option>\
                                                <option value="Neo Geo">Neo-Geo</option>\
                                                <option value="Nintendo 64">Nintendo 64</option>\
                                                <option value="Nintendo 3DS">Nintendo 3DS</option>\
                                                <option value="Nintendo DS">Nintendo DS</option>\
                                                <option value="Game Boy">Nintendo Game Boy</option>\
                                                <option value="Game Boy Advance">Nintendo Game Boy Advance</option>\
                                                <option value="Game Boy Color">Nintendo Game Boy Color</option>\
                                                <option value="Gamecube">Nintendo GameCube</option>\
                                                <option value="Nintendo">Nintendo NES</option>\
                                                <option value="Nintendo Switch">Nintendo Switch</option>\
                                                <option value="Super Nintendo">Nintendo SNES</option>\
                                                <option value="Virtual Boy">Nintendo Virtual Boy</option>\
                                                <option value="Nintendo Wii">Nintendo Wii</option>\
                                                <option value="Nintendo Wii U">Nintendo Wii U</option>\
                                                <option value="Sega 32X">Sega 32X</option>\
                                                <option value="Sega CD">Sega CD</option>\
                                                <option value="Dreamcast">Sega Dreamcast</option>\
                                                <option value="Game Gear">Sega Game Gear</option>\
                                                <option value="Sega Genesis">Sega Genesis</option>\
                                                <option value="Sega Master System">Sega Master System</option>\
                                                <option value="Sega Mega Drive">Sega Mega Drive</option>\
                                                <option value="Sega Saturn">Sega Saturn</option>\
                                                <option value="Sony Playstation">Sony Playstation</option>\
                                                <option value="PS2">Sony Playstation 2</option>\
                                                <option value="PS3">Sony Playstation 3</option>\
                                                <option value="PS4">Sony Playstation 4</option>\
                                                <option value="PSP">Sony Playstation Portable</option>\
                                                <option value="PSVita">Sony Playstation Vita</option>\
                                                <option value="TurboGrafx 16">TurboGrafx-16</option>\
                                                <option value="TurboGrafx CD">TurboGrafx CD</option>\
                                                <option value="Windows">Windows</option>\
                                                <option value="ZX Spectrum">ZX Spectrum</option>';
        html += '</select>';
        html += '</div>';

        html += '<div class="inputContainer">\
                            <div class="flex align-items-center">\
                                <div class="flex-grow">\
                                    <input is="emby-input" class="txtPath" label="Game system folder:" required="required" autocomplete="off" />\
                                </div>\
                                <button type="button" is="paper-icon-button-light" class="btnSelectPath autoSize emby-input-iconbutton"><i class="md-icon">search</i></button>\
                            </div>\
                        </div>\
';
        html += '<div class="formDialogFooter">';
        html += '<button is="emby-button" type="submit" class="raised button-submit block formDialogFooterItem"><span>Save</span></button>';
        html += '</div>';

        html += '</form>';
        html += '</div>';
        html += '</div>';

        dlg.innerHTML = html;

        dlg.querySelector('.btnCancel').addEventListener('click', function () {

            dialogHelper.close(dlg);
        });

        dlg.querySelector('.btnSelectPath').addEventListener("click", function () {
            require(['directorybrowser'], function (directoryBrowser) {

                var picker = new directoryBrowser();

                picker.show({

                    callback: function (path) {

                        if (path) {
                            var txtPath = dlg.querySelector('.txtPath');
                            txtPath.value = path;
                            txtPath.focus();
                        }
                        picker.close();
                    }
                });
            });
        });

        dlg.querySelector('form').addEventListener("submit", function (e) {

            loading.show();

            ApiClient.getPluginConfiguration("4C2FDA1C-FD5E-433A-AD2B-718E0B73E9A9").then(function (config) {

                var newEntry = true;

                var path = dlg.querySelector('.txtPath').value;
                var type = dlg.querySelector('.selectGameSystem').value;

                // need to handle updating a game system in addition to creating a new one
                if (config.GameSystems.length > 0) {
                    for (var i = 0, length = config.GameSystems.length; i < length; i++) {
                        if (config.GameSystems[i].Path === path) {
                            newEntry = false;
                            config.GameSystems[i].ConsoleType = type;
                        }
                    }
                }

                if (newEntry) {
                    var platform = {};
                    platform.Path = path;
                    platform.ConsoleType = type;
                    config.GameSystems.push(platform);
                }

                ApiClient.updatePluginConfiguration("4C2FDA1C-FD5E-433A-AD2B-718E0B73E9A9", config).then(function () {

                    loading.hide();

                    dialogHelper.close(dlg);
                    instance.loadConfiguration();
                });

                return true;
            });

            e.preventDefault();
        });

        dialogHelper.open(dlg);
    };

    View.prototype.addGameSystem = function () {

        var instance = this;

        require(['dialogHelper', 'formDialogStyle', 'emby-select', 'emby-input', 'paper-icon-button-light'], function (dialogHelper) {
            instance.showGameSystemEditor(dialogHelper);
        });
    };

    View.prototype.deleteConsoleFolder = function (systemPath) {

        var instance = this;

        require(['confirm'], function (confirm) {

            confirm({

                title: 'Delete Game System',
                text: 'Delete this game system?',
                confirmText: 'Delete',
                primary: 'cancel'

            }).then(function () {

                loading.show();

                ApiClient.getPluginConfiguration("4C2FDA1C-FD5E-433A-AD2B-718E0B73E9A9").then(function (config) {

                    var index = 0;
                    for (var i = 0, length = config.GameSystems.length; i < length; i++) {

                        if (config.GameSystems[i].Path === systemPath) {
                            index = i;
                        }
                    }

                    config.GameSystems.splice(index, 1);

                    ApiClient.updatePluginConfiguration("4C2FDA1C-FD5E-433A-AD2B-718E0B73E9A9", config).then(function () {
                        loading.hide();
                        instance.loadConfiguration();
                    });
                });
            });
        });
    };

    View.prototype.onResume = function (options) {

        BaseView.prototype.onResume.apply(this, arguments);

        var instance = this;
        instance.loadConfiguration();
    };

    return View;
});
