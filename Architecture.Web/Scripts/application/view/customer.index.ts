module Application.View.Customer.Index {
    "use strict";

    var pagedQuery = new Application.Common.PagedQuery<Application.ViewModel.Customer.FindCustomerAsync, any>("/api/customer");
    var insertCommand = new Application.Common.Command<Application.ViewModel.Customer.InsertCustomerAsync, any, any>("/api/customer");
    var vm = Application.ViewModel.Customer.ViewModel.getInitializedViewModel(pagedQuery, insertCommand);
    ko.applyBindings(vm);

} 