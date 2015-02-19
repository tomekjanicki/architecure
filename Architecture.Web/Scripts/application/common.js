/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/knockout/knockout.d.ts" />
/// <reference path="../typings/linq/linq.d.ts" />
var Application;
(function (Application) {
    var Common;
    (function (Common) {
        "use strict";
        function ajax(url, params, done, fail, method) {
            var s = {};
            s.url = url;
            s.type = method;
            s.data = params;
            $.ajax(s).done(done).fail(fail);
        }
        var Util = (function () {
            function Util() {
            }
            Util.initBlockUiForAjaxRequests = function () {
                var msg = "<h1>Processing request... Please wait.</h1>";
                var opt = {
                    message: msg,
                    css: { border: "none" }
                };
                $(document).ajaxStart(function () { return $.blockUI(opt); });
                $(document).ajaxStop(function () { return $.unblockUI(); });
            };
            Util.formatString = function (s) {
                var params = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    params[_i - 1] = arguments[_i];
                }
                var i = params.length;
                while (i--) {
                    s = s.replace(new RegExp("\\{" + i + "\\}", "gm"), params[i]);
                }
                return s;
            };
            Util.getLikeExpression = function (field, value) {
                return Util.formatString("[{0}] like \"%{1}%\"", field, value);
            };
            Util.validateSettings = function (valid, notValidMessage) {
                if (!valid) {
                    throw new Error(notValidMessage);
                }
            };
            Util.isUndefinedOrNull = function (arg) { return arg === undefined || arg === null; };
            Util.isUndefinedOrNullOrEmpty = function (arg) { return Util.isUndefinedOrNull(arg) || arg === ""; };
            Util.unpackFromString = function (str) {
                var byteCharacters = atob(str);
                var byteNumbers = new Array(byteCharacters.length);
                for (var i = 0; i < byteCharacters.length; i++) {
                    byteNumbers[i] = byteCharacters.charCodeAt(i);
                }
                return byteNumbers;
            };
            return Util;
        })();
        Common.Util = Util;
        var Guid = (function () {
            function Guid() {
            }
            Guid.s4 = function () {
                return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
            };
            Guid.newGuid = function () {
                return Util.formatString("{0}{1}-{2}-{3}-{4}-{5}{6}{7}", Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4());
            };
            return Guid;
        })();
        Common.Guid = Guid;
        var Paged = (function () {
            function Paged() {
            }
            return Paged;
        })();
        Common.Paged = Paged;
        (function (Method) {
            Method[Method["Post"] = 0] = "Post";
            Method[Method["Put"] = 1] = "Put";
            Method[Method["Delete"] = 2] = "Delete";
        })(Common.Method || (Common.Method = {}));
        var Method = Common.Method;
        var Query = (function () {
            function Query(url) {
                var _this = this;
                this.fetch = function (params, done, fail) {
                    ajax(_this.url, params, done, fail, "GET");
                };
                this.url = url;
            }
            return Query;
        })();
        Common.Query = Query;
        var PagedQuery = (function () {
            function PagedQuery(url) {
                var _this = this;
                this.fetch = function (params, done, fail) {
                    ajax(_this.url, params, done, fail, "GET");
                };
                this.url = url;
            }
            return PagedQuery;
        })();
        Common.PagedQuery = PagedQuery;
        var Command = (function () {
            function Command(url) {
                var _this = this;
                this.execute = function (params, done, fail, method) {
                    var m = _this.getMethod(method);
                    ajax(_this.url, params, done, fail, m);
                };
                this.url = url;
            }
            Command.prototype.getMethod = function (method) {
                if (method.toString() === "0") {
                    return "POST";
                }
                if (method.toString() === "1") {
                    return "PUT";
                }
                return "DELETE";
            };
            return Command;
        })();
        Common.Command = Command;
        var Header = (function () {
            function Header(title, key, orderEnabled, htmlEncode) {
                this.order = ko.observable("");
                this.style = ko.observable("");
                this.title = title;
                this.key = key;
                this.order = ko.observable("");
                this.orderEnabled = orderEnabled;
                this.htmlEncode = htmlEncode;
            }
            return Header;
        })();
        Common.Header = Header;
        var GridViewModel = (function () {
            function GridViewModel(pagedQuery) {
                var _this = this;
                this.pageSize = ko.observable(5);
                this.headers = ko.observableArray([]);
                this.currentPage = ko.observable(0);
                this.pageCount = ko.observable(0);
                this.items = ko.observableArray([]);
                this.itemCount = ko.observable(0);
                this.firstPageEnabled = ko.observable(false);
                this.prevPageEnabled = ko.observable(false);
                this.nextPageEnabled = ko.observable(false);
                this.lastPageEnabled = ko.observable(false);
                this.filterCriteriaExpression = ko.observable("Filter criteria: ");
                this.sort = ko.observable("");
                this.filterPanelVisible = ko.observable(true);
                this.filterButtonText = ko.computed(function () {
                    return _this.filterPanelVisible() ? "-" : "+";
                });
                this.filterButtonTooltip = ko.computed(function () {
                    return _this.filterPanelVisible() ? "Collapse" : "Expand";
                });
                this.avaliablePageSizes = [3, 5, 10, 20, 50, 100];
                this.sortItems = function (header) {
                    var order = _this.getSortOrder(header.key);
                    _this.sort(Util.formatString("{0} {1}", header.key, order));
                    _this.fetchData();
                };
                this.pagedQuery = pagedQuery;
                this.pageSize.subscribe(this.goToFirstPage, this);
            }
            GridViewModel.prototype.getDisplayValue = function (obj, key) {
                if (this.getDisplayValueCallback == null) {
                    throw new Error("getDisplayValueCallback not implemented in parrent class");
                }
                return this.getDisplayValueCallback(obj, key);
            };
            GridViewModel.prototype.goToFirstPage = function () {
                this.currentPage(0);
                this.fetchData();
            };
            GridViewModel.prototype.goToPrevPage = function () {
                this.currentPage(this.currentPage() - 1);
                this.fetchData();
            };
            GridViewModel.prototype.goToNextPage = function () {
                this.currentPage(this.currentPage() + 1);
                this.fetchData();
            };
            GridViewModel.prototype.goToLastPage = function () {
                this.currentPage(Math.ceil(this.itemCount() / this.pageSize() - 1));
                this.fetchData();
            };
            GridViewModel.prototype.fetchData = function () {
                var _this = this;
                var skip = this.currentPage() * this.pageSize();
                var criteria = this.criteriaCallback != null ? this.criteriaCallback() : "";
                if (criteria !== "") {
                    criteria = Util.formatString("&{0}", criteria);
                }
                var params = Util.formatString("pageSize={0}&skip={1}&sort={2}{3}", this.pageSize().toString(), skip.toString(), this.sort(), criteria);
                this.pagedQuery.fetch(params, function (data) {
                    var d = _this.parseData(data);
                    _this.itemCount(d.Count);
                    _this.pageCount(_this.calculateTotalPages());
                    _this.items(d.Items);
                    _this.setButtons();
                }, function (message) {
                    window.alert(message.status);
                });
            };
            GridViewModel.prototype.toggleFilterPanel = function () {
                this.filterPanelVisible(!this.filterPanelVisible());
            };
            GridViewModel.prototype.setFilter = function () {
                if (this.setFilterCallback != null) {
                    this.setFilterCallback();
                }
                this.setFilterCriteriaExpression();
                this.goToFirstPage();
            };
            GridViewModel.prototype.clearFilter = function () {
                if (this.clearCallback != null) {
                    this.clearCallback();
                }
                this.setFilterCriteriaExpression();
                this.goToFirstPage();
            };
            GridViewModel.prototype.setFilterCriteriaExpression = function () {
                var arr = [];
                if (this.filterCriteriaExpressionCallback != null) {
                    arr = this.filterCriteriaExpressionCallback();
                }
                this.filterCriteriaExpression(Common.Util.formatString("Filter criteria: {0}", arr.join(" AND ")));
            };
            GridViewModel.prototype.parseData = function (data) {
                var p = new Paged();
                p.Count = data.Count;
                p.Items = data.Items;
                return p;
            };
            GridViewModel.prototype.getSortOrder = function (key) {
                var _this = this;
                var order = "";
                Enumerable.From(this.headers()).ForEach(function (h) {
                    if (h.key !== key) {
                        h.order("");
                        h.style(_this.getStyle(h.order()));
                    }
                    else {
                        if (h.order() === "" || h.order() === "desc") {
                            h.order("asc");
                            order = "asc";
                        }
                        else {
                            h.order("desc");
                            order = "desc";
                        }
                        h.style(_this.getStyle(h.order()));
                    }
                });
                return order;
            };
            GridViewModel.prototype.getStyle = function (order) {
                if (order === "asc") {
                    return "fa fa-angle-up";
                }
                if (order === "desc") {
                    return "fa fa-angle-down";
                }
                return "";
            };
            GridViewModel.prototype.setButtons = function () {
                this.lastPageEnabled(this.isLastPageEnabled());
                this.nextPageEnabled(this.isNextPageEnabled());
                this.prevPageEnabled(this.isPrevPageEnabled());
                this.firstPageEnabled(this.isFirstPageEnabled());
            };
            GridViewModel.prototype.isLastPageEnabled = function () {
                return this.currentPage() < this.calculateTotalPages();
            };
            GridViewModel.prototype.isNextPageEnabled = function () {
                return this.itemCount() > this.pageSize() * (this.currentPage() + 1);
            };
            GridViewModel.prototype.isPrevPageEnabled = function () {
                return this.currentPage() > 0;
            };
            GridViewModel.prototype.isFirstPageEnabled = function () {
                return this.currentPage() > 0;
            };
            GridViewModel.prototype.calculateTotalPages = function () {
                var res = Math.ceil(this.itemCount() / this.pageSize() - 1);
                return res === -1 ? 0 : res;
            };
            return GridViewModel;
        })();
        Common.GridViewModel = GridViewModel;
        ;
        Application.Common.Util.initBlockUiForAjaxRequests();
    })(Common = Application.Common || (Application.Common = {}));
})(Application || (Application = {}));
//# sourceMappingURL=common.js.map