/// <reference path="../common.ts" />
var Application;
(function (Application) {
    var Test;
    (function (Test) {
        var Helper;
        (function (Helper) {
            "use strict";
            var FakeQuery = (function () {
                function FakeQuery(doneResult, failResult) {
                    this.doneResult = doneResult;
                    this.failResult = failResult;
                }
                FakeQuery.prototype.fetch = function (params, done, fail) {
                    if (this.doneResult != null) {
                        done(this.doneResult);
                    }
                    else {
                        fail(this.failResult);
                    }
                };
                return FakeQuery;
            })();
            Helper.FakeQuery = FakeQuery;
            var FakePagedQuery = (function () {
                function FakePagedQuery(doneResult, failResult) {
                    this.doneResult = doneResult;
                    this.failResult = failResult;
                }
                FakePagedQuery.prototype.fetch = function (params, done, fail) {
                    if (this.doneResult != null) {
                        done(this.doneResult);
                    }
                    else {
                        fail(this.failResult);
                    }
                };
                return FakePagedQuery;
            })();
            Helper.FakePagedQuery = FakePagedQuery;
            var FakeCommand = (function () {
                function FakeCommand(testResult) {
                    this.testResult = testResult;
                }
                FakeCommand.prototype.execute = function (params, done, fail, method) {
                    done(this.testResult);
                };
                return FakeCommand;
            })();
            Helper.FakeCommand = FakeCommand;
        })(Helper = Test.Helper || (Test.Helper = {}));
    })(Test = Application.Test || (Application.Test = {}));
})(Application || (Application = {}));
//# sourceMappingURL=helper.js.map