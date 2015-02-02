(function() {
    'use strict';

    angular.module('np.layout')
        .controller('HeaderController', headerController);

    headerController.$inject = ['$scope', '$http', 'Strings', 'Auth', 'EventsHub', 'Signin', 'User', 'Profile'];

    function headerController($scope, $http, Strings, Auth, EventsHub, Signin, User, Profile) {
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

        function buildMenus() {
            ctrl.mainMenu = getMainMenu();
        }

        function signedIn() {
            getProfile();
        }

        function signedOut() {
            ctrl.login.authenticated = Auth.authenticated();
        }

        function profileUpdated() {
            ctrl.login.profileThumbnail = User.getUserData().thumbnail;
        }

        function getProfile() {
            var userName = User.getUserData().userName;
            Profile.getOwnProfile(userName).then(getProfileSuccess, getProfileError);
        }

        function getProfileError() {
            Auth.signOut();
        }

        function getProfileSuccess(response) {
            ctrl.login.profileThumbnail = response.data.publicInfo.profileImageThumbnail;
            ctrl.login.userName = response.data.publicInfo.user.userName;
            ctrl.login.authenticated = Auth.authenticated();
        }

        function activate() {
            if (Auth.authenticated()) {
                var userData = User.getUserData();
                if (userData && userData.userName) {
                    getProfile(userData.userName);
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
            EventsHub.addListener(EventsHub.events.Profile.Updated, profileUpdated);

            $scope.$on('$destroy', function() {
                EventsHub.removeListener(EventsHub.events.Auth.SignedIn, signedIn);
                EventsHub.removeListener(EventsHub.events.Auth.SignedOut, signedOut);
                EventsHub.removeListener(EventsHub.events.Profile.Updated, profileUpdated);
            });
        }
    }
})();