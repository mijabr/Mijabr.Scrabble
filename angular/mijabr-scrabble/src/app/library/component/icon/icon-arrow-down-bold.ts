import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-icon-arrow-down-bold',
  template: `
<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' version='1.1' width='24' height='24' viewBox='0 0 24 24'
  style.background-color='{{background}}' attr.fill='{{fill}}' style.width='{{size}}' style.height='{{size}}'>
  <path d="M9,4H15V12H19.84L12,19.84L4.16,12H9V4Z" />
</svg>`,
})
export class IconArrowDownBoldComponent {
  @Input() fill = '#777';
  @Input() background = '#fff';
  @Input() size = '40';
}
