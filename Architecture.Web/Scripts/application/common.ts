/// <reference path="../typings/jquery/jquery.d.ts" />
/// <reference path="../typings/knockout/knockout.d.ts" />
/// <reference path="../typings/jquery.blockui/jquery.blockui.d.ts" />
/// <reference path="../typings/linq/linq.d.ts" />

module Application.Common {
    "use strict";

    function ajax(url: string, params: any, done: JQueryPromiseCallback<any>, fail: JQueryPromiseCallback<any>, method: string): void {
        var s: JQueryAjaxSettings = {};
        s.url = url;
        s.type = method;
        s.data = params;
        if (method !== "GET") {
            var token = $("input[name='__RequestVerificationToken']").val();
            if (token) {
                s.headers = {
                    "RequestVerificationToken": token
                };
            }
        }
        $.ajax(s).done(done).fail(fail);
    }

    export class Util {

        private static blockUi = (): void => {
            var msg = "<h1>Processing request... Please wait.</h1>";
            var opt: JQBlockUIOptions = {
                message: msg,
                css: { border: "none" }
            };
            $.blockUI(opt);
        }
        static initBlockUiForNonAjax = (): void => {
            $(document).on("click", "a[data-blockui], input[data-blockui]", Util.blockUi);
        }
        static initBlockUiForAjaxRequests = (): void => {
            $(document).ajaxStart(() => Util.blockUi());
            $(document).ajaxStop(() => $.unblockUI());
        }
        static formatString = (s: string, ...params: string[]): string => {
            var i = params.length;
            while (i--) {
                s = s.replace(new RegExp("\\{" + i + "\\}", "gm"), params[i]);
            }
            return s;
        }
        static getLikeExpression = (field: string, value: string): string => {
            return Util.formatString("[{0}] like \"%{1}%\"", field, value);
        }
        static validateSettings = (valid: boolean, notValidMessage: string): void => {
            if (!valid) {
                throw new Error(notValidMessage);
            }
        }
        static isUndefinedOrNull = (arg: any): boolean => arg === undefined || arg === null;
        static isUndefinedOrNullOrEmpty = (arg: string) => Util.isUndefinedOrNull(arg) || arg === "";
        static unpackFromString = (str: string): number[]=> {
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
        static newGuid = (): string => {
            return Util.formatString("{0}{1}-{2}-{3}-{4}-{5}{6}{7}",
                Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4(), Guid.s4());
        }
    }

    export class Paged<T> {
        // reSharper disable InconsistentNaming
        Items: T[];
        Count: number;
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

        constructor(private url: string) {
        }

        fetch = (params: string, done: JQueryPromiseCallback<TDone[]>, fail: JQueryPromiseCallback<TFail>): void => {
            ajax(this.url, params, done, fail, "GET");
        }
    }

    export class PagedQuery<TDone, TFail> implements IPagedQuery<TDone, TFail> {

        constructor(private url: string) {
        }

        fetch = (params: string, done: JQueryPromiseCallback<Paged<TDone>>, fail: JQueryPromiseCallback<TFail>): void => {
            ajax(this.url, params, done, fail, "GET");
        }
    }

    export class Command<TParam, TDone, TFail> implements ICommand<TParam, TDone, TFail> {

        constructor(private url: string) {
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

        execute = (params: TParam, done: JQueryPromiseCallback<TDone>, fail: JQueryPromiseCallback<TFail>, method: Method): void => {
            var m = this.getMethod(method);
            ajax(this.url, params, done, fail, m);
        }
    }

    Util.initBlockUiForAjaxRequests();
    Util.initBlockUiForNonAjax();
}  