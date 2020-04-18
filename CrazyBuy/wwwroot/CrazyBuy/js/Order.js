var OrderApp = angular.module('OrderApp', []).controller('OrderCtrl', function ($scope) {   
    $scope.sendId;
    $scope.action = function (id) {
        Order.checkDetail(id);
    };

    $scope.contact = function (id) {
        $scope.sendId = id;
        Order.getDetailData(id);
    };

    $scope.submit = function () {
        let args = {
            orderId: $scope.sendId,
            ContactContent: $scope.send.ContactContent
        };
        Order.addContactItem(args);
    };
    $scope.useSample = function () {
        $scope.contact.ContactContent = "匯款日：\n末五碼：\n備註：";
    };
});

var Order = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "order", Order.InitModule);
    },


    InitModule() {
        Utils.checkRole();
        NavBar.Init();
        Order.InitView();
        Order.getBankInfo();
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
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error"));}
        );
    },

    checkDetail(id) {
        window.location = "order-detail.html?tenantCode=" + Utils.TenantCode + '&id=' + id;
    },
    
    addContactItem(args) {
        Utils.ProcessAjax("/api/OrderContactItem", "PUT", true, args,
            function (ret) {
                if (ret.code === 1) {
                    alert(i18next.t("swal_createSuccess")); 
                    Order.getDetailData(args.orderId);
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error"));}
        );
    },

    getDetailData(id) {
        Utils.ProcessAjax("/api/order/" + id, "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=OrderCtrl]');
                    let $scope = angular.element(appElement).scope();
                    
                    $scope.now = new Date();
                    $scope.send = {};
                    $scope.send.ContactContent = "匯款日：\n末五碼：\n備註：";
                    $scope.contactList = ret.data.contactList;                    
                    $scope.$apply();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },
    getBankInfo() {
        Utils.ProcessAjax("/api/Common/getBankInfo", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=OrderCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.bank = ret.data.bank;
                    $scope.$apply();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },
};

window.onload = Order.doLoad;