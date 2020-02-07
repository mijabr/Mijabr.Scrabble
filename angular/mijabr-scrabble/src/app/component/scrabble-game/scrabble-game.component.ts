import { Component, OnInit, AfterViewChecked, ElementRef, ViewChild, HostListener } from '@angular/core';
import { ScrabbleService } from '../../service/scrabble.service';
import { ScrabbleGame } from '../../model/scrabble-game';
import { ScrabblePlayer } from '../../model/scrabble-player';

@Component({
  selector: 'app-scrabble',
  templateUrl: './scrabble-game.component.html',
  styleUrls: ['./scrabble-game.component.less']
})
export class ScrabbleGameComponent implements OnInit, AfterViewChecked  {

  @ViewChild('gameLog', null) private gameLog: ElementRef;

  game: ScrabbleGame;
  message = '';
  public boardSize: string;
  public tileSize: string;
  public fontSize: string;

  constructor(
    private scrabbleService: ScrabbleService
  ) { }

  ngOnInit() {
    this.calculateBoardSize();
    this.continueGame();
  }

  @HostListener('window:resize', [])
  calculateBoardSize() {
    const w = window.innerWidth;
    const h = window.innerHeight * 0.75;
    const size = Math.min(w, h);
    this.boardSize = `${size}px`;
    this.tileSize = `${size * 0.0666}px`;
    this.fontSize = `${size * 0.023}px`;
  }

  continueGame () {
    let game = this.scrabbleService.continueGame();
    if (game !== undefined && game !== null) {
      this.game = game;
      this.addMessage('Welcome back. Your game is still here waiting for you.');
      this.checkAiGo();
    } else {
      this.addMessage('Welcome to scrabble. Click Start New Game to begin.');
    }
  }

  newGame() {
    this.addMessage('Starting a new game. We are using the SOWPODS dictionary. https://www.wordgamedictionary.com/sowpods/');
    this.scrabbleService.newGame().subscribe(response => {
      this.game = response;
      this.addMessage('Good luck!');
      this.scrabbleService.saveGame(this.game);
    });
  }

  quitGame() {
    if (confirm('Are you sure you want to quit?') === true) {
      this.addMessage('Quitting the game.');
      this.scrabbleService.quitGame();
      this.game = undefined;
    }
  }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  checkGameOver(game: ScrabbleGame) {
    if (game.isFinished) {
      const playerWithNoTiles = game.players.find(p => p.tiles.length === 0);
      const winners = this.getWinners(game);
      this.addMessage(`${playerWithNoTiles.name} has no tiles left. `, false);
      this.addMessage('The game is finished. ', false);
      if (winners.length === 1) {
        this.addMessage(`${winners[0].name} has won with ${winners[0].score} points.`);
      } else if (winners.length > 1) {
        this.addMessage(`${winners[0].name} and ${winners[1].name} have tied with ${winners[0].score} points each.`);
      }
    }
  }

  getWinners(game: ScrabbleGame): Array<ScrabblePlayer> {

    let winners: Array<ScrabblePlayer> = [];
    let highest = -1;

    game.players.forEach(player => {
      if (player.score > highest) {
        highest = player.score;
        winners = new Array<ScrabblePlayer>();
        winners.push(player);
      } else if (player.score === highest) {
        winners.push(player);
      }
    });

    return winners;
  }

  checkAiGo() {
    if (this.game.playerTurn > 0 && !this.game.isFinished) {
      this.addMessage('Ok it\'s my go, let me think a bit...', false);
      this.aiGo();
    }
  }

  aiGo() {
    this.scrabbleService.aiGo(this.game).subscribe(response => {
      if (response.isValid) {
        this.game = response.game;
        this.scrabbleService.saveGame(this.game);
      }

      this.addMessage(' Got it!');
      this.submitGo();
    });
  }

  submitGo() {
    this.scrabbleService.submitGo(this.game).subscribe(response => {
      if (response.isValid) {
        this.game = response.game;
        this.scrabbleService.saveGame(this.game);
      }

      this.addMessage(response.message);

      this.checkGameOver(this.game);

      this.checkAiGo();
    });
  }

  addMessage(message: string, newline: boolean = true) {
    if (newline) {
      this.message += message + '<BR>';
    } else {
      this.message += message;
    }
  }

  scrollToBottom() {
    try {
      this.gameLog.nativeElement.scrollTop = this.gameLog.nativeElement.scrollHeight;
    } catch (err) { }
  }
}
