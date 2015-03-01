/// <reference path="../common.ts" />

module Application.Test.Helper {
    "use strict";

    export class FakeQuery<TDone, TFail> implements Application.Common.IQuery<TDone, TFail> {

        constructor(private doneResult: TDone[], private failResult: TFail) {
        }

        fetch(params: string, done: JQueryPromiseCallback<TDone[]>,
            fail: JQueryPromiseCallback<TFail>): void {
            if (this.doneResult != null) {
                done(this.doneResult);
            } else {
                fail(this.failResult);
            }
        }
    }

    export class FakePagedQuery<TDone, TFail> implements Application.Common.IPagedQuery<TDone, TFail> {

        constructor(private doneResult: Application.Common.Paged<TDone>, private failResult: TFail) {
        }

        fetch(params: string, done: JQueryPromiseCallback<Application.Common.Paged<TDone>>,
            fail: JQueryPromiseCallback<TFail>): void {
            if (this.doneResult != null) {
                done(this.doneResult);
            } else {
                fail(this.failResult);
            }
        }
    }

    export class FakeCommand<TParam, TDone, TFail> implements Application.Common.ICommand<TParam, TDone, TFail> {

        constructor(private testResult: TDone) {
        }

        execute(params: TParam, done: JQueryPromiseCallback<TDone>, fail: JQueryPromiseCallback<TFail>,
            method: Application.Common.Method): void {
            done(this.testResult);
        }
    }

}
