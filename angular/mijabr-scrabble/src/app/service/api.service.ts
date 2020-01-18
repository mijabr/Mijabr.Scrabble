import { Injectable } from '@angular/core';
import { EnvironmentService } from './environment.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class ApiService {

  constructor(
    private http: HttpClient,
    private environment: EnvironmentService
  ) { }

  post(url: string, body?: any): Observable<any> {
    if (this.environment.isProduction()) {
      const headers = new HttpHeaders();
      // this.createAuthorizationHeader(headers);
      return this.http.post(url, body, {
        headers: headers
      });
    } else {
      return this.mockApi(url);
    }
  }

  private mockApi(url: string) {
    url = `http://localhost:3004/${url}`;
    return this.http.get(url, {});
  }

  // createAuthorizationHeader(headers: HttpHeaders) {
  //   const token = this.userService.getUser().token;
  //   headers.append('Authorization', 'Bearer ' + token);
  // }
}
