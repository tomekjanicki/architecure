/// <reference path="../viewmodel/order.ts" />
var Application;
(function (Application) {
    var View;
    (function (View) {
        var Order;
        (function (Order) {
            var Create;
            (function (Create) {
                "use strict";
                var command = new Application.Common.Command("/api/order/postorder");
                var vm = Application.ViewModel.Order.CreateViewModel.getInitializedViewModel(command);
                ko.applyBindings(vm);
            })(Create = Order.Create || (Order.Create = {}));
        })(Order = View.Order || (View.Order = {}));
    })(View = Application.View || (Application.View = {}));
})(Application || (Application = {}));
//# sourceMappingURL=order.create.js.map