import { Component, Input, OnInit, DoCheck, Output, EventEmitter } from '@angular/core';
import { ScrabbleTile } from '../../model/scrabble-tile';

@Component({
  selector: 'app-scrabble-tile',
  templateUrl: './scrabble-tile.component.html',
  styleUrls: ['./scrabble-tile.component.less']
})
export class ScrabbleTileComponent implements OnInit, DoCheck {

  constructor() { }

  @Input() tileSize: string;
  @Input() tile: ScrabbleTile;
  @Input() owner: string;
  @Input() shove: string;
  @Output() startDrag: EventEmitter<any> = new EventEmitter();
  @Output() endDrag: EventEmitter<any> = new EventEmitter();

  isDraggable: boolean;

  ngOnInit() { }

  ngDoCheck() {
    this.isDraggable = this.owner !== 'board';
  }

  onKey(event: any) {
    if (this.owner === 'player') {
      this.tile.letter = '';
      if (event.keyCode >= 65 && event.keyCode <= 90) {
        this.tile.letter = event.key.toUpperCase();
      }
    }
  }

  onStartDragTile(tile: ScrabbleTile) {
    this.startDrag.emit(tile);
  }

  onEndDragTile(tile: ScrabbleTile) {
    this.endDrag.emit(tile);
  }
}
