import { ScrabbleTile } from './scrabble-tile';
import { ScrabblePlayer } from './scrabble-player';

export class ScrabbleGame {
  id: string;
  startTime: Date;
  lastActiveTime: Date;
  bagTiles: Array<ScrabbleTile>;
  boardTiles: Array<ScrabbleTile>;
  players: Array<ScrabblePlayer>;
  playerTurn: number;
  isFinished: boolean;
}
