var Application;
(function (Application) {
    var View;
    (function (View) {
        var Product1;
        (function (Product1) {
            var Index;
            (function (Index) {
                "use strict";
                var pagedQuery = new Application.Common.PagedQuery("/api/product");
                var query = new Application.Common.Query("/api/product");
                var deleteCommand = new Application.Common.Command("/api/product/");
                var vm = Application.ViewModel.Product1.IndexViewModel.getInitializedViewModel(pagedQuery, query, deleteCommand);
                ko.applyBindings(vm);
            })(Index = Product1.Index || (Product1.Index = {}));
        })(Product1 = View.Product1 || (View.Product1 = {}));
    })(View = Application.View || (Application.View = {}));
})(Application || (Application = {}));
//# sourceMappingURL=product1.index.js.map