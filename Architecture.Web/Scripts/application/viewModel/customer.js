var Application;
(function (Application) {
    var ViewModel;
    (function (_ViewModel) {
        var Customer;
        (function (Customer) {
            "use strict";
            var FindCustomer = (function () {
                function FindCustomer() {
                }
                return FindCustomer;
            })();
            Customer.FindCustomer = FindCustomer;
            var InsertCustomer = (function () {
                function InsertCustomer() {
                }
                return InsertCustomer;
            })();
            Customer.InsertCustomer = InsertCustomer;
            var ViewModel = (function () {
                function ViewModel(insertCommand, findPagedQuery) {
                    var _this = this;
                    this.insertCommand = insertCommand;
                    this.findPagedQuery = findPagedQuery;
                    this.insertCustomer = function () {
                        var insertCustomer = new InsertCustomer();
                        insertCustomer.Mail = Application.Common.Util.formatString("{0}@example.com", Application.Common.Guid.newGuid());
                        insertCustomer.Name = "<script>window.alert('bla');</script>";
                        _this.insertCommand.execute(insertCustomer, function () {
                            window.alert("OK");
                        }, function (data) {
                            window.alert(Application.Common.Util.formatString("Error status: {0}", data.status));
                        }, 0 /* Post */);
                    };
                    this.fetchCustomers = function () {
                        _this.findPagedQuery.fetch("pageSize=10&skip=0&sort=&name=", function (data) {
                            window.alert(data.Count);
                        }, function () {
                            window.alert("Error");
                        });
                    };
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