/// <reference path="../common.ts" />
var Application;
(function (Application) {
    var ViewModel;
    (function (ViewModel) {
        var Order;
        (function (Order) {
            "use strict";
            var CreateOrder = (function () {
                function CreateOrder() {
                    // reSharper disable InconsistentNaming
                    this.CustomerId = ko.observable(null);
                    this.CustomerName = ko.observable("bla");
                    this.Date = ko.observable(null);
                    this.OrderDetails = ko.observableArray([]);
                }
                return CreateOrder;
            })();
            Order.CreateOrder = CreateOrder;
            var CreateOrderDetail = (function () {
                function CreateOrderDetail() {
                    // reSharper disable InconsistentNaming
                    this.ProductId = ko.observable(null);
                    this.ProductCode = ko.observable("");
                    this.ProductName = ko.observable("");
                    this.Quantity = ko.observable(null);
                }
                return CreateOrderDetail;
            })();
            Order.CreateOrderDetail = CreateOrderDetail;
            var CreateViewModel = (function () {
                function CreateViewModel(command) {
                    this.command = command;
                    this.createOrder = ko.observable(new CreateOrder());
                    this.initCreateOrder();
                }
                CreateViewModel.prototype.create = function () {
                    var jsonString = ko.toJSON(this.createOrder());
                    var json = $.parseJSON(jsonString);
                    this.command.execute(json, function () {
                        window.alert("OK");
                    }, function (data) {
                        window.alert(data);
                    }, 0 /* Post */);
                };
                CreateViewModel.prototype.initCreateOrder = function () {
                    this.createOrder().CustomerId(5);
                    this.createOrder().CustomerName("CustomerName");
                    this.createOrder().Date(new Date(2014, 1, 1));
                    var od1 = new CreateOrderDetail();
                    od1.ProductId(7);
                    od1.ProductCode("ProductCode");
                    od1.ProductName("ProductName");
                    od1.Quantity(3);
                    this.createOrder().OrderDetails([od1]);
                };
                CreateViewModel.getInitializedViewModel = function (command) {
                    return new CreateViewModel(command);
                };
                return CreateViewModel;
            })();
            Order.CreateViewModel = CreateViewModel;
            ;
        })(Order = ViewModel.Order || (ViewModel.Order = {}));
    })(ViewModel = Application.ViewModel || (Application.ViewModel = {}));
})(Application || (Application = {}));
//# sourceMappingURL=order.js.map