/// <reference path="../../../typings/jquery/jquery.d.ts" />
/// <reference path="../../../typings/knockout/knockout.d.ts" />
/// <reference path="../../../typings/qunit/qunit.d.ts" />
/// <reference path="../../viewmodel/product.ts" />
/// <reference path="../../common.ts" />

module Application.Test.View.Product.Index {
    "use strict";

    function getJson(count: number): any {
        var jsonString = "{ \"Count\": " + count + ", \"Items\": [ " +
            "{ \"Id\": 1, \"Code\": \"C1\", \"Name\": \"N1\", \"Price\": 12.22 }, " +
            "{ \"Id\": 1, \"Code\": \"C1\", \"Name\": \"N1\", \"Price\": 12.22 }, " +
            "{ \"Id\": 1, \"Code\": \"C1\", \"Name\": \"N1\", \"Price\": 12.22 } " +
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

    function equals(vm: Application.ViewModel.Product.IndexViewModel, first: boolean, prev: boolean, next: boolean, last: boolean) {
        equal(vm.firstPageEnabled(), first);
        equal(vm.prevPageEnabled(), prev);
        equal(vm.nextPageEnabled(), next);
        equal(vm.lastPageEnabled(), last);
    }

    QUnit.module("Application.Test.View.Product.Index");

    QUnit.test("firstPage_countEqualsTo3_returnsAllDisabled",() => {

        var pagedQuery = new Application.Test.Helper.FakePagedQuery<Application.ViewModel.Product.Index, any>(
            getCountEqual3AndPageSizeEqual3(), null);

        var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
        vm.pageSize(3);

        equals(vm, false, false, false, false);
    });

    QUnit.test("firstPage_countEqualsTo4_returnsFirstPrevDisabledAndNextLastEnabled",() => {

        var pagedQuery = new Application.Test.Helper.FakePagedQuery<Application.ViewModel.Product.Index, any>(
            getCountEqual4AndPageSizeEqual3(), null);

        var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
        vm.pageSize(3);

        equals(vm, false, false, true, true);
    });

    QUnit.test("goToNextPage_countEqualsTo7_returnsFirstPrevNextLastEnabled",() => {

        var pagedQuery = new Application.Test.Helper.FakePagedQuery<Application.ViewModel.Product.Index, any>(
            getCountEqual7AndPageSizeEqual3(), null);

        var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
        vm.pageSize(3);
        if (vm.nextPageEnabled()) {
            vm.goToNextPage();
        }

        equals(vm, true, true, true, true);
    });

    QUnit.test("goToLastPage_countEqualsTo7_returnsFirstPrevEnabledAndNextLastDisabled",() => {

        var pagedQuery = new Application.Test.Helper.FakePagedQuery<Application.ViewModel.Product.Index, any>(
            getCountEqual7AndPageSizeEqual3(), null);

        var vm = Application.ViewModel.Product.IndexViewModel.getInitializedViewModel(pagedQuery, null, null);
        vm.pageSize(3);
        if (vm.lastPageEnabled()) {
            vm.goToLastPage();
        }

        equals(vm, true, true, false, false);
    });

}