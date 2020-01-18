import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-icon-star',
  template: `
<svg xmlns='http://www.w3.org/2000/svg' xmlns:xlink='http://www.w3.org/1999/xlink' version='1.1' width='24' height='24' viewBox='0 0 24 24'
  style.background-color='{{background}}' attr.fill='{{fill}}' style.width='{{size}}' style.height='{{size}}'>
  <path d="M12,17.27L18.18,21L16.54,13.97L22,9.24L14.81,8.62L12,2L9.19,8.62L2,9.24L7.45,13.97L5.82,21L12,17.27Z" />
</svg>`,
})
export class IconStarComponent {
  @Input() fill = '#777';
  @Input() background = '#fff';
  @Input() size = '40';
}
