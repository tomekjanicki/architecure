module Application.ViewModel.Customer {
    "use strict";

    export class FindCustomerAsync {
        // reSharper disable InconsistentNaming
        Id: number;
        Name: string;
        Mail: string;
        // reSharper restore InconsistentNaming
    }

    export class InsertCustomerAsync {
        // reSharper disable InconsistentNaming
        Name: string;
        Mail: string;
        // reSharper restore InconsistentNaming 
    }

    export class ViewModel {

        constructor(private insertCommand: Common.ICommand<InsertCustomerAsync, any, any>,
            private findPagedQuery: Common.IPagedQuery<FindCustomerAsync, any>) {
        }

        insertCustomer = (): void => {
            var insertCustomerAsync = new InsertCustomerAsync();
            insertCustomerAsync.Mail = Common.Util.formatString("{0}@example.com", Common.Guid.newGuid());
            insertCustomerAsync.Name = "name";
            this.insertCommand.execute(insertCustomerAsync,() => {
                window.alert("OK");
            },(data: any) => {
                    window.alert(Common.Util.formatString("Error status: {0}", data.status));
                }, Common.Method.Post);
        }
        fetchCustomers = (): void => {
            this.findPagedQuery.fetch("pageSize=10&skip=0&sort=&name=", (data: Common.Paged<FindCustomerAsync>) => {
                window.alert(data.Count);
            },() => {
                    window.alert("Error");
                });
        }

        static getInitializedViewModel(pagedQuery: Common.IPagedQuery<FindCustomerAsync, any>,
            insertCommand: Common.ICommand<InsertCustomerAsync, any, any>): ViewModel {
            var vm = new ViewModel(insertCommand, pagedQuery);
            return vm;
        }

    }
} 