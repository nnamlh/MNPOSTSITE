var app = angular.module('myApp', []);

app.controller('myCtrl', function ($scope, $http) {
    $scope.mailer = {
        'SenderProvinceID': '2',
        'RecieverProvinceID': '2',
        'RecieverDistrictID': '43',
        'SenderDistrictID': '43',
        'MailerTypeID': 'SN',
        'COD' : '0'
    };
    $scope.provinceChange = function (pType, type) {

        var url = '/home/GetProvinces?';
        console.log(url);
        if (type === 'send') {
            url = url + "parentId=" + $scope.mailer.SenderProvinceID + "&type=district";
        } else {
            url = url + "parentId=" + $scope.mailer.RecieverProvinceID + "&type=district";
        }

        $http.get(url).then(function (response) {

            if (type === 'send') {
                $scope.districtSend = angular.copy(response.data);

            } else {
                $scope.districtRecei = angular.copy(response.data);

            }

        });

    }

    $scope.dichvues = [];
    $scope.baiviets = [];
    $scope.provinceSend = provinceSendGet;
    $scope.provinceRecei = angular.copy($scope.provinceSend);
   
    $scope.districtSend = [];

    $scope.mailerTypes = mailerTypesGet;
    $scope.districtRecei = [];

    $scope.provinceChange('district', 'send');

    $scope.provinceChange('district', 'recei');
    $scope.calPrice = function () {

        $http({
            method: "POST",
            url: "/home/CalBillPrice",
            data: {
                'weight': $scope.mailer.Weight,
                'provinceId': $scope.mailer.RecieverProvinceID,
                'serviceTypeId': $scope.mailer.MailerTypeID,
                'cod': $scope.mailer.COD,
                'districtId': $scope.mailer.RecieverDistrictID
            }
        }).then(function mySuccess(response) {
            console.log(response.data);
            $scope.mailer.PriceDefault = response.data.price;
        }, function myError(response) {
            alert('Connect error');
        });
    };
    $scope.GetData = function () {
        /*
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
        */

        $http.get("/home/getbaiviets").then(function (response) {

            $scope.baiviets = response.data;

        });
    };

    $scope.GetData();

});