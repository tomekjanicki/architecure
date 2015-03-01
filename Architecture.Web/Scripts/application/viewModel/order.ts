/// <reference path="../common.ts" />

module Application.ViewModel.Order {
    "use strict";

    export class CreateOrder {
        // reSharper disable InconsistentNaming
        CustomerId: KnockoutObservable<number> = ko.observable(null);
        CustomerName = ko.observable("bla");
        Date: KnockoutObservable<Date> = ko.observable(null);
        OrderDetails = ko.observableArray<CreateOrderDetail>([]);
        // reSharper restore InconsistentNaming
    }

    export class CreateOrderDetail {
        // reSharper disable InconsistentNaming
        ProductId: KnockoutObservable<number> = ko.observable(null);
        ProductCode = ko.observable("");
        ProductName = ko.observable("");
        Quantity: KnockoutObservable<number> = ko.observable(null);
        // reSharper restore InconsistentNaming
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
            this.createOrder().CustomerId(5);
            this.createOrder().CustomerName("CustomerName");
            this.createOrder().Date(new Date(2014, 1, 1));
            var od1 = new CreateOrderDetail();
            od1.ProductId(7);
            od1.ProductCode("ProductCode");
            od1.ProductName("ProductName");
            od1.Quantity(3);
            this.createOrder().OrderDetails([od1]);
        }

        static getInitializedViewModel(command: Common.ICommand<CreateOrder, any, any>): CreateViewModel {
            return new CreateViewModel(command);
        }

    };

}  