import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { ScrabbleGame } from '../../model/scrabble-game';
import { ScrabbleTile } from '../../model/scrabble-tile';

@Component({
  selector: 'app-scrabble-player',
  templateUrl: './scrabble-player.component.html',
  styleUrls: ['./scrabble-player.component.less']
})
export class ScrabblePlayerComponent implements OnInit {

  @Input() boardSize: string;
  @Input() tileSize: string;
  @Input() game: ScrabbleGame;
  @Output() go: EventEmitter<any> = new EventEmitter();
  @Output() message: EventEmitter<string> = new EventEmitter();

  trayPlaceholders = [0, 1, 2, 3, 4, 5, 6];
  shove = ['', '', '', '', '', '', ''];
  draggingFromTrayPosition = -1;

  constructor(
    private changeDetector: ChangeDetectorRef
  ) { }

  ngOnInit() { }

  getPlayerTileAtTrayPosition(trayPosition: number): ScrabbleTile {
    return this.game.players[0].tiles.find(t => t.location === 'tray' && t.trayPosition === trayPosition);
  }

  onTileDragStart(tile: ScrabbleTile) {
    this.draggingFromTrayPosition = tile.trayPosition;
  }

  onTileDragEnd(tile: ScrabbleTile) {
    this.draggingFromTrayPosition = -1;
    this.clearShove();
  }

  onTileDroppedOnPlaceholder(event: any, trayPosition: number) {
    this.clearShove();
    const tile: ScrabbleTile = event.dragData;
    if (tile.location !== 'tray' || tile.trayPosition !== trayPosition) {
      this.movePlayerTileToTrayPosition(tile, trayPosition);
    }
  }

  onDragEnterTrayTile(tile: ScrabbleTile) {
    this.clearShove();
    this.shoveTilesAwayFromTrayPosition(tile.trayPosition);
    this.changeDetector.detectChanges();
  }

  onResetTiles() {
    this.moveAllPlayerTilesToTray();
    this.clearShove();
  }

  onSubmitGo() {
    if (this.game.isFinished) {
      this.message.emit('Sorry but this game is already done.');
      return;
    } else if (this.game.playerTurn === 0) {
      this.go.emit(null);
      this.clearShove();
    } else {
      this.message.emit('Hey, it\'s not your go!');
    }
  }

  moveAllPlayerTilesToTray() {
    this.game.players[0].tiles.forEach(t => {
      if (t.location !== 'tray') {
        t.location = 'tray';
        if (this.getTrayPositionTileCount(t.trayPosition) > 1) {
          t.trayPosition = this.getFirstFreeTrayPosition();
        }
      }
    });
  }

  movePlayerTileToTrayPosition(tile: any, trayPosition: number) {
    tile.trayPosition = -1;
    if (!this.isTrayPositionFree(trayPosition)) {
      const freeTrayPosition = this.getFirstFreeTrayPosition();
      if (freeTrayPosition < trayPosition) {
        for (let i = freeTrayPosition + 1; i <= trayPosition; i++) {
          this.movePlayerTileAtTrayPosition(i, -1);
        }
      } else if (freeTrayPosition >= trayPosition) {
        for (let i = freeTrayPosition - 1; i >= trayPosition; i--) {
          this.movePlayerTileAtTrayPosition(i, 1);
        }
      }
    }

    tile.location = 'tray';
    tile.trayPosition = trayPosition;
  }

  movePlayerTileAtTrayPosition(trayPosition: number, direction: number) {
    for (let i = 0; i < this.game.players[0].tiles.length; i++) {
      if (this.game.players[0].tiles[i].location === 'tray' && this.game.players[0].tiles[i].trayPosition === trayPosition) {
        this.game.players[0].tiles[i].trayPosition = this.game.players[0].tiles[i].trayPosition + direction;
      }
    }
  }

  getTrayPositionTileCount(trayPosition: number): number {
    let count = 0;
    this.game.players[0].tiles.forEach(t => {
      if (t.trayPosition === trayPosition) {
        count++;
      }
    });

    return count;
  }

  getFirstFreeTrayPosition(): number {
    for (let trayPosition = 0; trayPosition < 7; trayPosition++) {
      if (this.isTrayPositionFree(trayPosition)) {
        return trayPosition;
      }
    }
  }

  isTrayPositionFree(trayPosition: number) {
    let isPositionFree = true;
    this.game.players[0].tiles.forEach(t => {
      if (t.trayPosition === trayPosition && t.location === 'tray') {
        isPositionFree = false;
      }
    });
    return isPositionFree;
  }

  clearShove() {
    for (let i = 0; i < this.shove.length; ++i) {
      this.shove[i] = '';
    }
  }

  shoveTilesAwayFromTrayPosition(trayPosition: number) {
    let freeTrayPosition = this.getFirstFreeTrayPosition();
    if (freeTrayPosition === undefined) {
      freeTrayPosition = this.draggingFromTrayPosition;
    }
    if (freeTrayPosition < trayPosition) {
      for (let i = trayPosition; i > freeTrayPosition; i--) {
        this.shove[i] = 'left';
      }
    } else if (freeTrayPosition > trayPosition) {
      for (let i = trayPosition; i < freeTrayPosition; i++) {
        this.shove[i] = 'right';
      }
    }
  }
}
