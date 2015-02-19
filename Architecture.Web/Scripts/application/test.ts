module Test {
    "use strict";

    export class X {
        private f: number;

        constructor(f: number) {
            this.f = f;
        }

        public get1(): number {
            return this.f;
        }

        public get2 = (): number => {
            return this.f;
        }
        
        private get3(): number {
            return this.f;
        } 

        private get4 = (): number => this.f;

    }

    function hello(thing) {
        console.log(this + " says hello " + thing);
    }

    hello.call("Yehuda", "world"); //=> Yehuda says hello world

    var x = new X(5);
    x.get2();
} 