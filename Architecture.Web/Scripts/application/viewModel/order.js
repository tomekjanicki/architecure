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
                    this.customerId = ko.observable(null);
                    this.customerName = ko.observable("bla");
                    this.date = ko.observable(null);
                    this.orderDetails = ko.observableArray([]);
                }
                return CreateOrder;
            })();
            Order.CreateOrder = CreateOrder;
            var CreateOrderDetail = (function () {
                function CreateOrderDetail() {
                    this.productId = ko.observable(null);
                    this.productCode = ko.observable("");
                    this.productName = ko.observable("");
                    this.quantity = ko.observable(null);
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
                    this.createOrder().customerId(2);
                    this.createOrder().customerName("CustomerName");
                    this.createOrder().date(new Date(2014, 1, 1));
                    var od1 = new CreateOrderDetail();
                    od1.productId(1003);
                    od1.productCode("ProductCode");
                    od1.productName("ProductName");
                    od1.quantity(3);
                    this.createOrder().orderDetails([od1]);
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