var Application;
(function (Application) {
    var View;
    (function (View) {
        var Customer;
        (function (Customer) {
            var Index;
            (function (Index) {
                "use strict";
                var pagedQuery = new Application.Common.PagedQuery("/api/customer");
                var insertCommand = new Application.Common.Command("/api/customer");
                var vm = Application.ViewModel.Customer.ViewModel.getInitializedViewModel(pagedQuery, insertCommand);
                ko.applyBindings(vm);
            })(Index = Customer.Index || (Customer.Index = {}));
        })(Customer = View.Customer || (View.Customer = {}));
    })(View = Application.View || (Application.View = {}));
})(Application || (Application = {}));
//# sourceMappingURL=customer.index.js.map