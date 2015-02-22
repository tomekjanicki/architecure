var Application;
(function (Application) {
    var ViewModel;
    (function (_ViewModel) {
        var Customer;
        (function (Customer) {
            "use strict";
            var FindCustomerAsync = (function () {
                function FindCustomerAsync() {
                }
                return FindCustomerAsync;
            })();
            Customer.FindCustomerAsync = FindCustomerAsync;
            var InsertCustomerAsync = (function () {
                function InsertCustomerAsync() {
                }
                return InsertCustomerAsync;
            })();
            Customer.InsertCustomerAsync = InsertCustomerAsync;
            var ViewModel = (function () {
                function ViewModel(insertCommand, findPagedQuery) {
                    var _this = this;
                    this.insertCustomer = function () {
                        var insertCustomerAsync = new InsertCustomerAsync();
                        insertCustomerAsync.Mail = "example@example.com";
                        insertCustomerAsync.Name = "name";
                        _this.insertCommand.execute(insertCustomerAsync, function () {
                            window.alert("OK");
                        }, function () {
                            window.alert("Error");
                        }, 0 /* Post */);
                    };
                    this.fetchCustomers = function () {
                        _this.findPagedQuery.fetch("pageSize=10&skip=0&sort=&name=", function (data) {
                            window.alert(data.Count);
                        }, function () {
                            window.alert("Error");
                        });
                    };
                    this.insertCommand = insertCommand;
                    this.findPagedQuery = findPagedQuery;
                }
                ViewModel.getInitializedViewModel = function (pagedQuery, insertCommand) {
                    var vm = new ViewModel(insertCommand, pagedQuery);
                    return vm;
                };
                return ViewModel;
            })();
            Customer.ViewModel = ViewModel;
        })(Customer = _ViewModel.Customer || (_ViewModel.Customer = {}));
    })(ViewModel = Application.ViewModel || (Application.ViewModel = {}));
})(Application || (Application = {}));
//# sourceMappingURL=customer.js.map