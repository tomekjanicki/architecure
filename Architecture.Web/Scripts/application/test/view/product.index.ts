/// <reference path="../../../typings/jquery/jquery.d.ts" />
/// <reference path="../../../typings/knockout/knockout.d.ts" />
/// <reference path="../../../typings/qunit/qunit.d.ts" />
/// <reference path="../../viewmodel/product.ts" />
/// <reference path="../helper.ts" />

module Application.Test.View.Product.Index {
    "use strict";

    function getJson(count: number): any {
        var jsonString = "{ \"count\": " + count + ", \"items\": [ " +
            "{ \"id\": 1, \"code\": \"C1\", \"name\": \"N1\", \"price\": 12.22 }, " +
            "{ \"id\": 1, \"code\": \"C1\", \"name\": \"N1\", \"price\": 12.22 }, " +
            "{ \"id\": 1, \"code\": \"C1\", \"name\": \"N1\", \"price\": 12.22 } " +
            "] }";
        return $.parseJSON(jsonString);
    }

    function getCountEqual3AndPageSizeEqual3(): any {
        return getJson(3);
    }

    function getCountEqual4AndPageSizeEqual3(): any {
        return getJson(4);
    }

    function getCountEqual7AndPageSizeEqual3(): any {
        return getJson(7);
    }

    function equals(vm: ViewModel.Product.IndexViewModel, first: boolean, prev: boolean, next: boolean, last: boolean) {
        equal(vm.firstPageEnabled(), first);
        equal(vm.prevPageEnabled(), prev);
        equal(vm.nextPageEnabled(), next);
        equal(vm.lastPageEnabled(), last);
    }

    QUnit.module("Application.Test.View.Product.Index");

    QUnit.test("firstPage_countEqualsTo3_returnsAllDisabled",() => {

        var pagedQuery = new Test.Helper.FakePagedQuery<ViewModel.Product.Index, any>(
            getCountEqual3AndPageSizeEqual3(), null);

        var vm = ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
        vm.pageSize(3);

        equals(vm, false, false, false, false);
    });

    QUnit.test("firstPage_countEqualsTo4_returnsFirstPrevDisabledAndNextLastEnabled",() => {

        var pagedQuery = new Test.Helper.FakePagedQuery<ViewModel.Product.Index, any>(
            getCountEqual4AndPageSizeEqual3(), null);

        var vm = ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
        vm.pageSize(3);

        equals(vm, false, false, true, true);
    });

    QUnit.test("goToNextPage_countEqualsTo7_returnsFirstPrevNextLastEnabled",() => {

        var pagedQuery = new Test.Helper.FakePagedQuery<ViewModel.Product.Index, any>(
            getCountEqual7AndPageSizeEqual3(), null);

        var vm = ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
        vm.pageSize(3);
        if (vm.nextPageEnabled()) {
            vm.goToNextPage();
        }

        equals(vm, true, true, true, true);
    });

    QUnit.test("goToLastPage_countEqualsTo7_returnsFirstPrevEnabledAndNextLastDisabled",() => {

        var pagedQuery = new Test.Helper.FakePagedQuery<ViewModel.Product.Index, any>(
            getCountEqual7AndPageSizeEqual3(), null);

        var vm = ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
        vm.pageSize(3);
        if (vm.lastPageEnabled()) {
            vm.goToLastPage();
        }

        equals(vm, true, true, false, false);
    });

}