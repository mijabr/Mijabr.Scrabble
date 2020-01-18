import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'lib-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.less']
})
export class ModalComponent implements OnInit {

  @Input() show = false;
  @Input() width = '16em';
  @Input() height = 'auto';
  @Output() dismiss: EventEmitter<any> = new EventEmitter<any>();

  constructor() { }

  ngOnInit() { }

  onClickBackdrop() {
    this.dismiss.emit(null);
  }

  onClickContent(event: any) {
    event.stopPropagation();
   }
}
