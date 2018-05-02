(function () {
    "use strict";

    angular.module("ngCrmTable", [])
        .directive("loadingContainer", function () {
            return {
                restrict: "A",
                scope: false,
                link: function (scope, element, attrs) {
                    var loadingLayer = angular.element("<div class='loading'></div>");
                    element.append(loadingLayer);
                    element.addClass("loading-container");
                    scope.$watch(attrs.loadingContainer, function (value) {
                        loadingLayer.toggleClass("ng-hide", !value);
                    });
                }
            };
        })
        .factory("ngCrmTableList", [function () {
            return data;
        }]);

    angular.module("App", ["ngTable", "ngCrmTable"])
        .controller("dynamicCrmController", ["NgTableParams", "ngCrmTableList", function (NgTableParams, simpleList) {
            this.cols = cols;

            this.tableParams = new NgTableParams(
                { sorting: { age: "desc" } },
                { dataset: simpleList });
        }])
        .run(["ngTableDefaults", function (ngTableDefaults) {
            ngTableDefaults.params.count = 10;
            ngTableDefaults.settings.counts = [];
        }]);
})();