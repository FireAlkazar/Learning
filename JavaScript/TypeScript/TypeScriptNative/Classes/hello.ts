class Person {
	_name;
	constructor(name: string) {
		this._name = name;
	}
	
	sayHello() : void {
		console.log("Hello from " + this._name);
	}
} 

var person = new Person("Bob");
person.sayHello();
