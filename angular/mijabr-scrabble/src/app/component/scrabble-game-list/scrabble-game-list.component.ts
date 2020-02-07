import { Component, OnInit } from '@angular/core';
import { ScrabbleService } from '../../service/scrabble.service';
import { ScrabbleShortGame } from '../../model/scrabble-short-game';

@Component({
  selector: 'app-scrabble-game-list',
  templateUrl: './scrabble-game-list.component.html',
  styleUrls: ['./scrabble-game-list.component.less']
})
export class ScrabbleGameListComponent implements OnInit {

  gameList: ScrabbleShortGame[];

  constructor(
    private scrabbleService: ScrabbleService
  ) { }

  ngOnInit() {
    this.getGameList();
  }

  getGameList() {
    this.scrabbleService.gameList().subscribe(response => {
      this.gameList = response;
    });
  }
}
