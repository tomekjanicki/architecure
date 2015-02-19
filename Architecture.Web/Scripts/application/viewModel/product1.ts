module Application.ViewModel.Product1 {
    "use strict";

    export class Index {
        // reSharper disable InconsistentNaming
        public Id: number;
        public Code: string;
        public Name: string;
        public Price: number;
        public Date: Date;
        public Version: string;
        // reSharper restore InconsistentNaming

    }

    export class IndexViewModel extends Application.GridView.BaseGridView<Index> {

        constructor(option: Application.GridView.Option<Index>) {
            super(option);
        }

        public code: KnockoutObservable<string> = ko.observable("");
        public name: KnockoutObservable<string> = ko.observable("");

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

        public static getInitializedViewModel(pagedQuery: Common.IPagedQuery<Index, any>,
            query: Common.IQuery<Index, any>): IndexViewModel {
            var o = new Application.GridView.Option<Index>();
            o.filterPanelVisible = true;
            o.pagingEnabled = false;
            o.pagedQuery = pagedQuery;
            o.defaultPageSize = 10;
            o.query = query;
            o.filterPanelCriteriaTemplateName = "criteriaTemplate";
            o.columns.push(new Application.GridView.Column("Id", "Id", "Id", "", ""));
            o.columns.push(new Application.GridView.Column("Code", "Code", "Code", "", ""));
            o.columns.push(new Application.GridView.Column("Name", "Name", "Name", "", ""));
            o.columns.push(new Application.GridView.Column("Price", "Price", "Price", "$0,0.00", ""));
            o.columns.push(new Application.GridView.Column("Date", "Date", "", "YYYY-MM-DD", ""));
            o.columns.push(new Application.GridView.Column("", "", "", "",
                "<a data-bind=\"attr: { href: '\\\\product\\\\edit\\\\' + item.Id }\" class=\"btn btn-default\">Edit</a>"));
            o.errorHandlerCallback = (data: any) => window.alert(data);
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