var app = angular.module('myApp', ['ui.bootstrap', 'ui.select2']);

app.controller('myCtrl', function ($scope, $http, $rootScope) {

    $scope.select2Options = {
        width: 'element'
    };

    $scope.provinces = [];
    $scope.districts = [];
    $scope.wards = [];

    $scope.cusInfo = {};


    $scope.firstLoad = function () {

        $scope.getData();

        $http.get("/user/getprovince").then(function (response) {
            $scope.provinces = angular.fromJson(response.data);

           
        });

      

    };

    $scope.provinceChange = function (type) {

        var url = '';

        if (type === 'd') {
            url = '/user/getdistrict?provinceId=' + $scope.cusInfo.ProvinceID
        } else {
            url = '/user/getward?districtId=' + $scope.cusInfo.DistrictID
        }

        $http.get(url).then(function (response) {
            if (type === 'd') {
                $scope.districts = angular.fromJson(response.data);
            } else {
                $scope.wards = angular.fromJson(response.data);
            }
        });

    };

    $scope.getData = function () {
        showLoader(true);

        $http({
            method: 'GET',
            url: '/user/getinfo'
        }).then(function sucess(response) {

            showLoader(false);

            var result = angular.fromJson(response.data);

            if (result.error === 1) {
                alert('Không lấy được dữ liệu');
            } else {
                $scope.cusInfo = result.data;
                $scope.provinceChange("d");
                $scope.provinceChange("w");
            }

        }, function error() {
            showLoader(false);
            alert('Không tải được dữ liệu');
        });

    };

    $scope.firstLoad();

    $scope.sendUpdate = function () {
        showLoader(true);
        $http({
            method: 'POST',
            url: '/user/SendUpdate',
            data: {
                data: JSON.stringify($scope.cusInfo)
            }
        }).then(function sucess(response) {

            showLoader(false);

            var result = angular.fromJson(response.data);

            if (result.error === 1) {
                alert('Không lấy được dữ liệu');
            } else {
                alert('Đã cập nhật');
            }

        }, function error() {
            showLoader(false);
            alert('Không tải được dữ liệu');
        });

    };
});