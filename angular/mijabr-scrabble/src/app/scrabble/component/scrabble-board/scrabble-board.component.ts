import { Component, OnInit, Input, HostListener } from '@angular/core';
import { ScrabbleService } from '../../service/scrabble.service';
import { ScrabbleSquare } from '../../model/scrabble-square';
import { ScrabbleTile } from '../../model/scrabble-tile';
import { ScrabbleGame } from '../../model/scrabble-game';

@Component({
  selector: 'app-scrabble-board',
  templateUrl: './scrabble-board.component.html',
  styleUrls: ['./scrabble-board.component.less']
})
export class ScrabbleBoardComponent implements OnInit {
  @Input() boardSize: string;
  @Input() tileSize: string;

  private squares: ScrabbleSquare[];
  public grid: ScrabbleSquare[][] = new Array<Array<ScrabbleSquare>>(15);

  constructor(
    private scrabbleService: ScrabbleService
  ) { }

  @Input() game: ScrabbleGame;

  ngOnInit() {
    this.scrabbleService.getSquares()
      .subscribe((response: any) => {
        this.squares = response;
        for (let i = 0; i < 15; i++) {
          this.grid[i] = new Array<ScrabbleSquare>(15);
        }
        this.squares.forEach(s => {
          this.grid[s.y][s.x] = s;
        });
      });
  }

  getPlayerBoardTileAt(square: ScrabbleSquare): ScrabbleTile {
    if (this.game && this.game.players && this.game.players[0].tiles) {
      return this.game.players[0].tiles.find(t => t.boardPositionX === square.x && t.boardPositionY === square.y && t.location === 'board');
    }
  }

  getBoardTileAt(square: ScrabbleSquare): ScrabbleTile {
    if (this.game && this.game.boardTiles) {
      return this.game.boardTiles.find(t => t.boardPositionX === square.x && t.boardPositionY === square.y);
    }
  }

  onTileDrop(event: any, x: number, y: number) {
    const tile = event.dragData;
    tile.location = 'board';
    tile.boardPositionX = x;
    tile.boardPositionY = y;
  }
}
