import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LibraryModule } from './library/module/library.module';
import { AppRoutingModule } from './app-routing.module';
import { NgDragDropModule } from 'ng-drag-drop';
import { AuthModule, OidcSecurityService, OidcConfigService } from 'angular-auth-oidc-client';

import { AppComponent } from './app.component';
import { ScrabbleGameComponent } from './component/scrabble-game/scrabble-game.component';
import { ScrabbleBoardComponent } from './component/scrabble-board/scrabble-board.component';
import { ScrabblePlayerComponent } from './component/scrabble-player/scrabble-player.component';
import { ScrabbleTileComponent } from './component/scrabble-tile/scrabble-tile.component';
import { ScrabbleScoreComponent } from './component/scrabble-score/scrabble-score.component';
import { ScrabbleToolbarComponent } from './component/scrabble-toolbar/scrabble-toolbar.component';
import { ScrabbleGameListComponent } from './component/scrabble-game-list/scrabble-game-list.component';
import { ScrabbleService } from './service/scrabble.service';
import { ApiService } from './service/api.service';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { WordFinderComponent } from './component/word-finder/word-finder.component';
import { WordSelectorComponent } from './component/word-selector/word-selector.component';
import { WordCheckerComponent } from './component/word-checker/word-checker.component';
import { WordService } from './service/word.service';
import { AuthenticationService } from './service/authentication.service';
import { AuthenticationInterceptor } from './service/authentication.interceptor';
import { RedirectComponent } from './component/redirect/redirect.component';
import { ToolbarComponent } from './component/toolbar/toolbar.component';
import { UnauthorizedComponent } from './component/unauthorized/unauthorized.component';

@NgModule({
  declarations: [
    AppComponent,
    RedirectComponent,
    UnauthorizedComponent,
    ToolbarComponent,
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
    AuthModule.forRoot(),
    LibraryModule,
    AppRoutingModule,
    NgDragDropModule.forRoot()
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    },
    OidcSecurityService,
    OidcConfigService,
    AuthenticationService,
    ApiService,
    ScrabbleService,
    WordService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { 
  constructor(
    private authenticationService: AuthenticationService
  ) {
    this.authenticationService.initialise();
  }
}
