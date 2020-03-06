var OrderApp = angular.module('OrderApp', []).controller('OrderCtrl', function ($scope) {
    Order.InitView();

    $scope.action = function (id) {
        window.location = "order-detail.html?tenantId=" + Utils.GetUrlParameter('tenantId') + '&id=' + id;
    };
});

var Order = {
    doLoad() {
        Utils.Initial();

        Utils.InitI18next("zh-TW", "order", Order.InitModule);
    },


    InitModule() {
        NavBar.Init();
    },

    InitView() {
        Utils.ProcessAjax("/api/order", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=OrderCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.data = ret.data;
                    $scope.$apply();
                } else {
                    alert("service error");
                }
            },
            function (error) { alert("ajax error") }
        );
    }
};

window.onload = Order.doLoad;