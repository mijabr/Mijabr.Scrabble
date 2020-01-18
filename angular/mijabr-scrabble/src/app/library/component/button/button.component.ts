import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'lib-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.less']
})
export class ButtonComponent implements OnInit {

  @Input() usage: string;
  @Input() type = 'button';
  @Input() enabled = true;

  constructor() { }

  ngOnInit() { }

  get backGroundColor() {
    if (this.usage === 'primary') {
      return 'blue';
    } else if (this.usage === 'secondary') {
      return 'white';
    }
    return 'green';
  }
}
