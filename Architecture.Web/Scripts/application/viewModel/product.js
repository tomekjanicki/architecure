/// <reference path="../common.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var Application;
(function (Application) {
    var ViewModel;
    (function (ViewModel) {
        var Product;
        (function (Product) {
            "use strict";
            var IndexProduct = (function () {
                function IndexProduct() {
                }
                return IndexProduct;
            })();
            Product.IndexProduct = IndexProduct;
            var IndexViewModel = (function (_super) {
                __extends(IndexViewModel, _super);
                function IndexViewModel(pagedQuery, deleteProductCommand) {
                    _super.call(this, pagedQuery);
                    this.code = ko.observable("");
                    this.name = ko.observable("");
                    this.codeLocal = "";
                    this.nameLocal = "";
                    this.criteriaCallback = this.getCriteria;
                    this.deleteProductCommand = deleteProductCommand;
                    this.clearCallback = this.clear;
                    this.setFilterCallback = this.setFilterCriteria;
                    this.filterCriteriaExpressionCallback = this.filterCriteriaExp;
                    this.getDisplayValueCallback = this.getDisplayVal;
                    this.headers([
                        new Application.Common.Header("Id", "id", true, true),
                        new Application.Common.Header("Code", "code", true, true),
                        new Application.Common.Header("Name", "name", true, true),
                        new Application.Common.Header("Price", "price", true, true),
                        new Application.Common.Header("", "edit", false, false),
                        new Application.Common.Header("", "delete", false, false)
                    ]);
                    this.criteriaTemplate = "criteriaTemplate";
                }
                IndexViewModel.prototype.getDisplayVal = function (obj, key) {
                    switch (key) {
                        case "id":
                            return obj.Id.toFixed(0);
                        case "code":
                            return obj.Code;
                        case "name":
                            return obj.Name;
                        case "price":
                            return obj.Price.toFixed(2);
                        case "edit":
                            return "<a href=\"product\\edit\\0\" class=\"btn btn-default\">Edit</a>";
                        case "delete":
                            return "<a href=\"#\" class=\"btn btn-default\">Delete</a>";
                        default:
                            throw new Error("Not implemented");
                    }
                };
                IndexViewModel.prototype.filterCriteriaExp = function () {
                    var arr = [];
                    if (this.codeLocal !== "") {
                        arr.push(Application.Common.Util.getLikeExpression("Code", this.codeLocal));
                    }
                    if (this.nameLocal !== "") {
                        arr.push(Application.Common.Util.getLikeExpression("Name", this.nameLocal));
                    }
                    return arr;
                };
                IndexViewModel.prototype.getCriteria = function () {
                    return Application.Common.Util.formatString("code={0}&name={1}", this.codeLocal, this.nameLocal);
                };
                IndexViewModel.prototype.setFilterCriteria = function () {
                    this.codeLocal = this.code();
                    this.nameLocal = this.name();
                };
                IndexViewModel.prototype.clear = function () {
                    this.code("");
                    this.name("");
                    this.codeLocal = this.code();
                    this.nameLocal = this.name();
                };
                IndexViewModel.getInitializedViewModel = function (pagedQuery, deleteProductCommand) {
                    var vm = new IndexViewModel(pagedQuery, deleteProductCommand);
                    vm.fetchData();
                    return vm;
                };
                return IndexViewModel;
            })(Application.Common.GridViewModel);
            Product.IndexViewModel = IndexViewModel;
            ;
        })(Product = ViewModel.Product || (ViewModel.Product = {}));
    })(ViewModel = Application.ViewModel || (Application.ViewModel = {}));
})(Application || (Application = {}));
//# sourceMappingURL=product.js.map