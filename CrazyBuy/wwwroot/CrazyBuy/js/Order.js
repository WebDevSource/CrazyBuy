var OrderApp = angular.module('OrderApp', []).controller('OrderCtrl', function ($scope) {
    $scope.sendId;
    $scope.search = {};
    $scope.action = function (id) {
        Order.checkDetail(id);
    };

    $scope.searchQuery = function () {
        $scope.search.startDate = $('input[name="daterange"]').data('daterangepicker').startDate.format('YYYY-MM-DD');
        $scope.search.endDate = $('input[name="daterange"]').data('daterangepicker').endDate.format('YYYY-MM-DD');
        console.log($scope.search);
        Order.querySearch($scope.search);
    };

    $scope.contact = function (id) {
        $scope.sendId = id;
        Order.getDetailData(id);
    };

    $scope.submit = function () {
        let args = {
            orderId: $scope.sendId,
            ContactContent: $scope.contact.ContactContent
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
        $(document).ready(function () {
            $('.js-example-basic-multiple').select2({
                placeholder: "請選擇",
                theme: "classic",
                width: 'resolve'

            });
        });
        
    },

    doClearSearch() {
        let appElement = document.querySelector('[ng-controller=OrderCtrl]');
        let $scope = angular.element(appElement).scope();
        $scope.search = {};
        //$('.selectpicker').selectpicker('deselectAll');
        $('.js-example-basic-multiple').val(null).trigger('change');

        Order.InitView()
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
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    },

    querySearch(search) {
        Utils.ProcessAjax("/api/order", "POST", true, search,
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
            function (error) { alert(i18next.t("msg_service_error")); }
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
            function (error) { alert(i18next.t("msg_service_error")); }
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


$(function () {
    $('input[name="daterange"]').daterangepicker({}, function (start, end, label) {
        let appElement = document.querySelector('[ng-controller=OrderCtrl]');
        let $scope = angular.element(appElement).scope();
        $scope.search.startDate = start.format('YYYY-MM-DD');
        $scope.search.endDate = end.format('YYYY-MM-DD');
    });
});

window.onload = Order.doLoad;