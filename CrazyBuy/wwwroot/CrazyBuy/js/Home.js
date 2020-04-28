var HomeApp = angular.module('HomeApp', []).controller('HomeCtrl', function ($scope) {

});
var Home = {
    doLoad() {
        Utils.Initial();

        Utils.InitI18next("zh-TW", "home", Home.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Home.InitView();
    },

    InitView() {
        Home.InitProductData();
        Home.getImageData();
    },

    InitProductData(role) {

        Utils.ProcessAjax("/api/prd/getHomePrdList", "GET", true, "",
            function (ret) {
                Home.initProductList(ret.data);
            },
            function (error) {
                alert(i18next.t("msg_tenant_not_find"));
            }
        );

    },

    initProductList(data) {
        let role = Utils.getRole();
        let html = '';
        for (let item in data) {
            html += ProductCard.getHtml(data[item], role);
        }
        $('.items-card-row').html(html);
    },
    getImageData() {
        Utils.ProcessAjax("/api/tenant/getBulletin", "GET", true, "",
            function (ret) {
                if (ret.code === -1) {
                    alert(i18next.t("msg_service_error"));
                } else {
                    let appElement = document.querySelector('[ng-controller=HomeCtrl]');
                    let $scope = angular.element(appElement).scope();
                    let images = [];
                    let rightUp = [];
                    let rightDown = [];
                    let home = [];
                    for (let i = 0; i < ret.data.length; i++) {
                        let isHomeImage = false;
                        let item = ret.data[i];
                        let url = Utils.GetBulletinImageUrl(item);
                        if (i18next.t("home_carousel") == item.layout) {
                            home.push(url);
                            isHomeImage = true;
                        } else if (i18next.t("home_carousel_rightUp") == item.layout) {
                            isHomeImage = true;
                            rightUp.push(url);
                        } else if (i18next.t("home_carousel_rightDown") == item.layout) {
                            isHomeImage = true;
                            rightDown.push(url);
                        }
                        if (isHomeImage) {
                            images.push(url);
                        }
                    }
                    $scope.images = images;
                    $scope.home = home;
                    $scope.rightUp = rightUp;
                    $scope.rightDown = rightDown;
                    $scope.$apply();
                }
            },
            function (error) { alert(i18next.t("ajax error")) }
        );
    },

};

window.onload = Home.doLoad;