/// <reference path="../common.ts" />

module Application.ViewModel.Order {
    "use strict";

    export class CreateOrder {
        customerId: KnockoutObservable<number> = ko.observable(null);
        customerName = ko.observable("bla");
        date: KnockoutObservable<Date> = ko.observable(null);
        orderDetails = ko.observableArray<CreateOrderDetail>([]);
    }

    export class CreateOrderDetail {
        productId: KnockoutObservable<number> = ko.observable(null);
        productCode = ko.observable("");
        productName = ko.observable("");
        quantity: KnockoutObservable<number> = ko.observable(null);
    }

    export class CreateViewModel {

        constructor(private command: Common.ICommand<CreateOrder, any, any>) {
            this.initCreateOrder();
        }

        createOrder = ko.observable(new CreateOrder());

        create(): void {
            var jsonString = ko.toJSON(this.createOrder());
            var json = $.parseJSON(jsonString);
            this.command.execute(json,
                () => {
                    window.alert("OK");
                },
                (data: any) => {
                    window.alert(data);
                }, Common.Method.Post);
        }

        private initCreateOrder(): void {
            this.createOrder().customerId(2);
            this.createOrder().customerName("CustomerName");
            this.createOrder().date(new Date(2014, 1, 1));
            var od1 = new CreateOrderDetail();
            od1.productId(1003);
            od1.productCode("ProductCode");
            od1.productName("ProductName");
            od1.quantity(3);
            this.createOrder().orderDetails([od1]);
        }

        static getInitializedViewModel(command: Common.ICommand<CreateOrder, any, any>): CreateViewModel {
            return new CreateViewModel(command);
        }

    };

}  