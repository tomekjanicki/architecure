/// <reference path="../../../typings/jquery/jquery.d.ts" />
/// <reference path="../../../typings/knockout/knockout.d.ts" />
/// <reference path="../../../typings/qunit/qunit.d.ts" />
/// <reference path="../../viewmodel/product.ts" />
/// <reference path="../../common.ts" />
var Application;
(function (Application) {
    var Test;
    (function (Test) {
        var View;
        (function (View) {
            var Product;
            (function (Product) {
                var Index;
                (function (Index) {
                    "use strict";
                    function getJson(count) {
                        var jsonString = "{ \"Count\": " + count + ", \"Items\": [ " + "{ \"Id\": 1, \"Code\": \"C1\", \"Name\": \"N1\", \"Price\": 12.22 }, " + "{ \"Id\": 1, \"Code\": \"C1\", \"Name\": \"N1\", \"Price\": 12.22 }, " + "{ \"Id\": 1, \"Code\": \"C1\", \"Name\": \"N1\", \"Price\": 12.22 } " + "] }";
                        return $.parseJSON(jsonString);
                    }
                    function getCountEqual3AndPageSizeEqual3() {
                        return getJson(3);
                    }
                    function getCountEqual4AndPageSizeEqual3() {
                        return getJson(4);
                    }
                    function getCountEqual7AndPageSizeEqual3() {
                        return getJson(7);
                    }
                    function equals(vm, first, prev, next, last) {
                        equal(vm.firstPageEnabled(), first);
                        equal(vm.prevPageEnabled(), prev);
                        equal(vm.nextPageEnabled(), next);
                        equal(vm.lastPageEnabled(), last);
                    }
                    QUnit.module("Application.Test.View.Product.Index");
                    QUnit.test("firstPage_countEqualsTo3_returnsAllDisabled", function () {
                        var pagedQuery = new Application.Test.Helper.FakePagedQuery(getCountEqual3AndPageSizeEqual3(), null);
                        var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
                        vm.pageSize(3);
                        equals(vm, false, false, false, false);
                    });
                    QUnit.test("firstPage_countEqualsTo4_returnsFirstPrevDisabledAndNextLastEnabled", function () {
                        var pagedQuery = new Application.Test.Helper.FakePagedQuery(getCountEqual4AndPageSizeEqual3(), null);
                        var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
                        vm.pageSize(3);
                        equals(vm, false, false, true, true);
                    });
                    QUnit.test("goToNextPage_countEqualsTo7_returnsFirstPrevNextLastEnabled", function () {
                        var pagedQuery = new Application.Test.Helper.FakePagedQuery(getCountEqual7AndPageSizeEqual3(), null);
                        var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
                        vm.pageSize(3);
                        if (vm.nextPageEnabled()) {
                            vm.goToNextPage();
                        }
                        equals(vm, true, true, true, true);
                    });
                    QUnit.test("goToLastPage_countEqualsTo7_returnsFirstPrevEnabledAndNextLastDisabled", function () {
                        var pagedQuery = new Application.Test.Helper.FakePagedQuery(getCountEqual7AndPageSizeEqual3(), null);
                        var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
                        vm.pageSize(3);
                        if (vm.lastPageEnabled()) {
                            vm.goToLastPage();
                        }
                        equals(vm, true, true, false, false);
                    });
                })(Index = Product.Index || (Product.Index = {}));
            })(Product = View.Product || (View.Product = {}));
        })(View = Test.View || (Test.View = {}));
    })(Test = Application.Test || (Application.Test = {}));
})(Application || (Application = {}));
//# sourceMappingURL=product.index.js.map