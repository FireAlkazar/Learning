import { Component } from '@angular/core';

@Component({
  selector: 'my-app',
  template: `
    <h1>{{title}}</h1>
    <ul>
        <li *ngFor="let message of messages">
            {{message}}
        </li>
    </ul>
  `
})
export class AppComponent { 
    title = "Angular2 example";
    messages = [
        "Hello World!",
        "Another string",
        "Another one",
        "Another one",
        "Another one",
        "...",
        "Another one"
    ];
}