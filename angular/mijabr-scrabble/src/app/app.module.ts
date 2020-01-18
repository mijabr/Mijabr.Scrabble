import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LibraryModule } from './library/module/library.module';
import { AppRoutingModule } from './app-routing.module';
import { NgDragDropModule } from 'ng-drag-drop';

import { AppComponent } from './app.component';
import { ScrabbleGameComponent } from './scrabble/component/scrabble-game/scrabble-game.component';
import { ScrabbleBoardComponent } from './scrabble/component/scrabble-board/scrabble-board.component';
import { ScrabblePlayerComponent } from './scrabble/component/scrabble-player/scrabble-player.component';
import { ScrabbleTileComponent } from './scrabble/component/scrabble-tile/scrabble-tile.component';
import { ScrabbleScoreComponent } from './scrabble/component/scrabble-score/scrabble-score.component';
import { ScrabbleToolbarComponent } from './scrabble/component/scrabble-toolbar/scrabble-toolbar.component';
import { ScrabbleGameListComponent } from './scrabble/component/scrabble-game-list/scrabble-game-list.component';
import { ScrabbleService } from './scrabble/service/scrabble.service';
import { ApiService } from './service/api.service';
import { HttpClientModule } from '@angular/common/http';
import { EnvironmentService } from './service/environment.service';
import { WordFinderComponent } from './words/component/word-finder/word-finder.component';
import { WordSelectorComponent } from './words/component/word-selector/word-selector.component';
import { WordCheckerComponent } from './words/component/word-checker/word-checker.component';
import { WordService } from './words/service/word.service';

@NgModule({
  declarations: [
    AppComponent,
    ScrabbleGameComponent,
    ScrabbleBoardComponent,
    ScrabblePlayerComponent,
    ScrabbleTileComponent,
    ScrabbleScoreComponent,
    ScrabbleToolbarComponent,
    ScrabbleGameListComponent,
    WordSelectorComponent,
    WordFinderComponent,
    WordCheckerComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    LibraryModule,
    AppRoutingModule,
    NgDragDropModule.forRoot()
  ],
  providers: [
    ApiService,
    ScrabbleService,
    EnvironmentService,
    WordService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
