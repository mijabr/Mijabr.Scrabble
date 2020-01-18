import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable()
export class EnvironmentService {

  constructor() { }

  isProduction(): boolean {
    return environment.production;
  }
}
