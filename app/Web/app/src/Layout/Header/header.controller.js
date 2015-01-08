(function() {
    'use strict';

    angular.module('np.layout')
        .controller('HeaderController', headerController);

    headerController.$inject = ['$scope', '$http', 'Strings', 'Auth', 'EventsHub', 'Signin', 'Config', 'Url', 'User'];

    function headerController($scope, $http, Strings, Auth, EventsHub, Signin, Config, Url, User) {
        var ctrl = this;
        ctrl.mainMenu = [];
        ctrl.signin = signin;
        ctrl.signout = signout;
        ctrl.login = {};
        //ctrl.togglePopup = togglePopup;
        //ctrl.popupVisible = false;

        activate();

        //function togglePopup() {
        //    ctrl.popupVisible = !ctrl.popupVisible;
        //}

        function signin() {
            Signin.open();
        }

        function signout() {
            //ctrl.popupVisible = false;
            Auth.signOut();
        }

        function getMainMenu() {
            var gallery = {
                    text: Strings.getLocalizedString('MainMenu_Gallery'),
                    uri: 'home'
                },
                exhibition = {
                    text: Strings.getLocalizedString('MainMenu_Gallery'),
                    uri: 'exhibition'
                },
                projects = {
                    text: Strings.getLocalizedString('MainMenu_Gallery'),
                    uri: 'projects'
                };

            var menu = [
                gallery,
                exhibition,
                projects
            ];

            return menu;
        }

        function getSignInMenu() {
            return {
                signOut: Strings.getLocalizedString('AuthMenu_SignOut'),
                signIn: Strings.getLocalizedString('AuthMenu_SignIn')
            };
        }

        function buildMenus() {
            ctrl.mainMenu = getMainMenu();
        }

        function signedIn() {
            getHome();
        }

        function signedOut() {
            ctrl.login.authenticated = Auth.authenticated();
        }

        function getHome() {
            var userName = User.getUserData().userName;
            var uri = Url.build(Config.ApiUris.Base, Config.ApiUris.Users.Home).formatNamed({ userName: userName });
            $http.get(uri).then(getHomeSuccess, getHomeError);
        }

        function getHomeError() {
            Auth.signOut();
        }

        function getHomeSuccess(response) {
            ctrl.login.profileThumbnail = response.data.thumbnail;
            ctrl.login.userName = response.data.user.userName;
            ctrl.login.authenticated = Auth.authenticated();
            User.updateUserData({
                thumbnail: response.data.thumbnail,
                fullName: response.data.user.fullName
            });
        }

        function activate() {
            if (Auth.authenticated()) {
                var userData = User.getUserData();
                if (userData && userData.userName) {
                    getHome(userData.userName);
                } else {
                    Auth.signOut();
                }
            }
            else {
                ctrl.login.authenticated = Auth.authenticated();
            }
            
            buildMenus();

            EventsHub.addListener(EventsHub.events.Auth.SignedIn, signedIn);
            EventsHub.addListener(EventsHub.events.Auth.SignedOut, signedOut);

            $scope.$on('$destroy', function() {
                EventsHub.removeListener(EventsHub.events.Auth.SignedIn, signedIn);
                EventsHub.removeListener(EventsHub.events.Auth.SignedOut, signedOut);
            });
        }
    }
})();