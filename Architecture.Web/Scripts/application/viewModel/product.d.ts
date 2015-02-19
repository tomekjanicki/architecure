/// <reference path="../common.d.ts" />
declare module Application.ViewModel.Product {
    class IndexProduct {
        id: number;
        code: string;
        name: string;
        price: number;
    }
    class IndexViewModel extends Common.GridViewModel<IndexProduct> {
        constructor(pagedQuery: Common.IPagedQuery<IndexProduct, any>);
        code: KnockoutObservable<string>;
        name: KnockoutObservable<string>;
        namex: KnockoutObservable<string>;
        private filterCriteriaExp();
        private getCriteria();
        private clear();
        static getInitializedViewModel(pagedQuery: Common.IPagedQuery<IndexProduct, any>): IndexViewModel;
    }
}
