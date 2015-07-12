/// <reference path="common.ts" />
module Application.GridView {
    "use strict";

    export class Column {
        constructor(public caption: string, public field: string, public sortKey: string,
            public formatString: string, public cellTemplate: string, public cellTemplateName: string) {
        }
    }

    export class Option<TModel> {
        filterPanelVisible = true;
        filterPanelCriteriaTemplateName: string = null;
        filterPanelCriteriaCallback: () => string = null;
        filterPanelCriteriaExpressionCallback: () => string[] = null;
        filterPanelClearButtonCallback: () => void = null;
        filterPanelSetButtonCallback: () => void = null;
        defaultCriteriaCallback: () => string = null;
        columns: Column[] = [];
        pagingEnabled = true;
        defaultPageSize = 5;
        avaliablePageSizes: number[] = [3, 5, 10, 20, 50, 100];
        pageSizeArgumentName = "pageSize";
        skipArgumentName = "skip";
        sortArgumentName = "sort";
        pagedQuery: Common.IPagedQuery<TModel, any> = null;
        query: Common.IQuery<TModel, any> = null;
        errorHandlerCallback: (data: any) => void = null;
        validate = (): void => {
            if (this.filterPanelVisible) {
                Common.Util.validateSettings(
                    this.filterPanelCriteriaTemplateName !== "", "filterPanelCriteriaTemplateName is not set");
                Common.Util.validateSettings(
                    this.filterPanelCriteriaCallback !== null, "filterPanelCriteriaCallback is not set");
                Common.Util.validateSettings(
                    this.filterPanelCriteriaExpressionCallback !== null, "filterPanelCriteriaExpressionCallback is not set");
                Common.Util.validateSettings(
                    this.filterPanelClearButtonCallback !== null, "filterPanelClearButtonCallback is not set");
                Common.Util.validateSettings(
                    this.filterPanelSetButtonCallback !== null, "filterPanelSetButtonCallback is not set");
            }
            Common.Util.validateSettings(this.columns.length > 0, "At least one column should be defined");
            if (this.pagingEnabled) {
                Common.Util.validateSettings(this.avaliablePageSizes.length > 0,
                    "At least one avaliablePageSizes should be defined when pagingEnabled is set to true");
                Common.Util.validateSettings(this.avaliablePageSizes.length > 0
                    && Enumerable.From<number>(this.avaliablePageSizes).Contains(this.defaultPageSize),
                    "AvaliablePageSizes does not contain defaultPageSize when pagingEnabled is set to true");
                Common.Util.validateSettings(this.pageSizeArgumentName !== "",
                    "PageSizeArgumentName should be defined when pagingEnabled is set to true");
                Common.Util.validateSettings(this.skipArgumentName !== "",
                    "skipArgumentName should be defined when pagingEnabled is set to true");
                Common.Util.validateSettings(this.pagedQuery !== null,
                    "pagedQuery should be defined when pagingEnabled is set to true");
            } else {
                Common.Util.validateSettings(this.query !== null,
                    "query should be defined when pagingEnabled is set to false");
            }
            Common.Util.validateSettings(this.sortArgumentName !== "",
                "sortArgumentName should be defined");
            Common.Util.validateSettings(this.errorHandlerCallback !== null,
                "errorHandlerCallback should be defined");
        }
    }

    export class Header {
        private getSortStyle = (): string => {
            switch (this.sortDirection()) {
                case "asc":
                    return "fa fa-sort-asc";
                case "desc":
                    return "fa fa-sort-desc";

                default:
                    return "";
            }
        }
        sortDirection = ko.observable("");
        sortStyle = ko.computed(this.getSortStyle);

        constructor(public caption: string, public field: string, public sortEnabled: boolean, public formatString: string,
            public useTemplate: boolean, public useInlineTemplate: boolean, public templateName: string) {
        }
    }

    export class BaseGridView<TModel> {

        private getFilterPanelButtonStyle = (): string => this.filterPanelCriteriaVisible()
            ?
            "fa fa-minus-square-o"
            :
            "fa fa-plus-square-o";
        private getFilterButtonTooltip = (): string => this.filterPanelCriteriaVisible() ? "Collapse" : "Expand";
        pageSize = ko.observable(0);
        headers = ko.observableArray<Header>([]);
        currentPage = ko.observable(0);
        pageCount = ko.observable(0);
        items = ko.observableArray<TModel>([]);
        itemCount = ko.observable(0);
        firstPageEnabled = ko.observable(false);
        prevPageEnabled = ko.observable(false);
        nextPageEnabled = ko.observable(false);
        lastPageEnabled = ko.observable(false);
        filterPanelCriteriaExpression = ko.observable("Filter criteria: ");
        sort = ko.observable("");
        filterPanelCriteriaVisible = ko.observable(true);
        filterPanelButtonStyle = ko.computed(this.getFilterPanelButtonStyle);
        filterButtonTooltip = ko.computed(this.getFilterButtonTooltip);

        private optionValidated = false;

        constructor(public option: Option<TModel>) {
        }

        fetchData = (): void => {
            if (!this.optionValidated) {
                this.option.validate();
                this.initFieldsFromOption();
                this.optionValidated = true;
            }
            var criteria =
                this.option.filterPanelVisible
                    ?
                    this.option.filterPanelCriteriaCallback()
                    :
                    (this.option.defaultCriteriaCallback != null ? this.option.defaultCriteriaCallback() : "");
            if (criteria !== "") {
                criteria = Common.Util.formatString("&{0}", criteria);
            }
            var params: string;
            if (this.option.pagingEnabled) {
                var skip = this.currentPage() * this.pageSize();
                params = Common.Util.formatString("{4}={0}&{5}={1}&{6}={2}{3}",
                    this.pageSize().toString(), skip.toString(), encodeURIComponent(this.sort()),
                    criteria, this.option.pageSizeArgumentName,
                    this.option.skipArgumentName, this.option.sortArgumentName);
                this.option.pagedQuery.fetch(params, this.handlePagedData, this.handleError);
            } else {
                params = Common.Util.formatString("{2}={0}{1}", encodeURIComponent(this.sort()), criteria, this.option.sortArgumentName);
                this.option.query.fetch(params, this.handleData, this.handleError);
            }
        }
        refresh = (): void => this.fetchData();
        goToFirstPage = (): void => {
            if (this.option.pagingEnabled) {
                this.currentPage(0);
                this.fetchData();
            }
        }
        goToPrevPage = (): void => {
            if (this.option.pagingEnabled) {
                this.currentPage(this.currentPage() - 1);
                this.fetchData();
            }
        }
        goToNextPage = (): void => {
            if (this.option.pagingEnabled) {
                this.currentPage(this.currentPage() + 1);
                this.fetchData();
            }
        }
        goToLastPage = (): void => {
            if (this.option.pagingEnabled) {
                this.currentPage(Math.ceil(this.itemCount() / this.pageSize() - 1));
                this.fetchData();
            }
        }
        filterPanelCriteriaTemplateName = (): string => this.option.filterPanelCriteriaTemplateName;
        avaliablePageSizes = (): number[]=> this.option.avaliablePageSizes;
        filterPanelVisible = (): boolean => this.option.filterPanelVisible;
        pagingEnabled = (): boolean => this.option.pagingEnabled;
        sortItems = (header: Header): void => {
            var order = this.getSortOrder(header.field);
            this.sort(Common.Util.formatString("{0} {1}", header.field, order));
            this.fetchData();
        }
        toggleFilterPanel = (): void => {
            if (this.option.filterPanelVisible) {
                this.filterPanelCriteriaVisible(!this.filterPanelCriteriaVisible());
            }
        }
        setFilter = (): void => {
            if (this.option.filterPanelVisible) {
                this.option.filterPanelSetButtonCallback();
                this.setFilterCriteriaExpression();
                if (this.option.pagingEnabled) {
                    this.goToFirstPage();
                } else {
                    this.fetchData();
                }
            }
        }
        clearFilter = (): void => {
            if (this.option.filterPanelVisible) {
                this.option.filterPanelClearButtonCallback();
                this.setFilterCriteriaExpression();
                if (this.option.pagingEnabled) {
                    this.goToFirstPage();
                } else {
                    this.fetchData();
                }
            }
        }

        private initFieldsFromOption = (): void => {
            var ste = ko.stringTemplateEngine.instance;
            this.pageSize(this.option.defaultPageSize);
            this.option.columns.forEach((c: Column) => {
                var templateName: string;
                if (c.cellTemplate !== "") {
                    templateName = Common.Guid.newGuid();
                    ste.addTemplate(templateName, c.cellTemplate);
                } else {
                    templateName = c.cellTemplateName;
                }
                this.headers.push(new Header(c.caption, c.field, c.sortKey !== "", c.formatString,
                    c.cellTemplate !== "" || c.cellTemplateName !== "", c.cellTemplate !== "", templateName));
            });
            if (this.option.pagingEnabled) {
                this.pageSize.subscribe(this.goToFirstPage, this);
            }
        }

        private handlePagedData = (data: Common.Paged<TModel>): void => {
            this.itemCount(data.count);
            this.pageCount(this.calculateTotalPages());
            this.items(data.items);
            this.setButtons();
        }

        private handleError = (data: any): void => this.option.errorHandlerCallback(data);

        private handleData = (data: TModel[]): void => {
            this.items(data);
            this.itemCount(data.length);
        };

        private setFilterCriteriaExpression = (): void => {
            var arr = this.option.filterPanelCriteriaExpressionCallback();
            this.filterPanelCriteriaExpression(Common.Util.formatString("Filter criteria: {0}", arr.join(" AND ")));
        }

        private getSortOrder = (field: string): string => {
            var order = "";
            Enumerable.From<Header>(this.headers()).ForEach((h: Header) => {
                if (h.field !== field) {
                    h.sortDirection("");
                } else {
                    if (h.sortDirection() === "" || h.sortDirection() === "desc") {
                        h.sortDirection("asc");
                        order = "asc";
                    } else {
                        h.sortDirection("desc");
                        order = "desc";
                    }
                }
            });
            return order;
        }

        private calculateTotalPages = (): number => {
            if (this.option.pagingEnabled) {
                var res = Math.ceil(this.itemCount() / this.pageSize() - 1);
                return res === -1 ? 0 : res;
            }
            return 0;
        }

        private isLastPageEnabled = (): boolean => this.option.pagingEnabled ? this.currentPage() < this.calculateTotalPages() : false;

        private isNextPageEnabled = (): boolean => this.option.pagingEnabled ? this.itemCount() > this.pageSize() *
            (this.currentPage() + 1) : false;

        private isPrevPageEnabled = (): boolean => this.option.pagingEnabled ? this.currentPage() > 0 : false;

        private isFirstPageEnabled = (): boolean => this.option.pagingEnabled ? this.currentPage() > 0 : false;

        private setButtons = (): void => {
            this.lastPageEnabled(this.isLastPageEnabled());
            this.nextPageEnabled(this.isNextPageEnabled());
            this.prevPageEnabled(this.isPrevPageEnabled());
            this.firstPageEnabled(this.isFirstPageEnabled());
        }
        getDisplayValue = (obj: TModel, field: string, formatString: string): string => {
            var d: any = obj[field];
            var undefinedOrNull = Common.Util.isUndefinedOrNull(d);
            if (undefinedOrNull) {
                return "";
            }
            if (formatString === "") {
                return d.toString();
            } else {
                return this.getFormattedValue(d, formatString);
            }
        }

        private getFormattedValue = (value: any, formatString: string): string => {
            // if (value instanceof Number)
            //    return numeral(value).format(formatString);           
            // if (value instanceof Date)
            //    return moment(value).format(formatString);           
            // return "not implemented yet";
            switch (typeof value) {
                case "number":
                    return numeral(value).format(formatString);
                case "string": // todo - jak okreslic czy to date - jquery zwraca obiekt ktory date zamienia na string'a
                    return moment(value).format(formatString);
                default:
                    return "not implemented yet";
            }
        }


    };
}

