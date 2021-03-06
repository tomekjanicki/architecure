﻿module Application.View.Customer.Index {
    "use strict";

    var pagedQuery = new Common.PagedQuery<ViewModel.Customer.FindCustomer, any>("/api/customer/findcustomers");
    var insertCommand = new Common.Command<ViewModel.Customer.InsertCustomer, any, any>("/api/customer/postcustomer");
    var vm = ViewModel.Customer.ViewModel.getInitializedViewModel(pagedQuery, insertCommand);
    ko.applyBindings(vm);

} 