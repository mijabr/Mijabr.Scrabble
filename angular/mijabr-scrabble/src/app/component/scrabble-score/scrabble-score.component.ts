import { Component, OnInit, Input } from '@angular/core';
import { ScrabbleGame } from '../../model/scrabble-game';

@Component({
  selector: 'app-scrabble-score',
  templateUrl: './scrabble-score.component.html',
  styleUrls: ['./scrabble-score.component.less']
})
export class ScrabbleScoreComponent implements OnInit {

  @Input() game: ScrabbleGame;

  constructor() { }

  ngOnInit() {
  }
}
