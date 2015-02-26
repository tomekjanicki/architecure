/// <reference path="common.ts" />
var Application;
(function (Application) {
    var GridView;
    (function (GridView) {
        "use strict";
        var Column = (function () {
            function Column(caption, field, sortKey, formatString, cellTemplate, cellTemplateName) {
                this.caption = caption;
                this.field = field;
                this.sortKey = sortKey;
                this.formatString = formatString;
                this.cellTemplate = cellTemplate;
                this.cellTemplateName = cellTemplateName;
            }
            return Column;
        })();
        GridView.Column = Column;
        var Option = (function () {
            function Option() {
                var _this = this;
                this.filterPanelVisible = true;
                this.filterPanelCriteriaTemplateName = null;
                this.filterPanelCriteriaCallback = null;
                this.filterPanelCriteriaExpressionCallback = null;
                this.filterPanelClearButtonCallback = null;
                this.filterPanelSetButtonCallback = null;
                this.defaultCriteriaCallback = null;
                this.columns = [];
                this.pagingEnabled = true;
                this.defaultPageSize = 5;
                this.avaliablePageSizes = [3, 5, 10, 20, 50, 100];
                this.pageSizeArgumentName = "pageSize";
                this.skipArgumentName = "skip";
                this.sortArgumentName = "sort";
                this.pagedQuery = null;
                this.query = null;
                this.errorHandlerCallback = null;
                this.validate = function () {
                    if (_this.filterPanelVisible) {
                        Application.Common.Util.validateSettings(_this.filterPanelCriteriaTemplateName !== "", "filterPanelCriteriaTemplateName is not set");
                        Application.Common.Util.validateSettings(_this.filterPanelCriteriaCallback !== null, "filterPanelCriteriaCallback is not set");
                        Application.Common.Util.validateSettings(_this.filterPanelCriteriaExpressionCallback !== null, "filterPanelCriteriaExpressionCallback is not set");
                        Application.Common.Util.validateSettings(_this.filterPanelClearButtonCallback !== null, "filterPanelClearButtonCallback is not set");
                        Application.Common.Util.validateSettings(_this.filterPanelSetButtonCallback !== null, "filterPanelSetButtonCallback is not set");
                    }
                    Application.Common.Util.validateSettings(_this.columns.length > 0, "At least one column should be defined");
                    if (_this.pagingEnabled) {
                        Application.Common.Util.validateSettings(_this.avaliablePageSizes.length > 0, "At least one avaliablePageSizes should be defined when pagingEnabled is set to true");
                        Application.Common.Util.validateSettings(_this.avaliablePageSizes.length > 0 && Enumerable.From(_this.avaliablePageSizes).Contains(_this.defaultPageSize), "AvaliablePageSizes does not contain defaultPageSize when pagingEnabled is set to true");
                        Application.Common.Util.validateSettings(_this.pageSizeArgumentName !== "", "PageSizeArgumentName should be defined when pagingEnabled is set to true");
                        Application.Common.Util.validateSettings(_this.skipArgumentName !== "", "skipArgumentName should be defined when pagingEnabled is set to true");
                        Application.Common.Util.validateSettings(_this.pagedQuery !== null, "pagedQuery should be defined when pagingEnabled is set to true");
                    }
                    else {
                        Application.Common.Util.validateSettings(_this.query !== null, "query should be defined when pagingEnabled is set to false");
                    }
                    Application.Common.Util.validateSettings(_this.sortArgumentName !== "", "sortArgumentName should be defined");
                    Application.Common.Util.validateSettings(_this.errorHandlerCallback !== null, "errorHandlerCallback should be defined");
                };
            }
            return Option;
        })();
        GridView.Option = Option;
        var Header = (function () {
            function Header(caption, field, sortEnabled, formatString, useTemplate, useInlineTemplate, templateName) {
                var _this = this;
                this.caption = caption;
                this.field = field;
                this.sortEnabled = sortEnabled;
                this.formatString = formatString;
                this.useTemplate = useTemplate;
                this.useInlineTemplate = useInlineTemplate;
                this.templateName = templateName;
                this.getSortStyle = function () {
                    switch (_this.sortDirection()) {
                        case "asc":
                            return "fa fa-sort-asc";
                        case "desc":
                            return "fa fa-sort-desc";
                        default:
                            return "";
                    }
                };
                this.sortDirection = ko.observable("");
                this.sortStyle = ko.computed(this.getSortStyle);
            }
            return Header;
        })();
        GridView.Header = Header;
        var BaseGridView = (function () {
            function BaseGridView(option) {
                var _this = this;
                this.option = option;
                this.getFilterPanelButtonStyle = function () { return _this.filterPanelCriteriaVisible() ? "fa fa-minus-square-o" : "fa fa-plus-square-o"; };
                this.getFilterButtonTooltip = function () { return _this.filterPanelCriteriaVisible() ? "Collapse" : "Expand"; };
                this.pageSize = ko.observable(0);
                this.headers = ko.observableArray([]);
                this.currentPage = ko.observable(0);
                this.pageCount = ko.observable(0);
                this.items = ko.observableArray([]);
                this.itemCount = ko.observable(0);
                this.firstPageEnabled = ko.observable(false);
                this.prevPageEnabled = ko.observable(false);
                this.nextPageEnabled = ko.observable(false);
                this.lastPageEnabled = ko.observable(false);
                this.filterPanelCriteriaExpression = ko.observable("Filter criteria: ");
                this.sort = ko.observable("");
                this.filterPanelCriteriaVisible = ko.observable(true);
                this.filterPanelButtonStyle = ko.computed(this.getFilterPanelButtonStyle);
                this.filterButtonTooltip = ko.computed(this.getFilterButtonTooltip);
                this.optionValidated = false;
                this.fetchData = function () {
                    if (!_this.optionValidated) {
                        _this.option.validate();
                        _this.initFieldsFromOption();
                        _this.optionValidated = true;
                    }
                    var criteria = _this.option.filterPanelVisible ? _this.option.filterPanelCriteriaCallback() : (_this.option.defaultCriteriaCallback != null ? _this.option.defaultCriteriaCallback() : "");
                    if (criteria !== "") {
                        criteria = Application.Common.Util.formatString("&{0}", criteria);
                    }
                    var params;
                    if (_this.option.pagingEnabled) {
                        var skip = _this.currentPage() * _this.pageSize();
                        params = Application.Common.Util.formatString("{4}={0}&{5}={1}&{6}={2}{3}", _this.pageSize().toString(), skip.toString(), _this.sort(), criteria, _this.option.pageSizeArgumentName, _this.option.skipArgumentName, _this.option.sortArgumentName);
                        _this.option.pagedQuery.fetch(params, _this.handlePagedData, _this.handleError);
                    }
                    else {
                        params = Application.Common.Util.formatString("{2}={0}{1}", _this.sort(), criteria, _this.option.sortArgumentName);
                        _this.option.query.fetch(params, _this.handleData, _this.handleError);
                    }
                };
                this.refresh = function () { return _this.fetchData(); };
                this.goToFirstPage = function () {
                    if (_this.option.pagingEnabled) {
                        _this.currentPage(0);
                        _this.fetchData();
                    }
                };
                this.goToPrevPage = function () {
                    if (_this.option.pagingEnabled) {
                        _this.currentPage(_this.currentPage() - 1);
                        _this.fetchData();
                    }
                };
                this.goToNextPage = function () {
                    if (_this.option.pagingEnabled) {
                        _this.currentPage(_this.currentPage() + 1);
                        _this.fetchData();
                    }
                };
                this.goToLastPage = function () {
                    if (_this.option.pagingEnabled) {
                        _this.currentPage(Math.ceil(_this.itemCount() / _this.pageSize() - 1));
                        _this.fetchData();
                    }
                };
                this.filterPanelCriteriaTemplateName = function () { return _this.option.filterPanelCriteriaTemplateName; };
                this.avaliablePageSizes = function () { return _this.option.avaliablePageSizes; };
                this.filterPanelVisible = function () { return _this.option.filterPanelVisible; };
                this.pagingEnabled = function () { return _this.option.pagingEnabled; };
                this.sortItems = function (header) {
                    var order = _this.getSortOrder(header.field);
                    _this.sort(Application.Common.Util.formatString("{0} {1}", header.field, order));
                    _this.fetchData();
                };
                this.toggleFilterPanel = function () {
                    if (_this.option.filterPanelVisible) {
                        _this.filterPanelCriteriaVisible(!_this.filterPanelCriteriaVisible());
                    }
                };
                this.setFilter = function () {
                    if (_this.option.filterPanelVisible) {
                        _this.option.filterPanelSetButtonCallback();
                        _this.setFilterCriteriaExpression();
                        if (_this.option.pagingEnabled) {
                            _this.goToFirstPage();
                        }
                        else {
                            _this.fetchData();
                        }
                    }
                };
                this.clearFilter = function () {
                    if (_this.option.filterPanelVisible) {
                        _this.option.filterPanelClearButtonCallback();
                        _this.setFilterCriteriaExpression();
                        if (_this.option.pagingEnabled) {
                            _this.goToFirstPage();
                        }
                        else {
                            _this.fetchData();
                        }
                    }
                };
                this.initFieldsFromOption = function () {
                    var ste = ko.stringTemplateEngine.instance;
                    _this.pageSize(_this.option.defaultPageSize);
                    _this.option.columns.forEach(function (c) {
                        var templateName;
                        if (c.cellTemplate !== "") {
                            templateName = Application.Common.Guid.newGuid();
                            ste.addTemplate(templateName, c.cellTemplate);
                        }
                        else {
                            templateName = c.cellTemplateName;
                        }
                        _this.headers.push(new Header(c.caption, c.field, c.sortKey !== "", c.formatString, c.cellTemplate !== "" || c.cellTemplateName !== "", c.cellTemplate !== "", templateName));
                    });
                    if (_this.option.pagingEnabled) {
                        _this.pageSize.subscribe(_this.goToFirstPage, _this);
                    }
                };
                this.handlePagedData = function (data) {
                    _this.itemCount(data.Count);
                    _this.pageCount(_this.calculateTotalPages());
                    _this.items(data.Items);
                    _this.setButtons();
                };
                this.handleError = function (data) { return _this.option.errorHandlerCallback(data); };
                this.handleData = function (data) {
                    _this.items(data);
                    _this.itemCount(data.length);
                };
                this.setFilterCriteriaExpression = function () {
                    var arr = _this.option.filterPanelCriteriaExpressionCallback();
                    _this.filterPanelCriteriaExpression(Application.Common.Util.formatString("Filter criteria: {0}", arr.join(" AND ")));
                };
                this.getSortOrder = function (field) {
                    var order = "";
                    Enumerable.From(_this.headers()).ForEach(function (h) {
                        if (h.field !== field) {
                            h.sortDirection("");
                        }
                        else {
                            if (h.sortDirection() === "" || h.sortDirection() === "desc") {
                                h.sortDirection("asc");
                                order = "asc";
                            }
                            else {
                                h.sortDirection("desc");
                                order = "desc";
                            }
                        }
                    });
                    return order;
                };
                this.calculateTotalPages = function () {
                    if (_this.option.pagingEnabled) {
                        var res = Math.ceil(_this.itemCount() / _this.pageSize() - 1);
                        return res === -1 ? 0 : res;
                    }
                    return 0;
                };
                this.isLastPageEnabled = function () { return _this.option.pagingEnabled ? _this.currentPage() < _this.calculateTotalPages() : false; };
                this.isNextPageEnabled = function () { return _this.option.pagingEnabled ? _this.itemCount() > _this.pageSize() * (_this.currentPage() + 1) : false; };
                this.isPrevPageEnabled = function () { return _this.option.pagingEnabled ? _this.currentPage() > 0 : false; };
                this.isFirstPageEnabled = function () { return _this.option.pagingEnabled ? _this.currentPage() > 0 : false; };
                this.setButtons = function () {
                    _this.lastPageEnabled(_this.isLastPageEnabled());
                    _this.nextPageEnabled(_this.isNextPageEnabled());
                    _this.prevPageEnabled(_this.isPrevPageEnabled());
                    _this.firstPageEnabled(_this.isFirstPageEnabled());
                };
                this.getDisplayValue = function (obj, field, formatString) {
                    var d = obj[field];
                    var undefinedOrNull = Application.Common.Util.isUndefinedOrNull(d);
                    if (undefinedOrNull) {
                        return "";
                    }
                    if (formatString === "") {
                        return d.toString();
                    }
                    else {
                        return _this.getFormattedValue(d, formatString);
                    }
                };
                this.getFormattedValue = function (value, formatString) {
                    switch (typeof value) {
                        case "number":
                            return numeral(value).format(formatString);
                        case "string":
                            return moment(value).format(formatString);
                        default:
                            return "not implemented yet";
                    }
                };
            }
            return BaseGridView;
        })();
        GridView.BaseGridView = BaseGridView;
        ;
    })(GridView = Application.GridView || (Application.GridView = {}));
})(Application || (Application = {}));
//# sourceMappingURL=gridview.js.map