var MemberApp = angular.module('MemberApp', []).controller('MemberCtrl', function ($scope) {
    $scope.agree = false;
    $scope.checkPwd = '';
    $scope.member = {};
    MemberInfo.GetMemberData();

    $scope.submit = function () {
        if ($scope.agree) {
            if ($scope.checkPwd === $scope.member.password) {
                Utils.ProcessAjax("/api/member/" + $scope.member.memberId, "POST", true, $scope.member,
                    function (ret) {
                        switch (ret.code) {
                            case 1:
                                alert('update successful.');
                                window.location = "index.html?tenantId=" + Utils.GetUrlParameter('tenantId');
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

var MemberInfo = {
    doLoad() {
        Utils.Initial();

        Utils.InitI18next("zh-TW", "memberInfo", MemberInfo.InitModule);
    },


    InitModule() {
        NavBar.Init();
        MemberInfo.InitView();
    },

    InitView() {

    },

    GetMemberData() {
        Utils.ProcessAjax("/api/member", "GET", true, "",
            function (ret) {
                if (ret.code === 1) {
                    let appElement = document.querySelector('[ng-controller=MemberCtrl]');
                    let $scope = angular.element(appElement).scope();
                    $scope.member = ret.data;
                    $scope.$apply();
                } else {
                    alert("service error");
                }
            },
            function (error) { alert("ajax error") }
        );
    }
};

window.onload = MemberInfo.doLoad;