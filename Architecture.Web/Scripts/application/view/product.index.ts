module Application.View.Product1.Index {
    "use strict";

    var pagedQuery = new Application.Common.PagedQuery<Application.ViewModel.Product.Index, any>("/api/product");
    var query = new Application.Common.Query<Application.ViewModel.Product.Index, any>("/api/product");
    var deleteCommand = new Application.Common.Command<Application.ViewModel.Product.Delete, any, any>("/api/product/");
    var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, query, deleteCommand);
    ko.applyBindings(vm);
}