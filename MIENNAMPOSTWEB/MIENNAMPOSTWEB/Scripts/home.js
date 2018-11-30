var app = angular.module('myApp', []);

app.controller('myCtrl', function ($scope, $http) {

    $scope.dichvues = [];
    $scope.baiviets = [];

    $scope.GetData = function () {

        $http.get("/home/getdichvues").then(function (response) {

            $scope.dichvues = response.data;
            for (var i = 0; i < $scope.dichvues.length; i++) {
                if (i === 0) {
                    $scope.dichvues[i].isActive = true;
                } else {
                    $scope.dichvues[i].isActive = false;
                }
                $scope.dichvues[i].tabid = 'tabdichvu' + i;

            }
        });


        $http.get("/home/getbaiviets").then(function (response) {

            $scope.baiviets = response.data;

        });
    };

    $scope.GetData();

});