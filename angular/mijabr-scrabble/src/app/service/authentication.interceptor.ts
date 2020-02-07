import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { OidcSecurityService } from 'angular-auth-oidc-client';

@Injectable({
    providedIn: 'root'
})
export class AuthenticationInterceptor implements HttpInterceptor {

    constructor(
        private oidcSecurityService: OidcSecurityService,
    ) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        const token = this.oidcSecurityService.getToken();
        
        if (token !== '') {
            const tokenValue = 'Bearer ' + token;
            request = request.clone({ setHeaders: { Authorization: tokenValue } });
        }

        return next.handle(request).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status === 401) {
                    this.oidcSecurityService.authorize();
                }
                return next.handle(request);
            }));

    }
}
