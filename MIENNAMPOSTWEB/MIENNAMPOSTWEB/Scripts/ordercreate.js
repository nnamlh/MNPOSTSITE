var app = angular.module('myApp', ['ui.bootstrap', 'myDirective', 'myKeyPress', 'ui.select2']);

app.controller('myCtrl', function ($scope, $http, $rootScope) {

    $scope.select2Options = {
        width: 'element'
    };
    $scope.cusInfo = {};
    $scope.mailer = {
        'SenderID': '', 'MailerID': '', 'SenderWardID': '', 'SenderDistrictID': '',
        'SenderProvinceID': '', 'RecieverName': '', 'RecieverAddress': '', 'RecieverWardID': '', 'RecieverDistrictID': '', 'RecieverProvinceID': '',
        'RecieverPhone': '', 'Weight': 0.01, 'Quantity': 1, 'PaymentMethodID': 'NGTT', 'MailerTypeID': 'SN',
        'PriceService': 0, 'MerchandiseID': 'H', 'MailerDescription': '', 'Notes': '', 'COD': 0, 'LengthSize': 0, 'WidthSize': 0, 'HeightSize': 0, 'Amount': 0, 'PriceCoD': 0,
        'PriceDefault': 0
    };
    $scope.provinces = [];
    $scope.districts = [];
    $scope.wards = [];
    $scope.merchandises = [{ 'code': 'H', 'name': 'Hàng hóa' }, { 'code': 'T', 'name': 'Tài liệu' }];
    $scope.mailerTypes = [];
    $scope.payments = [];
    $scope.firstLoad = function () {
        $scope.mailer.SenderID = $scope.cusInfo.CustomerCode;
        $scope.mailer.SenderProvinceID = $scope.cusInfo.ProvinceID;
        $scope.mailer.SenderDistrictID = $scope.cusInfo.DistrictID;
        $scope.mailer.SenderWardID = $scope.cusInfo.WardID;
        $http.get("/user/getprovince").then(function (response) {
            $scope.provinces = angular.fromJson(response.data);
        });

        $http.get("/donhang/GetSetting").then(function (response) {
            var result = angular.fromJson(response.data);

            $scope.mailerTypes = result.data.MailerTypes;

            $scope.payments = result.data.Payments;
        });
    };

    $scope.provinceChange = function (type) {

        var url = '';

        if (type === 'd') {
            url = '/user/getdistrict?provinceId=' + $scope.mailer.RecieverProvinceID
        } else {
            url = '/user/getward?districtId=' + $scope.mailer.RecieverDistrictID
        }

        $http.get(url).then(function (response) {
            if (type === 'd') {
                $scope.districts = angular.fromJson(response.data);
            } else {
                $scope.wards = angular.fromJson(response.data);
            }
        });

    };

    $scope.getSender = function () {
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

                if ($scope.cusInfo.Address === '' || $scope.cusInfo.ProvinceID === '' || $scope.cusInfo.DistrictID === '' || $scope.cusInfo.CustomerName === '') {
                    $scope.msg = 'Bạn cần cập nhật thông tin cá nhân trước khi tạo đơn hàng';

                    showModelFix('showInfo');
                } else {

                    $scope.firstLoad();
                }
            }

        }, function error() {
            showLoader(false);
            alert('Không tải được dữ liệu');
        });

    };

    $scope.getSender();


    $scope.sendOrder = function () {
        showLoader(true);
        $http({
            method: "POST",
            url: "/donhang/AddOrder",
            data: {
                data: JSON.stringify($scope.mailer)
            }
        }).then(function mySuccess(response) {
            showLoader(false);

            var result = angular.fromJson(response.data);

            if (result.error === 1) {
                alert(result.msg);
            } else {
                $scope.mailer = {
                    'SenderID': '', 'MailerID': '', 'SenderWardID': '', 'SenderDistrictID': '',
                    'SenderProvinceID': '', 'RecieverName': '', 'RecieverAddress': '', 'RecieverWardID': '', 'RecieverDistrictID': '', 'RecieverProvinceID': '',
                    'RecieverPhone': '', 'Weight': 0.01, 'Quantity': 1, 'PaymentMethodID': 'NGTT', 'MailerTypeID': 'SN',
                    'PriceService': 0, 'MerchandiseID': 'H', 'MailerDescription': '', 'Notes': '', 'COD': 0, 'LengthSize': 0, 'WidthSize': 0, 'HeightSize': 0, 'Amount': 0, 'PriceCoD': 0,
                    'PriceDefault': 0
                };
                $scope.mailer.SenderID = $scope.cusInfo.CustomerCode;
                $scope.mailer.SenderProvinceID = $scope.cusInfo.ProvinceID;
                $scope.mailer.SenderDistrictID = $scope.cusInfo.DistrictID;
                $scope.mailer.SenderWardID = $scope.cusInfo.WardID;

                alert('Đã tạo thành công');

            }

        }, function myError(response) {
            showLoader(false);
            alert('Connect error');

        });
    };

    $scope.calPrice = function () {
        showLoader(true);
        $http({
            method: "POST",
            url: "/donhang/CalBillPrice",
            data: {
                'weight': $scope.mailer.Weight,
                'customerId': $scope.mailer.SenderID,
                'provinceId': $scope.mailer.SenderProvinceID,
                'serviceTypeId': $scope.mailer.MailerTypeID
            }
        }).then(function mySuccess(response) {
            showLoader(false);
            $scope.mailer.PriceDefault = response.data.price;
            $scope.mailer.PriceCoD = response.data.codPrice;

            $scope.mailer.Amount = $scope.mailer.PriceDefault + $scope.mailer.PriceCoD + $scope.mailer.PriceService;

        }, function myError(response) {
            showLoader(false);
            alert('Connect error');

        });
    };

});