module Application.ViewModel.Customer {
    "use strict";

    export class FindCustomer {
        id: number;
        name: string;
        mail: string;
    }

    export class InsertCustomer {
        name: string;
        mail: string;
    }

    export class ViewModel {

        constructor(private insertCommand: Common.ICommand<InsertCustomer, any, any>,
            private findPagedQuery: Common.IPagedQuery<FindCustomer, any>) {
        }

        insertCustomer = (): void => {
            var insertCustomer = new InsertCustomer();
            insertCustomer.mail = Common.Util.formatString("{0}@example.com", Common.Guid.newGuid());
            insertCustomer.name = "<script>window.alert('bla');</script>";
            this.insertCommand.execute(insertCustomer,() => {
                window.alert("OK");
            },(data: any) => {
                    window.alert(Common.Util.formatString("Error status: {0}", data.status));
                }, Common.Method.Post);
        }
        fetchCustomers = (): void => {
            this.findPagedQuery.fetch("pageSize=10&skip=0&sort=&name=", (data: Common.Paged<FindCustomer>) => {
                window.alert(data.count);
            },() => {
                    window.alert("Error");
                });
        }

        static getInitializedViewModel(pagedQuery: Common.IPagedQuery<FindCustomer, any>,
            insertCommand: Common.ICommand<InsertCustomer, any, any>): ViewModel {
            var vm = new ViewModel(insertCommand, pagedQuery);
            return vm;
        }

    }
} 