var Application;
(function (Application) {
    var View;
    (function (View) {
        var Product;
        (function (Product) {
            var Index;
            (function (Index) {
                "use strict";
                var pagedQuery = new Application.Common.PagedQuery("/api/product/findproductspaged");
                var query = new Application.Common.Query("/api/product/findproducts");
                var deleteCommand = new Application.Common.Command("/api/product/deleteproduct");
                var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, query, deleteCommand);
                ko.applyBindings(vm);
            })(Index = Product.Index || (Product.Index = {}));
        })(Product = View.Product || (View.Product = {}));
    })(View = Application.View || (Application.View = {}));
})(Application || (Application = {}));
//# sourceMappingURL=product.index.js.map