/// <reference path="../common.ts" />

module Application.ViewModel.Product {
    "use strict";

    export class IndexProduct {
        // reSharper disable InconsistentNaming
        public Id: number;
        public Code: string;
        public Name: string;
        public Price: number;
        public Version: string;
        // reSharper restore InconsistentNaming
    }

    export class IndexViewModel extends Common.GridViewModel<IndexProduct> {

        private deleteProductCommand: Common.ICommand<number, any, any>

        constructor(pagedQuery: Common.IPagedQuery<IndexProduct, any>, deleteProductCommand: Common.ICommand<number, any, any>) {
            super(pagedQuery);
            this.criteriaCallback = this.getCriteria;
            this.deleteProductCommand = deleteProductCommand;
            this.clearCallback = this.clear;
            this.setFilterCallback = this.setFilterCriteria;
            this.filterCriteriaExpressionCallback = this.filterCriteriaExp;
            this.getDisplayValueCallback = this.getDisplayVal;
            this.headers([
                new Common.Header("Id", "id", true, true),
                new Common.Header("Code", "code", true, true),
                new Common.Header("Name", "name", true, true),
                new Common.Header("Price", "price", true, true),
                new Common.Header("", "edit", false, false),
                new Common.Header("", "delete", false, false)
            ]);
            this.criteriaTemplate = "criteriaTemplate";
        }

        public code: KnockoutObservable<string> = ko.observable("");
        public name: KnockoutObservable<string> = ko.observable("");

        private codeLocal: string = "";
        private nameLocal: string = "";

        private getDisplayVal(obj: IndexProduct, key: string): string {
            switch (key) {
                case "id":
                    return obj.Id.toFixed(0);
                case "code":
                    return obj.Code;
                case "name":
                    return obj.Name;
                case "price":
                    return obj.Price.toFixed(2);
                case "edit":
                    return "<a href=\"product\\edit\\0\" class=\"btn btn-default\">Edit</a>";
                case "delete":
                    return "<a href=\"#\" class=\"btn btn-default\">Delete</a>";
                default:
                    throw new Error("Not implemented");
            }
        }

        private filterCriteriaExp(): string[] {
            var arr: string[] = [];
            if (this.codeLocal !== "") {
                arr.push(Common.Util.getLikeExpression("Code", this.codeLocal));
            }
            if (this.nameLocal !== "") {
                arr.push(Common.Util.getLikeExpression("Name", this.nameLocal));
            }
            return arr;
        }

        private getCriteria(): string {
            return Common.Util.formatString("code={0}&name={1}", this.codeLocal, this.nameLocal);
        }

        private setFilterCriteria(): void {
            this.codeLocal = this.code();
            this.nameLocal = this.name();
        }

        private clear(): void {
            this.code("");
            this.name("");
            this.codeLocal = this.code();
            this.nameLocal = this.name();
        }

        public static getInitializedViewModel(pagedQuery: Common.IPagedQuery<IndexProduct, any>,
            deleteProductCommand: Common.ICommand<number, any, any>): IndexViewModel {
            var vm = new IndexViewModel(pagedQuery, deleteProductCommand);
            vm.fetchData();
            return vm;
        }
    };
}