/// <reference path="../../typings/qunit/qunit.d.ts" />
/// <reference path="../common.ts" />

module Application.Test.Common {
    "use strict";

    QUnit.module("Application.Test.Common.Util");

    QUnit.test("formatString_twoArguments_returnsConcatendedString",() => {
        var a1 = "a";
        var a2 = "b";

        var result = Application.Common.Util.formatString("{0} {1}", a1, a2);

        QUnit.equal(result, "a b");
    });

    QUnit.test("isUndefinedOrNullOrEmpty_emptyString_returnsTrue",() => {

        var result = Application.Common.Util.isUndefinedOrNullOrEmpty("");

        QUnit.equal(result, true);
    });

    QUnit.test("isUndefinedOrNullOrEmpty_null_returnsTrue",() => {

        var result = Application.Common.Util.isUndefinedOrNullOrEmpty(null);

        QUnit.equal(result, true);
    });

    QUnit.test("isUndefinedOrNullOrEmpty_undefined_returnsTrue",() => {

        var result = Application.Common.Util.isUndefinedOrNullOrEmpty(undefined);

        QUnit.equal(result, true);
    });

    QUnit.test("isUndefinedOrNullOrEmpty_nonEmptyString_returnsFalse",() => {

        var result = Application.Common.Util.isUndefinedOrNullOrEmpty("a");

        QUnit.equal(result, false);
    });

}