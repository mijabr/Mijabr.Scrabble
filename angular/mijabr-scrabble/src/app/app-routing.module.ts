import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ScrabbleGameComponent } from './component/scrabble-game/scrabble-game.component';
import { ScrabbleGameListComponent } from './component/scrabble-game-list/scrabble-game-list.component';
import { WordSelectorComponent } from './component/word-selector/word-selector.component';
import { UnauthorizedComponent } from './component/unauthorized/unauthorized.component';
import { AuthenticationGuard } from './service/authentication.guard';
import { RedirectComponent } from './component/redirect/redirect.component';

const routes: Routes = [
  { path: '', redirectTo: 'scrabble/game', pathMatch: 'full' },
  { path: 'scrabble/game', component: ScrabbleGameComponent, canActivate: [AuthenticationGuard] },
  { path: 'scrabble/list', component: ScrabbleGameListComponent, canActivate: [AuthenticationGuard] },
  { path: 'scrabble/words', component: WordSelectorComponent, canActivate: [AuthenticationGuard] },
  { path: 'redirect', component: RedirectComponent },
  { path: 'unauthorized', component: UnauthorizedComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
