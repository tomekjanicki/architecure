/// <reference path="../viewmodel/product.ts" />
var Application;
(function (Application) {
    var View;
    (function (View) {
        var Product;
        (function (Product) {
            var Index;
            (function (Index) {
                "use strict";
                var pagedQuery = new Application.Common.PagedQuery("/api/product");
                var deleteProductCommand = new Application.Common.Command("/api/product/delete");
                var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, deleteProductCommand);
                ko.applyBindings(vm);
            })(Index = Product.Index || (Product.Index = {}));
        })(Product = View.Product || (View.Product = {}));
    })(View = Application.View || (Application.View = {}));
})(Application || (Application = {}));
//# sourceMappingURL=product.index.js.map