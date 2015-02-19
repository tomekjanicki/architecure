/// <reference path="../viewmodel/order.ts" />

module Application.View.Order.Create {
    "use strict";

    var command = new Common.Command<Application.ViewModel.Order.CreateOrder, any, any>("/api/order");
    var vm = Application.ViewModel.Order.CreateViewModel.getInitializedViewModel(command);
    ko.applyBindings(vm);

}