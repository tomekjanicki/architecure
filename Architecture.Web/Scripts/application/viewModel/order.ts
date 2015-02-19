/// <reference path="../common.ts" />

module Application.ViewModel.Order {
    "use strict";

    export class CreateOrder {
        // reSharper disable InconsistentNaming
        public CustomerId: KnockoutObservable<number> = ko.observable(null);
        public CustomerName: KnockoutObservable<string> = ko.observable("bla");
        public Date: KnockoutObservable<Date> = ko.observable(null);
        public OrderDetails: KnockoutObservableArray<CreateOrderDetail> = ko.observableArray([]);
        // reSharper restore InconsistentNaming
    }

    export class CreateOrderDetail {
        // reSharper disable InconsistentNaming
        public ProductId: KnockoutObservable<number> = ko.observable(null);
        public ProductCode: KnockoutObservable<string> = ko.observable("");
        public ProductName: KnockoutObservable<string> = ko.observable("");
        public Quantity: KnockoutObservable<number> = ko.observable(null);
        // reSharper restore InconsistentNaming
    }

    export class CreateViewModel {
        private command: Common.ICommand<CreateOrder, any, any>;

        constructor(command: Common.ICommand<CreateOrder, any, any>) {
            this.command = command;
            this.initCreateOrder();
        }

        public createOrder: KnockoutObservable<CreateOrder> = ko.observable(new CreateOrder());

        public create(): void {
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
            var od1: CreateOrderDetail = new CreateOrderDetail();
            od1.ProductId(7);
            od1.ProductCode("ProductCode");
            od1.ProductName("ProductName");
            od1.Quantity(3);
            this.createOrder().OrderDetails([od1]);
        }

        public static getInitializedViewModel(command: Common.ICommand<CreateOrder, any, any>): CreateViewModel {
            return new CreateViewModel(command);
        }

    };

}  