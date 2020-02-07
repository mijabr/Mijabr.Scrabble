import { ScrabbleShortPlayer } from './scrabble-short-player';

export class ScrabbleShortGame {
  id: string;
  startTime: Date;
  lastActiveTime: Date;
  players: Array<ScrabbleShortPlayer>;
}
