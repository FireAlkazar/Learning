import { Component } from '@angular/core';

@Component({
  selector: 'my-app',
  template: `<h1 *ngFor="let g of greetings">{{g}}</h1>`
})
export class AppComponent { 
  greetings = ["Hello, world", "Ang2"];
}
