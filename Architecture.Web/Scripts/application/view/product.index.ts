module Application.View.Product.Index {
    "use strict";

    var pagedQuery = new Common.PagedQuery<ViewModel.Product.Index, any>("/api/product/findproductspaged");
    var query = new Common.Query<ViewModel.Product.Index, any>("/api/product/findproducts");
    var deleteCommand = new Common.Command<ViewModel.Product.Delete, any, any>("/api/product/deleteproduct");
    var vm = ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, query, deleteCommand);
    ko.applyBindings(vm);
}