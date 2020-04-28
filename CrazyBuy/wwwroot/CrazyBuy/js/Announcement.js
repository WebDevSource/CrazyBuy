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
                    alert(i18next.t("msg_service_error"));
                } else {
                    let appElement = document.querySelector('[ng-controller=AnnoCtrl]');
                    let $scope = angular.element(appElement).scope();
                    let data = [];
                    for (let i = 0; i < ret.data.length; i++) {
                        let item = ret.data[i];
                        if (i18next.t("announcement") == item.layout) {
                            data.push(Utils.GetBulletinImageUrl(item));
                        }
                    }
                    $scope.data = data;
                    //$scope.url = Utils.GetBulletinImageUrl(data[0]);
                    $scope.$apply();
                }
            },
            function (error) { alert(i18next.t("ajax error")) }
        );
    },

};

window.onload = Announcement.doLoad;