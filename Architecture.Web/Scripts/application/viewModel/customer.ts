module Application.ViewModel.Customer {
    "use strict";

    export class FindCustomerAsync {
        // reSharper disable InconsistentNaming
        public Id: number;
        public Name: string;
        public Mail: string;
        // reSharper restore InconsistentNaming
    }

    export class InsertCustomerAsync {
        // reSharper disable InconsistentNaming
        public Name: string;
        public Mail: string;
        // reSharper restore InconsistentNaming 
    }

    export class ViewModel {

        constructor(insertCommand: Application.Common.ICommand<InsertCustomerAsync, any, any>, findPagedQuery: Application.Common.IPagedQuery<FindCustomerAsync, any>) {
            this.insertCommand = insertCommand;
            this.findPagedQuery = findPagedQuery;
        }

        private insertCommand: Application.Common.ICommand<InsertCustomerAsync, any, any>;
        private findPagedQuery: Application.Common.IPagedQuery<FindCustomerAsync, any>;

        public insertCustomer = (): void => {
            var insertCustomerAsync = new InsertCustomerAsync();
            insertCustomerAsync.Mail = Application.Common.Util.formatString("{0}@example.com", Application.Common.Guid.newGuid());
            insertCustomerAsync.Name = "name";
            this.insertCommand.execute(insertCustomerAsync,() => {
                window.alert("OK");
            },(data: any) => {
                    window.alert(Application.Common.Util.formatString("Error status: {0}", data.status));
                }, Application.Common.Method.Post);
        }

        public fetchCustomers = (): void => {
            this.findPagedQuery.fetch("pageSize=10&skip=0&sort=&name=",(data: Application.Common.Paged<FindCustomerAsync>) => {
                window.alert(data.Count);
            },() => {
                    window.alert("Error");
                });
        }

        public static getInitializedViewModel(pagedQuery: Application.Common.IPagedQuery<FindCustomerAsync, any>,
            insertCommand: Application.Common.ICommand<InsertCustomerAsync, any, any>): ViewModel {
            var vm = new ViewModel(insertCommand, pagedQuery);
            return vm;
        }

    }
} 