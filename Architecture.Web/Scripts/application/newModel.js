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
        var P;
        (function (P) {
            "use strict";
            var IndexProduct = (function () {
                function IndexProduct() {
                }
                return IndexProduct;
            })();
            P.IndexProduct = IndexProduct;
            var IndexViewModel = (function (_super) {
                __extends(IndexViewModel, _super);
                function IndexViewModel(option) {
                    _super.call(this, option);
                    this.code = ko.observable("");
                    this.name = ko.observable("");
                    this.codeLocal = "";
                    this.nameLocal = "";
                    this.getDisplayValueCallback = this.getDisplayVal;
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
                        default:
                            throw new Error("Not implemented");
                    }
                };
                IndexViewModel.prototype.criteriaExpression = function () {
                    var arr = [];
                    if (this.codeLocal !== "") {
                        arr.push(Application.Common.Util.getLikeExpression("Code", this.codeLocal));
                    }
                    if (this.nameLocal !== "") {
                        arr.push(Application.Common.Util.getLikeExpression("Name", this.nameLocal));
                    }
                    return arr;
                };
                IndexViewModel.prototype.criteria = function () {
                    return Application.Common.Util.formatString("code={0}&name={1}", this.codeLocal, this.nameLocal);
                };
                IndexViewModel.prototype.setButton = function () {
                    this.swapValues();
                };
                IndexViewModel.prototype.clearButton = function () {
                    this.code("");
                    this.name("");
                    this.swapValues();
                };
                IndexViewModel.prototype.swapValues = function () {
                    this.codeLocal = this.code();
                    this.nameLocal = this.name();
                };
                IndexViewModel.getInitializedViewModel = function (pagedQuery) {
                    var o = new Application.GridView.Option();
                    o.pagedQuery = pagedQuery;
                    o.filterPanelCriteriaTemplateName = "criteriaTemplate";
                    o.columns.push(new Application.GridView.Column("Id", "id", "id", ""));
                    o.columns.push(new Application.GridView.Column("Code", "code", "code", ""));
                    o.columns.push(new Application.GridView.Column("Name", "name", "name", ""));
                    o.columns.push(new Application.GridView.Column("Price", "price", "price", ""));
                    var vm = new IndexViewModel(o);
                    o.filterPanelClearButtonCallback = vm.clearButton;
                    o.filterPanelSetButtonCallback = vm.setButton;
                    o.filterPanelCriteriaCallback = vm.criteria;
                    o.filterPanelCriteriaExpressionCallback = vm.criteriaExpression;
                    vm.fetchData();
                    return vm;
                };
                return IndexViewModel;
            })(Application.GridView.BaseGridView);
            P.IndexViewModel = IndexViewModel;
            ;
        })(P = ViewModel.P || (ViewModel.P = {}));
    })(ViewModel = Application.ViewModel || (Application.ViewModel = {}));
})(Application || (Application = {}));
//# sourceMappingURL=newModel.js.map