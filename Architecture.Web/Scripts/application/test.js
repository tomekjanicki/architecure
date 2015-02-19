var Test;
(function (Test) {
    "use strict";
    var X = (function () {
        function X(f) {
            var _this = this;
            this.get2 = function () {
                return _this.f;
            };
            this.get4 = function () { return _this.f; };
            this.f = f;
        }
        X.prototype.get1 = function () {
            return this.f;
        };
        X.prototype.get3 = function () {
            return this.f;
        };
        return X;
    })();
    Test.X = X;
    function hello(thing) {
        console.log(this + " says hello " + thing);
    }
    hello.call("Yehuda", "world"); //=> Yehuda says hello world
    var x = new X(5);
    x.get2();
})(Test || (Test = {}));
//# sourceMappingURL=test.js.map