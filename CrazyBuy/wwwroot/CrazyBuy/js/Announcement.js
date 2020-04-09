var cartApp = angular.module('AnnoApp', []).controller('AnnoCtrl', function ($scope) {

});

var Announcement = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "announcement", Announcement.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Announcement.InitView();
    },

    InitView() {
        Announcement.getData();
    },

    getData() {
        Utils.ProcessAjax("/api/tenant/getBulletin", "GET", true, "",
            function (ret) {
                if (ret.code === -1) {
                    alert("service error");
                } else {
                    let appElement = document.querySelector('[ng-controller=AnnoCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.data = ret.data;
                    $scope.$apply();
                }
            },
            function (error) { alert("ajax error") }
        );
    },

};

window.onload = Announcement.doLoad;