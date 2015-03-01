/// <reference path="../../typings/numeraljs/numeraljs.d.ts" />
/// <reference path="../../typings/moment/moment.d.ts" />
/// <reference path="../gridview.ts" />
/// <reference path="../../typings/knockstrap/knockstrap.d.ts" />
module Application.ViewModel.Product {
    "use strict";

    export class IndexOption extends GridView.Option<Index> {
        deleteCommand: Common.ICommand<Delete, any, any> = null;
    }

    export class Index {
        // reSharper disable InconsistentNaming
        Id: number;
        Code: string;
        Name: string;
        Price: number;
        Date: Date;
        Version: string;
        CanDelete: boolean;
        // reSharper restore InconsistentNaming

    }

    export class Delete {
        // reSharper disable InconsistentNaming
        Id: number;
        Version: number[];
        // reSharper restore InconsistentNaming
    }

    export class IndexViewModel extends GridView.BaseGridView<Index> {

        constructor(option: GridView.Option<Index>) {
            super(option);
        }

        code = ko.observable("");
        name = ko.observable("");
        confirmationVisible = ko.observable(false);
        confirmingItem: Index;
        showConfirmation = (item: Index): void => {
            this.confirmingItem = item;
            this.confirmationVisible(true);
        }

        private codeLocal = "";
        private nameLocal = "";

        private criteriaExpression = (): string[]=> {
            var arr: string[] = [];
            if (this.codeLocal !== "") {
                arr.push(Common.Util.getLikeExpression("Code", this.codeLocal));
            }
            if (this.nameLocal !== "") {
                arr.push(Common.Util.getLikeExpression("Name", this.nameLocal));
            }
            return arr;
        }

        private getOption = (): IndexOption => {
            return <IndexOption>this.option;
        }

        private criteria = (): string => Common.Util.formatString("code={0}&name={1}", this.codeLocal, this.nameLocal);

        private swapValues = (): void => {
            this.codeLocal = this.code();
            this.nameLocal = this.name();
        }

        private setButton = (): void => this.swapValues();

        private clearButton = (): void => {
            this.code("");
            this.name("");
            this.swapValues();
        }

        private successDelete = (): void => {
            this.refresh();
            this.confirmationVisible(false);
        }
        deleteOrder = (): void => {
            var p = new Delete();
            p.Id = this.confirmingItem.Id;
            p.Version = Common.Util.unpackFromString(this.confirmingItem.Version);
            var option = this.getOption();
            option.deleteCommand.execute(p, this.successDelete, option.errorHandlerCallback, Common.Method.Delete);
        }

        static getInitializedViewModel(pagedQuery: Common.IPagedQuery<Index, any>,
            query: Common.IQuery<Index, any>,
            deleteCommand: Common.ICommand<Delete, any, any>): IndexViewModel {
            var o = new IndexOption();
            o.filterPanelVisible = true;
            o.pagingEnabled = true;
            o.pagedQuery = pagedQuery;
            o.defaultPageSize = 10;
            o.query = query;
            o.deleteCommand = deleteCommand;
            o.filterPanelCriteriaTemplateName = "criteriaTemplate";
            o.columns.push(new GridView.Column("Id", "Id", "Id", "", "", ""));
            o.columns.push(new GridView.Column("Code", "Code", "Code", "", "", ""));
            o.columns.push(new GridView.Column("Name", "Name", "Name", "", "", ""));
            o.columns.push(new GridView.Column("Price", "Price", "Price", "$0,0.00", "", ""));
            o.columns.push(new GridView.Column("Date", "Date", "", "YYYY-MM-DD", "", ""));
            o.columns.push(new GridView.Column("", "", "", "",
                "<a data-bind=\"attr: { href: '\\\\product\\\\edit\\\\' + item.Id }\" class=\"btn btn-default\" " +
                "title=\"Edit product\" data-blockui=\"\">Edit</a>", ""));
            o.columns.push(new GridView.Column("", "", "", "", "", "deleteProduct"));
            o.errorHandlerCallback = (data: any) => window.alert(data.status);
            var vm = new IndexViewModel(o);
            o.filterPanelClearButtonCallback = vm.clearButton;
            o.filterPanelSetButtonCallback = vm.setButton;
            o.filterPanelCriteriaCallback = vm.criteria;
            o.filterPanelCriteriaExpressionCallback = vm.criteriaExpression;
            o.defaultCriteriaCallback = vm.criteria;
            vm.fetchData();
            return vm;
        }
    };

} 