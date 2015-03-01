module Application.View.Product.Index {
    "use strict";

    var pagedQuery = new Common.PagedQuery<ViewModel.Product.Index, any>("/api/product");
    var query = new Common.Query<ViewModel.Product.Index, any>("/api/product");
    var deleteCommand = new Common.Command<ViewModel.Product.Delete, any, any>("/api/product");
    var vm = ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, query, deleteCommand);
    ko.applyBindings(vm);
}