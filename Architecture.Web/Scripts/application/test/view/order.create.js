/// <reference path="../../../typings/jquery/jquery.d.ts" />
/// <reference path="../../../typings/knockout/knockout.d.ts" />
/// <reference path="../../../typings/qunit/qunit.d.ts" />
var Application;
(function (Application) {
    var Test;
    (function (Test) {
        var View;
        (function (View) {
            var Order;
            (function (Order) {
                var Create;
                (function (Create) {
                    "use strict";
                    QUnit.module("Application.Test.View.Order.Create");
                })(Create = Order.Create || (Order.Create = {}));
            })(Order = View.Order || (View.Order = {}));
        })(View = Test.View || (Test.View = {}));
    })(Test = Application.Test || (Application.Test = {}));
})(Application || (Application = {}));
//# sourceMappingURL=order.create.js.map