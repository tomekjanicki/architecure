/// <reference path="../viewmodel/product.ts" />


module Application.View.Product.Index {
    "use strict";

    var pagedQuery = new Application.Common.PagedQuery<Application.ViewModel.Product.IndexProduct, any>("/api/product");
    var deleteProductCommand = new Application.Common.Command<number, any, any>("/api/product/delete");
    var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, deleteProductCommand);
    ko.applyBindings(vm);
}