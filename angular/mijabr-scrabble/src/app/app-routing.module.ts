import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ScrabbleGameComponent } from './scrabble/component/scrabble-game/scrabble-game.component';
import { ScrabbleGameListComponent } from './scrabble/component/scrabble-game-list/scrabble-game-list.component';
import { WordSelectorComponent } from './words/component/word-selector/word-selector.component';

const routes: Routes = [
  { path: '', redirectTo: 'scrabble/game', pathMatch: 'full' },
  { path: 'scrabble/game', component: ScrabbleGameComponent },
  { path: 'scrabble/list', component: ScrabbleGameListComponent },
  { path: 'scrabble/words', component: WordSelectorComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
