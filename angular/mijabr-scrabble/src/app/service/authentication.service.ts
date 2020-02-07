import { Injectable } from '@angular/core';

import {
  OidcSecurityService,
  OpenIdConfiguration,
  AuthWellKnownEndpoints,
  AuthorizationState
} from 'angular-auth-oidc-client';

import { Observable } from 'rxjs';
import { ActivatedRoute } from '@angular/router';
import { map,  take, switchMap, tap } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  public authenticated: Observable<boolean>;
  public authState: AuthorizationState;

  constructor(
    private oidcSecurityService: OidcSecurityService,
    private route: ActivatedRoute
  ) { }

  private href = 'scrabble';

  private thisBaseUrl(): string {
    return location.protocol + '//' + location.host + '/' + this.href;
  }

  private idBaseUrl(): string {
    if (location.host === 'localtest.me') {
      return location.protocol + '//id.' + location.host;
    } else {
      return location.protocol + '//id-' + location.host;
    }
  }

  public get loggedIn(): Observable<boolean> {
    return this.oidcSecurityService.getIsAuthorized();
  }

  public get authorizationInProgress(): Observable<boolean> {
    return this.route.queryParamMap.pipe(
      map(p => p.has('code'))
    );
  }

  public logIn() {
    this.oidcSecurityService.authorize();
  }

  public logOut() {
    this.oidcSecurityService.logoff();
  }

  public getUserData(): Observable<any> {
    return this.oidcSecurityService.getUserData();
  }

  public initialise() {

    const config = this.getOpenIdConfiguration();
    const authWellKnownEndpoints = this.getAuthWellKnownEndpoints();

    this.oidcSecurityService.setupModule(config, authWellKnownEndpoints);

    this.oidcSecurityService.onCheckSessionChanged.subscribe(c => this.logOut());
  }

  public completeAuthentication(): Observable<AuthorizationState> {
    return this.whenModuleIsSetup().pipe(
      tap(() => this.oidcSecurityService.authorizedCallbackWithCode(window.location.toString())),
      switchMap(() => this.oidcSecurityService.onAuthorizationResult),
      take(1),
      map(r => r.authorizationState)
    );
  }

  private whenModuleIsSetup(): Observable<void> {
    return new Observable(observer => {
      if (this.oidcSecurityService.moduleSetup) {
        observer.next(void 0);
      } else {
        this.oidcSecurityService.onModuleSetup.subscribe(() => {
          observer.next(void 0);
        });
      }
    });
  }

  private getOpenIdConfiguration(): OpenIdConfiguration {
    return {
      stsServer: this.idBaseUrl(),
      redirect_url: this.thisBaseUrl() + '/redirect',
      client_id: 'scrabble-client',
      response_type: 'code',
      scope: 'openid profile scrabble',
      post_logout_redirect_uri: this.thisBaseUrl(),
      post_login_route: 'scrabble/game',
      log_console_warning_active: true,
  //  log_console_debug_active: true,
      max_id_token_iat_offset_allowed_in_seconds: 30,
      start_checksession: true,
      iss_validation_off: true
    };
  }

  private getAuthWellKnownEndpoints(): AuthWellKnownEndpoints {
    return {
      jwks_uri: this.idBaseUrl() + '/.well-known/openid-configuration/jwks',
      issuer: "http://identity",
      authorization_endpoint: this.idBaseUrl() + '/connect/authorize',
      token_endpoint: this.idBaseUrl() + '/connect/token',
      userinfo_endpoint: this.idBaseUrl() + '/connect/userinfo',
      end_session_endpoint: this.idBaseUrl() + '/connect/endsession',
      check_session_iframe: this.idBaseUrl() + '/connect/checksession',
      revocation_endpoint: this.idBaseUrl() + '/connect/revocation',
      introspection_endpoint: this.idBaseUrl() + '/connect/introspect',
    };
  }
}
