var RegApp = angular.module('RegApp', []).controller('RegCtrl', function ($scope) {
    $scope.agree = false;
    $scope.checkPwd = '';
    $scope.member = {};
    $scope.update = function (selectedValue) {
        // $scope.level2 = selectedValue.areas;
        $scope.level2 = $scope.towns[selectedValue]
    };
    $scope.submit = function () {
        if ($scope.agree) {
            if ($scope.checkPwd === $scope.member.password) {
                $scope.member.memberCode = Utils.FormatDate(new Date(), "yyyyMMddHHmmssSSS");
                $scope.member.tenantType = '批發商';
                $scope.member.status = '正常';
                $scope.member.creator = 1;
                if ($scope.SelArea && $scope.SelArea.includes(':')) {
                    $scope.member.townId = parseInt($scope.SelArea.split(':')[0]);
                    $scope.member.zipCode = parseInt($scope.SelArea.split(':')[1]);
                }                
                Utils.ProcessAjax("/api/member/" + Utils.GetTenantId(), "PUT", true, $scope.member,
                    function (ret) {
                        switch (ret.code) {
                            case 1:
                                alert(i18next.t("register_success"));
                                window.location = "index.html?tenantCode=" + Utils.TenantCode;
                                break;
                            case -1:
                                console.log(ret.data);
                                alert(i18next.t("msg_service_error"));
                                break;
                        }
                    },
                    function (error) { alert(i18next.t("msg_service_error"));}
                );
            } else {
                alert('please check password.');
            }
        } else {
            alert('not agree.');
        }
    };
});

var Register = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "register", Register.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Register.InitView();

        Register.getPlaces();
    },

    InitView() {

    },
    getPlaces() {
        Utils.ProcessAjax("/api/Common/getPlaces", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=RegCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.Citys = ret.data;
                    $scope.towns = [];
                    for (let i in ret.data) {
                        let item = ret.data[i];
                        $scope.towns[item.id] = item.areas;
                    }
                    $scope.$apply();
                } else {
                    alert(i18next.t("msg_service_error"));
                }
            },
            function (error) { alert(i18next.t("msg_service_error")); }
        );
    }
};

window.onload = Register.doLoad;