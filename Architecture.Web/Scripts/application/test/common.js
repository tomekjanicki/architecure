/// <reference path="../../typings/qunit/qunit.d.ts" />
/// <reference path="../common.ts" />
var Application;
(function (Application) {
    var Test;
    (function (Test) {
        var Common;
        (function (Common) {
            "use strict";
            QUnit.module("Application.Test.Common.Util");
            QUnit.test("formatString_twoArguments_returnsConcatendedString", function () {
                var a1 = "a";
                var a2 = "b";
                var result = Application.Common.Util.formatString("{0} {1}", a1, a2);
                QUnit.equal(result, "a b");
            });
            QUnit.test("isUndefinedOrNullOrEmpty_emptyString_returnsTrue", function () {
                var result = Application.Common.Util.isUndefinedOrNullOrEmpty("");
                QUnit.equal(result, true);
            });
            QUnit.test("isUndefinedOrNullOrEmpty_null_returnsTrue", function () {
                var result = Application.Common.Util.isUndefinedOrNullOrEmpty(null);
                QUnit.equal(result, true);
            });
            QUnit.test("isUndefinedOrNullOrEmpty_undefined_returnsTrue", function () {
                var result = Application.Common.Util.isUndefinedOrNullOrEmpty(undefined);
                QUnit.equal(result, true);
            });
            QUnit.test("isUndefinedOrNullOrEmpty_nonEmptyString_returnsFalse", function () {
                var result = Application.Common.Util.isUndefinedOrNullOrEmpty("a");
                QUnit.equal(result, false);
            });
        })(Common = Test.Common || (Test.Common = {}));
    })(Test = Application.Test || (Application.Test = {}));
})(Application || (Application = {}));
//# sourceMappingURL=common.js.map