var RegApp = angular.module('RegApp', []).controller('RegCtrl', function ($scope) {
    $scope.agree = false;
    $scope.checkPwd = '';
    $scope.member = {};

    $scope.submit = function () {
        if ($scope.agree) {
            if ($scope.checkPwd === $scope.member.password) {
                $scope.member.memberCode = Utils.FormatDate(new Date(), "yyyyMMddHHmmssSSS");
                $scope.member.tenantType = '批發商';
                $scope.member.status = '正常';
                $scope.member.creator = 1;
                Utils.ProcessAjax("/api/member/" + Utils.GetTenantId(), "PUT", true, $scope.member,
                    function (ret) {
                        switch (ret.code) {
                            case 1:
                                alert('register successful.');
                                window.location = "index.html?tenantCode=" + Utils.TenantCode;
                                break;
                            case -1:
                                alert(ret.data);
                                break;
                        }
                    },
                    function (error) { alert("ajax error") }
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
    },

    InitView() {

    }
};

window.onload = Register.doLoad;