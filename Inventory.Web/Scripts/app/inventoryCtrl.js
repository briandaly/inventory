(function () {
    'use strict';

    //create angularjs controller
    var app = angular.module('app', []);//set and get the angular module

    app.controller('inventoryController', ['$scope', '$http', inventoryController]);

    //angularjs controller method
    function inventoryController($scope, $http) {

        //declare variable for maintain ajax load and entry or edit mode
        $scope.loading = true;
        $scope.addMode = false;

        //get all items in inventory
        $http.get('/api/Inventory/').success(function (data) {
            $scope.items = data;
            $scope.loading = false;
        })
        .error(function () {
            $scope.error = "An Error has occured while loading items!";
            $scope.loading = false;
        });

        //by pressing toggleEdit button ng-click in html, this method will be hit
        $scope.toggleEdit = function () {
            this.item.editMode = !this.item.editMode;
            $(".datepicker").datepicker(); // Resassign datapicker event handlers (lost after ajax req)
        };

        //by pressing toggleAdd button ng-click in html, this method will be hit
        $scope.toggleAdd = function () {
            $scope.addMode = !$scope.addMode;
            $(".datepicker").datepicker(); // Resassign datapicker event handlers (lost after ajax req)
        };

        //Insert item
        $scope.add = function () {
            $scope.loading = true;
            $http.post('/api/Inventory/', this.newitem).success(function (data) {
                showDialog("Added Successfully!!");
                $scope.addMode = false;
                $scope.items.push(data);
                $scope.loading = false;
            }).error(function (data) {
                $scope.error = "An Error has occured while Adding Item! " + data;
                $scope.loading = false;
            });
        };

        //Edit item
        $scope.save = function () {
            $scope.loading = true;
            var frien = this.item;
            $http.put('/api/Inventory/' + frien.id, frien).success(function (data) {
                showDialog("Saved Successfully!!");
                frien.editMode = false;
                $scope.loading = false;
            }).error(function (data) {
                $scope.error = "An Error has occured while Saving item! " + data;
                $scope.loading = false;
            });
        };

        //Delete item
        $scope.deleteitem = function () {
            $scope.loading = true;
            var Id = this.item.id;
            $http.delete('/api/Inventory/' + Id).success(function (data) {
                showDialog("Deleted Successfully!!");
                $.each($scope.items, function (i) {
                    if ($scope.items[i].id === Id) {
                        $scope.items.splice(i, 1);
                        return false;
                    }
                });
                $scope.loading = false;
            }).error(function (data) {
                $scope.error = "An Error has occured while Saving Item! " + data;
                $scope.loading = false;
            });
        };
    }
})();