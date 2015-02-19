module Application.ViewModel.Product {
    "use strict";

    export class IndexOption extends Application.GridView.Option<Index> {

        public deleteCommand: Application.Common.ICommand<Delete, any, any> = null;
    }

    export class Index {
        // reSharper disable InconsistentNaming
        public Id: number;
        public Code: string;
        public Name: string;
        public Price: number;
        public Date: Date;
        public Version: string;
        public CanDelete: boolean;
        // reSharper restore InconsistentNaming

    }

    export class Delete {
        // reSharper disable InconsistentNaming
        public Id: number;
        public Version: number[];
        // reSharper restore InconsistentNaming
    }

    export class IndexViewModel extends Application.GridView.BaseGridView<Index> {

        constructor(option: Application.GridView.Option<Index>) {
            super(option);
        }

        public code: KnockoutObservable<string> = ko.observable("");
        public name: KnockoutObservable<string> = ko.observable("");
        public confirmationVisible: KnockoutObservable<boolean> = ko.observable(false);
        public confirmingItem: Index;

        public showConfirmation = (item: Index): void => {
            this.confirmingItem = item;
            this.confirmationVisible(true);
        }

        private codeLocal: string = "";
        private nameLocal: string = "";

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

        public deleteOrder = (): void => {
            var p = new Delete();
            p.Id = this.confirmingItem.Id;
            p.Version = Common.Util.unpackFromString(this.confirmingItem.Version);
            var option = this.getOption();
            option.deleteCommand.execute(p, this.successDelete, option.errorHandlerCallback, Application.Common.Method.Delete);
        }

        public static getInitializedViewModel(pagedQuery: Common.IPagedQuery<Index, any>,
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
            o.columns.push(new Application.GridView.Column("Id", "Id", "Id", "", "", ""));
            o.columns.push(new Application.GridView.Column("Code", "Code", "Code", "", "", ""));
            o.columns.push(new Application.GridView.Column("Name", "Name", "Name", "", "", ""));
            o.columns.push(new Application.GridView.Column("Price", "Price", "Price", "$0,0.00", "", ""));
            o.columns.push(new Application.GridView.Column("Date", "Date", "", "YYYY-MM-DD", "", ""));
            o.columns.push(new Application.GridView.Column("", "", "", "",
                "<a data-bind=\"attr: { href: '\\\\product\\\\edit\\\\' + item.Id }\" class=\"btn btn-default\" " +
                "title=\"Edit product\" data-blockui=\"\">Edit</a>", ""));
            o.columns.push(new Application.GridView.Column("", "", "", "", "", "deleteProduct"));
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