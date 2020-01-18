import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ScrabbleGame } from '../../model/scrabble-game';

@Component({
  selector: 'app-scrabble-toolbar',
  templateUrl: './scrabble-toolbar.component.html',
  styleUrls: ['./scrabble-toolbar.component.less']
})
export class ScrabbleToolbarComponent implements OnInit {

  @Input() game: ScrabbleGame;
  @Output() newGame: EventEmitter<string> = new EventEmitter();
  @Output() quitGame: EventEmitter<string> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onNewGame() {
    this.newGame.emit(null);
  }

  onQuitGame() {
    this.quitGame.emit(null);
  }

}
