module Application.GridView {
    "use strict";

    export class Column {
        public caption: string;
        public field: string;
        public sortKey: string; // when empty sort is disabled
        public formatString: string;
        public cellTemplate: string;
        public cellTemplateName: string;

        constructor(caption: string, field: string, sortKey: string, formatString: string, cellTemplate: string, cellTemplateName: string) {
            this.caption = caption;
            this.field = field;
            this.sortKey = sortKey;
            this.formatString = formatString;
            this.cellTemplate = cellTemplate;
            this.cellTemplateName = cellTemplateName;
        }
    }

    export class Option<TModel> {
        public filterPanelVisible: boolean = true;
        public filterPanelCriteriaTemplateName: string = null;
        public filterPanelCriteriaCallback: () => string = null;
        public filterPanelCriteriaExpressionCallback: () => string[] = null;
        public filterPanelClearButtonCallback: () => void = null;
        public filterPanelSetButtonCallback: () => void = null;
        public defaultCriteriaCallback: () => string = null;
        public columns: Column[] = [];
        public pagingEnabled: boolean = true;
        public defaultPageSize: number = 5;
        public avaliablePageSizes: number[] = [3, 5, 10, 20, 50, 100];
        public pageSizeArgumentName: string = "pageSize";
        public skipArgumentName: string = "skip";
        public sortArgumentName: string = "sort";
        public pagedQuery: Application.Common.IPagedQuery<TModel, any> = null;
        public query: Application.Common.IQuery<TModel, any> = null;
        public errorHandlerCallback: (data: any) => void = null;

        public validate = (): void => {
            if (this.filterPanelVisible) {
                Application.Common.Util.validateSettings(
                    this.filterPanelCriteriaTemplateName !== "", "filterPanelCriteriaTemplateName is not set");
                Application.Common.Util.validateSettings(
                    this.filterPanelCriteriaCallback !== null, "filterPanelCriteriaCallback is not set");
                Application.Common.Util.validateSettings(
                    this.filterPanelCriteriaExpressionCallback !== null, "filterPanelCriteriaExpressionCallback is not set");
                Application.Common.Util.validateSettings(
                    this.filterPanelClearButtonCallback !== null, "filterPanelClearButtonCallback is not set");
                Application.Common.Util.validateSettings(
                    this.filterPanelSetButtonCallback !== null, "filterPanelSetButtonCallback is not set");
            }
            Application.Common.Util.validateSettings(this.columns.length > 0, "At least one column should be defined");
            if (this.pagingEnabled) {
                Application.Common.Util.validateSettings(this.avaliablePageSizes.length > 0,
                    "At least one avaliablePageSizes should be defined when pagingEnabled is set to true");
                Application.Common.Util.validateSettings(this.avaliablePageSizes.length > 0
                    && Enumerable.From<number>(this.avaliablePageSizes).Contains(this.defaultPageSize),
                    "AvaliablePageSizes does not contain defaultPageSize when pagingEnabled is set to true");
                Application.Common.Util.validateSettings(this.pageSizeArgumentName !== "",
                    "PageSizeArgumentName should be defined when pagingEnabled is set to true");
                Application.Common.Util.validateSettings(this.skipArgumentName !== "",
                    "skipArgumentName should be defined when pagingEnabled is set to true");
                Application.Common.Util.validateSettings(this.pagedQuery !== null,
                    "pagedQuery should be defined when pagingEnabled is set to true");
            } else {
                Application.Common.Util.validateSettings(this.query !== null,
                    "query should be defined when pagingEnabled is set to false");
            }
            Application.Common.Util.validateSettings(this.sortArgumentName !== "",
                "sortArgumentName should be defined");
            Application.Common.Util.validateSettings(this.errorHandlerCallback !== null,
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

        public caption: string;
        public field: string;
        public sortDirection: KnockoutObservable<string> = ko.observable("");
        public sortEnabled: boolean;
        public sortStyle: KnockoutComputed<string> = ko.computed(this.getSortStyle);
        public formatString: string;
        public useTemplate: boolean;
        public useInlineTemplate: boolean;
        public templateName: string;

        constructor(caption: string, field: string, sortEnabled: boolean, formatString: string,
            useTemplate: boolean, useInlineTemplate: boolean, templateName: string) {
            this.caption = caption;
            this.field = field;
            this.sortEnabled = sortEnabled;
            this.formatString = formatString;
            this.useTemplate = useTemplate;
            this.templateName = templateName;
            this.useInlineTemplate = useInlineTemplate;
        }
    }

    export class BaseGridView<TModel> {

        private getFilterPanelButtonStyle = (): string => this.filterPanelCriteriaVisible()
            ?
            "fa fa-minus-square-o"
            :
            "fa fa-plus-square-o";
        private getFilterButtonTooltip = (): string => this.filterPanelCriteriaVisible() ? "Collapse" : "Expand";

        public pageSize: KnockoutObservable<number> = ko.observable(0);
        public headers: KnockoutObservableArray<Header> = ko.observableArray([]);
        public currentPage: KnockoutObservable<number> = ko.observable(0);
        public pageCount: KnockoutObservable<number> = ko.observable(0);
        public items: KnockoutObservableArray<TModel> = ko.observableArray([]);
        public itemCount: KnockoutObservable<number> = ko.observable(0);
        public firstPageEnabled: KnockoutObservable<boolean> = ko.observable(false);
        public prevPageEnabled: KnockoutObservable<boolean> = ko.observable(false);
        public nextPageEnabled: KnockoutObservable<boolean> = ko.observable(false);
        public lastPageEnabled: KnockoutObservable<boolean> = ko.observable(false);
        public filterPanelCriteriaExpression: KnockoutObservable<string> = ko.observable("Filter criteria: ");
        public sort: KnockoutObservable<string> = ko.observable("");
        public filterPanelCriteriaVisible: KnockoutObservable<boolean> = ko.observable(true);
        public filterPanelButtonStyle: KnockoutComputed<string> = ko.computed(this.getFilterPanelButtonStyle);
        public filterButtonTooltip: KnockoutComputed<string> = ko.computed(this.getFilterButtonTooltip);

        public option: Option<TModel>;
        private optionValidated: boolean = false;

        constructor(option: Option<TModel>) {
            this.option = option;
        }

        public fetchData = (): void => {
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
                criteria = Application.Common.Util.formatString("&{0}", criteria);
            }
            var params: string;
            if (this.option.pagingEnabled) {
                var skip = this.currentPage() * this.pageSize();
                params = Application.Common.Util.formatString("{4}={0}&{5}={1}&{6}={2}{3}",
                    this.pageSize().toString(), skip.toString(), this.sort(), criteria, this.option.pageSizeArgumentName,
                    this.option.skipArgumentName, this.option.sortArgumentName);
                this.option.pagedQuery.fetch(params, this.handlePagedData, this.handleError);
            } else {
                params = Application.Common.Util.formatString("{2}={0}{1}", this.sort(), criteria, this.option.sortArgumentName);
                this.option.query.fetch(params, this.handleData, this.handleError);
            }
        }

        public refresh = (): void => this.fetchData();

        public goToFirstPage = (): void => {
            if (this.option.pagingEnabled) {
                this.currentPage(0);
                this.fetchData();
            }
        }

        public goToPrevPage = (): void => {
            if (this.option.pagingEnabled) {
                this.currentPage(this.currentPage() - 1);
                this.fetchData();
            }
        }

        public goToNextPage = (): void => {
            if (this.option.pagingEnabled) {
                this.currentPage(this.currentPage() + 1);
                this.fetchData();
            }
        }

        public goToLastPage = (): void => {
            if (this.option.pagingEnabled) {
                this.currentPage(Math.ceil(this.itemCount() / this.pageSize() - 1));
                this.fetchData();
            }
        }

        public filterPanelCriteriaTemplateName = (): string => this.option.filterPanelCriteriaTemplateName;

        public avaliablePageSizes = (): number[]=> this.option.avaliablePageSizes;

        public filterPanelVisible = (): boolean => this.option.filterPanelVisible;

        public pagingEnabled = (): boolean => this.option.pagingEnabled;

        public sortItems = (header: Header): void => {
            var order = this.getSortOrder(header.field);
            this.sort(Application.Common.Util.formatString("{0} {1}", header.field, order));
            this.fetchData();
        }

        public toggleFilterPanel = (): void => {
            if (this.option.filterPanelVisible) {
                this.filterPanelCriteriaVisible(!this.filterPanelCriteriaVisible());
            }
        }

        public setFilter = (): void => {
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

        public clearFilter = (): void => {
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
                    templateName = Application.Common.Guid.newGuid();
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

        private handlePagedData = (data: Application.Common.Paged<TModel>): void => {
            this.itemCount(data.Count);
            this.pageCount(this.calculateTotalPages());
            this.items(data.Items);
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

        public getDisplayValue = (obj: TModel, field: string, formatString: string): string => {
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

