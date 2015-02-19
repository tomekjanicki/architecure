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
        var Product1;
        (function (Product1) {
            "use strict";
            var Index = (function () {
                function Index() {
                }
                return Index;
            })();
            Product1.Index = Index;
            var IndexViewModel = (function (_super) {
                __extends(IndexViewModel, _super);
                function IndexViewModel(option) {
                    var _this = this;
                    _super.call(this, option);
                    this.code = ko.observable("");
                    this.name = ko.observable("");
                    this.codeLocal = "";
                    this.nameLocal = "";
                    this.criteriaExpression = function () {
                        var arr = [];
                        if (_this.codeLocal !== "") {
                            arr.push(Application.Common.Util.getLikeExpression("Code", _this.codeLocal));
                        }
                        if (_this.nameLocal !== "") {
                            arr.push(Application.Common.Util.getLikeExpression("Name", _this.nameLocal));
                        }
                        return arr;
                    };
                    this.criteria = function () { return Application.Common.Util.formatString("code={0}&name={1}", _this.codeLocal, _this.nameLocal); };
                    this.swapValues = function () {
                        _this.codeLocal = _this.code();
                        _this.nameLocal = _this.name();
                    };
                    this.setButton = function () { return _this.swapValues(); };
                    this.clearButton = function () {
                        _this.code("");
                        _this.name("");
                        _this.swapValues();
                    };
                }
                IndexViewModel.getInitializedViewModel = function (pagedQuery, query) {
                    var o = new Application.GridView.Option();
                    o.filterPanelVisible = true;
                    o.pagingEnabled = false;
                    o.pagedQuery = pagedQuery;
                    o.defaultPageSize = 10;
                    o.query = query;
                    o.filterPanelCriteriaTemplateName = "criteriaTemplate";
                    o.columns.push(new Application.GridView.Column("Id", "Id", "Id", "", ""));
                    o.columns.push(new Application.GridView.Column("Code", "Code", "Code", "", ""));
                    o.columns.push(new Application.GridView.Column("Name", "Name", "Name", "", ""));
                    o.columns.push(new Application.GridView.Column("Price", "Price", "Price", "$0,0.00", ""));
                    o.columns.push(new Application.GridView.Column("Date", "Date", "", "YYYY-MM-DD", ""));
                    o.columns.push(new Application.GridView.Column("", "", "", "", "<a data-bind=\"attr: { href: '\\\\product\\\\edit\\\\' + item.Id }\" class=\"btn btn-default\">Edit</a>"));
                    o.errorHandlerCallback = function (data) { return window.alert(data); };
                    var vm = new IndexViewModel(o);
                    o.filterPanelClearButtonCallback = vm.clearButton;
                    o.filterPanelSetButtonCallback = vm.setButton;
                    o.filterPanelCriteriaCallback = vm.criteria;
                    o.filterPanelCriteriaExpressionCallback = vm.criteriaExpression;
                    o.defaultCriteriaCallback = vm.criteria;
                    vm.fetchData();
                    return vm;
                };
                return IndexViewModel;
            })(Application.GridView.BaseGridView);
            Product1.IndexViewModel = IndexViewModel;
            ;
        })(Product1 = ViewModel.Product1 || (ViewModel.Product1 = {}));
    })(ViewModel = Application.ViewModel || (Application.ViewModel = {}));
})(Application || (Application = {}));
//# sourceMappingURL=product1.js.map