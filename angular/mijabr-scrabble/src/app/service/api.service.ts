import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class ApiService {

  constructor(
    private http: HttpClient
  ) { }

  post(url: string, body?: any): Observable<any> {
    if (environment.production) {
      return this.http.post(url, body);
    } else {
      return this.mockApi(url);
    }
  }

  private mockApi(url: string) {
    url = `http://localhost:3004/${url}`;
    return this.http.get(url, {});
  }
}
