/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/knockout/knockout.d.ts" />
/// <reference path="../typings/linq/linq.d.ts" />

module Application.Common {
    "use strict";

    function ajax(url: string, params: any, done: JQueryPromiseCallback<any>, fail: JQueryPromiseCallback<any>, method: string): void {
        var s: JQueryAjaxSettings = {};
        s.url = url;
        s.type = method;
        s.data = params;
        $.ajax(s).done(done).fail(fail);
    }


    export class Util {

        public static formatString = (s: string, ...params: string[]): string => {
            var i = params.length;
            while (i--) {
                s = s.replace(new RegExp("\\{" + i + "\\}", "gm"), params[i]);
            }
            return s;
        }

        public static getLikeExpression = (field: string, value: string): string => {
            return Util.formatString("[{0}] like \"%{1}%\"", field, value);
        }

        public static validateSettings = (valid: boolean, notValidMessage: string): void => {
            if (!valid) {
                throw new Error(notValidMessage);
            }
        }

        public static isUndefinedOrNull = (arg: any): boolean => arg === undefined || arg === null;

        public static isUndefinedOrNullOrEmpty = (arg: string) => Util.isUndefinedOrNull(arg) || arg === "";

        public static unpackFromString = (str: string): number[]=> {
            var byteCharacters = atob(str);
            var byteNumbers = new Array(byteCharacters.length);
            for (var i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            return byteNumbers;
        }

    }

    export class Guid {

        private static s4 = (): string => {
            return Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
        }

        public static newGuid = (): string => {
            return Util.formatString("{0}{1}-{2}-{3}-{4}-{5}{6}{7}",
                Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4());
        }
    }

    export class Paged<T> {
        // reSharper disable InconsistentNaming
        public Items: T[];
        public Count: number;
        // reSharper enable InconsistentNaming
    }

    export interface IQuery<TDone, TFail> {
        fetch(params: string, done: JQueryPromiseCallback<TDone[]>, fail: JQueryPromiseCallback<TFail>): void;
    }

    export interface IPagedQuery<TDone, TFail> {
        fetch(params: string, done: JQueryPromiseCallback<Paged<TDone>>, fail: JQueryPromiseCallback<TFail>): void;
    }

    export enum Method {
        Post,
        Put,
        Delete
    }

    export interface ICommand<TParam, TDone, TFail> {
        execute(params: TParam, done: JQueryPromiseCallback<TDone>, fail: JQueryPromiseCallback<TFail>, method: Method): void;
    }

    export class Query<TDone, TFail> implements IQuery<TDone, TFail> {

        private url: string;

        constructor(url: string) {
            this.url = url;
        }

        public fetch = (params: string, done: JQueryPromiseCallback<TDone[]>, fail: JQueryPromiseCallback<TFail>): void => {
            ajax(this.url, params, done, fail, "GET");
        }
    }

    export class PagedQuery<TDone, TFail> implements IPagedQuery<TDone, TFail> {

        private url: string;

        constructor(url: string) {
            this.url = url;
        }

        public fetch = (params: string, done: JQueryPromiseCallback<Paged<TDone>>, fail: JQueryPromiseCallback<TFail>): void => {
            ajax(this.url, params, done, fail, "GET");
        }
    }

    export class Command<TParam, TDone, TFail> implements ICommand<TParam, TDone, TFail> {

        private url: string;

        constructor(url: string) {
            this.url = url;
        }

        private getMethod(method: Method): string {
            if (method.toString() === "0") {
                return "POST";
            }
            if (method.toString() === "1") {
                return "PUT";
            }
            return "DELETE";
        }

        public execute = (params: TParam, done: JQueryPromiseCallback<TDone>, fail: JQueryPromiseCallback<TFail>, method: Method): void => {
            var m = this.getMethod(method);
            ajax(this.url, params, done, fail, m);
        }
    }

    export class Header {
        public title: string;
        public key: string;
        public order: KnockoutObservable<string> = ko.observable("");
        public orderEnabled: boolean;
        public htmlEncode: boolean;
        public style: KnockoutObservable<string> = ko.observable("");

        constructor(title: string, key: string, orderEnabled: boolean, htmlEncode: boolean) {
            this.title = title;
            this.key = key;
            this.order = ko.observable("");
            this.orderEnabled = orderEnabled;
            this.htmlEncode = htmlEncode;
        }
    }

    export class GridViewModel<TModel> {
        private pagedQuery: IPagedQuery<TModel, any>;

        public pageSize: KnockoutObservable<number> = ko.observable(5);

        constructor(pagedQuery: IPagedQuery<TModel, any>) {
            this.pagedQuery = pagedQuery;
            this.pageSize.subscribe(this.goToFirstPage, this);
        }

        public headers: KnockoutObservableArray<Header> = ko.observableArray([]);
        public currentPage: KnockoutObservable<number> = ko.observable(0);
        public pageCount: KnockoutObservable<number> = ko.observable(0);
        public items: KnockoutObservableArray<TModel> = ko.observableArray([]);
        public itemCount: KnockoutObservable<number> = ko.observable(0);
        public firstPageEnabled: KnockoutObservable<boolean> = ko.observable(false);
        public prevPageEnabled: KnockoutObservable<boolean> = ko.observable(false);
        public nextPageEnabled: KnockoutObservable<boolean> = ko.observable(false);
        public lastPageEnabled: KnockoutObservable<boolean> = ko.observable(false);
        public filterCriteriaExpression: KnockoutObservable<string> = ko.observable("Filter criteria: ");
        public sort: KnockoutObservable<string> = ko.observable("");
        public filterPanelVisible: KnockoutObservable<boolean> = ko.observable(true);
        public filterButtonText: KnockoutComputed<string> = ko.computed(() => {
            return this.filterPanelVisible() ? "-" : "+";
        })
        public filterButtonTooltip: KnockoutComputed<string> = ko.computed(() => {
            return this.filterPanelVisible() ? "Collapse" : "Expand";
        })
        public criteriaTemplate: string;
        public avaliablePageSizes: number[] = [3, 5, 10, 20, 50, 100];
        public criteriaCallback: () => string;
        public filterCriteriaExpressionCallback: () => string[];
        public clearCallback: () => void;
        public setFilterCallback: () => void;
        public getDisplayValueCallback: (obj: TModel, key: string) => string;

        public getDisplayValue(obj: TModel, key: string): string {
            if (this.getDisplayValueCallback == null) {
                throw new Error("getDisplayValueCallback not implemented in parrent class");
            }
            return this.getDisplayValueCallback(obj, key);
        }

        public goToFirstPage(): void {
            this.currentPage(0);
            this.fetchData();
        }

        public goToPrevPage(): void {
            this.currentPage(this.currentPage() - 1);
            this.fetchData();
        }

        public goToNextPage(): void {
            this.currentPage(this.currentPage() + 1);
            this.fetchData();
        }

        public goToLastPage(): void {
            this.currentPage(Math.ceil(this.itemCount() / this.pageSize() - 1));
            this.fetchData();
        }

        public fetchData(): void {
            var skip = this.currentPage() * this.pageSize();
            var criteria = this.criteriaCallback != null ? this.criteriaCallback() : "";
            if (criteria !== "") {
                criteria = Util.formatString("&{0}", criteria);
            }
            var params = Util.formatString("pageSize={0}&skip={1}&sort={2}{3}", this.pageSize().toString(),
                skip.toString(), this.sort(), criteria);
            this.pagedQuery.fetch(
                params,
                (data: any) => {
                    var d = this.parseData(data);
                    this.itemCount(d.Count);
                    this.pageCount(this.calculateTotalPages());
                    this.items(d.Items);
                    this.setButtons();
                },
                (message: any) => {
                    window.alert(message.status);
                }
                );
        }

        public sortItems = (header: Common.Header): void => {
            var order = this.getSortOrder(header.key);
            this.sort(Util.formatString("{0} {1}", header.key, order));
            this.fetchData();
        }

        public toggleFilterPanel(): void {
            this.filterPanelVisible(!this.filterPanelVisible());
        }

        public setFilter(): void {
            if (this.setFilterCallback != null) {
                this.setFilterCallback();
            }
            this.setFilterCriteriaExpression();
            this.goToFirstPage();
        }

        public clearFilter(): void {
            if (this.clearCallback != null) {
                this.clearCallback();
            }
            this.setFilterCriteriaExpression();
            this.goToFirstPage();
        }

        private setFilterCriteriaExpression(): void {
            var arr: string[] = [];
            if (this.filterCriteriaExpressionCallback != null) {
                arr = this.filterCriteriaExpressionCallback();
            }
            this.filterCriteriaExpression(Common.Util.formatString("Filter criteria: {0}", arr.join(" AND ")));
        }

        private parseData(data: any): Paged<TModel> {
            var p = new Paged<TModel>();
            p.Count = data.Count;
            p.Items = data.Items;
            return p;
        }

        private getSortOrder(key: string): string {
            var order = "";
            Enumerable.From<Header>(this.headers()).ForEach((h: Header) => {
                if (h.key !== key) {
                    h.order("");
                    h.style(this.getStyle(h.order()));
                } else {
                    if (h.order() === "" || h.order() === "desc") {
                        h.order("asc");
                        order = "asc";
                    } else {
                        h.order("desc");
                        order = "desc";
                    }
                    h.style(this.getStyle(h.order()));
                }
            });
            return order;
        }

        private getStyle(order: string): string {
            if (order === "asc") {
                return "fa fa-angle-up";
            }
            if (order === "desc") {
                return "fa fa-angle-down";
            }
            return "";
        }

        private setButtons(): void {
            this.lastPageEnabled(this.isLastPageEnabled());
            this.nextPageEnabled(this.isNextPageEnabled());
            this.prevPageEnabled(this.isPrevPageEnabled());
            this.firstPageEnabled(this.isFirstPageEnabled());
        }

        private isLastPageEnabled(): boolean {
            return this.currentPage() < this.calculateTotalPages();
        }

        private isNextPageEnabled(): boolean {
            return this.itemCount() > this.pageSize() * (this.currentPage() + 1);
        }

        private isPrevPageEnabled(): boolean {
            return this.currentPage() > 0;
        }

        private isFirstPageEnabled(): boolean {
            return this.currentPage() > 0;
        }

        private calculateTotalPages(): number {
            var res = Math.ceil(this.itemCount() / this.pageSize() - 1);
            return res === -1 ? 0: res;
        }

    };
}  