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
            Util.blockUi = function () {
                var msg = "<h1>Processing request... Please wait.</h1>";
                var opt = {
                    message: msg,
                    css: { border: "none" }
                };
                $.blockUI(opt);
            };
            Util.initBlockUiForNonAjax = function () {
                $(document).on("click", "a[data-blockui], input[data-blockui]", Util.blockUi);
            };
            Util.initBlockUiForAjaxRequests = function () {
                $(document).ajaxStart(function () { return Util.blockUi(); });
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
        Application.Common.Util.initBlockUiForAjaxRequests();
        Application.Common.Util.initBlockUiForNonAjax();
    })(Common = Application.Common || (Application.Common = {}));
})(Application || (Application = {}));
//# sourceMappingURL=common.js.map