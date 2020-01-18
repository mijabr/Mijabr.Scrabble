import { Component, Input } from '@angular/core';

@Component({
  selector: 'lib-icon-account',
  template: `
<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' version='1.1' width='24' height='24' viewBox='0 0 24 24'
  style.background-color='{{background}}' attr.fill='{{fill}}' style.width='{{size}}' style.height='{{size}}'>
  <path d="M12,4A4,4 0 0,1 16,8A4,4 0 0,1 12,12A4,4 0 0,1 8,8A4,4 0 0,1 12,4M12,14C16.42,14 20,15.79 20,18V20H4V18C4,15.79 7.58,14 12,14Z"
  />
</svg>`,
})
export class IconAccountComponent {
  @Input() fill = '#777';
  @Input() background = '#fff';
  @Input() size = '40';
}
