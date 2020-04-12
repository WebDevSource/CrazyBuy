var QuestionApp = angular.module('QuestionApp', []).controller('QuestionCtrl', function ($scope) {
    $scope.action = function (key) {
        $scope.value = $scope.map[key];
    };
});

var Questions = {
    doLoad() {
        Utils.Initial();
        Utils.InitI18next("zh-TW", "questions", Questions.InitModule);
    },


    InitModule() {
        NavBar.Init();
        Questions.InitView();
    },

    InitView() {
        Questions.getFAQ();
    },

    getFAQ() {
        Utils.ProcessAjax("/api/tenant/getFAQ", "GET", true, "",
            function (ret) {
                if (ret.code === -1) {
                    alert("service error");
                } else {
                    let appElement = document.querySelector('[ng-controller=QuestionCtrl]');
                    let $scope = angular.element(appElement).scope();

                    let myMap = new Map();
                    let data = ret.data;
                    data.forEach(function (item, index, array) {
                        let type = item.type;
                        let list;
                        if (myMap.has(type)) {
                            list = myMap.get(type);
                        } else {
                            list = [];
                        }
                        list.push(item);
                        myMap.set(type, list);
                    });                    
                    console.log(myMap);                    
                    $scope.map = Questions.mapToData(myMap);
                    $scope.value = $scope.map[data[0].type];
                    $scope.$apply();
                }
            },
            function (error) { alert("ajax error") }
        );
    },

    mapToData(map) {
        let result = {};
        map.forEach(function (value, key) {
            result[key] = value;
        });
        return result;
    }

};

window.onload = Questions.doLoad;