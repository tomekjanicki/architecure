module Application.View.Customer.Index {
    "use strict";

    var pagedQuery = new Common.PagedQuery<ViewModel.Customer.FindCustomerAsync, any>("/api/customer");
    var insertCommand = new Common.Command<ViewModel.Customer.InsertCustomerAsync, any, any>("/api/customer");
    var vm = ViewModel.Customer.ViewModel.getInitializedViewModel(pagedQuery, insertCommand);
    ko.applyBindings(vm);

} 