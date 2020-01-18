import { Injectable } from '@angular/core';
import { ScrabbleSquare } from '../model/scrabble-square';
import { ScrabbleGame } from '../model/scrabble-game';
// import { Jsonp } from '@angular/http';
import { ScrabbleShortGame } from '../model/scrabble-short-game';
import { Observable } from 'rxjs';
import { ApiService } from '../../service/api.service';

@Injectable()
export class ScrabbleService {

  constructor(
    private apiService: ApiService
  ) { }

  getSquares(): Observable<ScrabbleSquare[]> {
    return this.apiService.post('scrabble/api/scrabble/getsquares');
  }

  continueGame(): Observable<ScrabbleGame> {
    const gameJson = localStorage.getItem('scrabble-game');
    const game: ScrabbleGame = JSON.parse(gameJson);
    return Observable.create(game);
  }

  saveGame(game: ScrabbleGame) {
    const gameJson = JSON.stringify(game);
    localStorage.setItem('scrabble-game', gameJson);
  }

  quitGame() {
    localStorage.removeItem('scrabble-game');
  }

  newGame(): Observable<ScrabbleGame> {
    return this.apiService.post('scrabble/api/scrabble/newgame');
  }

  submitGo(game: ScrabbleGame): Observable<any> {
    return this.apiService.post('scrabble/api/scrabble/submitgo', game);
  }

  aiGo(game: ScrabbleGame): Observable<any> {
    return this.apiService.post('scrabble/api/scrabble/aigo', game);
  }

  gameList(): Observable<Array<ScrabbleShortGame>> {
    return this.apiService.post('scrabble/api/scrabble/shortlist');
  }
}
