import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-expander-button',
  templateUrl: './expander-button.component.html',
  styleUrls: ['./expander-button.component.less']
})
export class ExpanderButtonComponent implements OnInit {

  @Output() onExpand: EventEmitter<boolean> = new EventEmitter<boolean>();
  isExpanded = false;
  constructor() { }

  ngOnInit() {
  }

  onClick() {
    this.isExpanded = !this.isExpanded;
    this.onExpand.emit(this.isExpanded);

  }

}
