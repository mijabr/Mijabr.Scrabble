import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/service/authentication.service';

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html',
  styleUrls: ['./unauthorized.component.css']
})
export class UnauthorizedComponent implements OnInit {

  public message = '';

  constructor(private authenticationService: AuthenticationService) { }

  ngOnInit() {
    this.authenticationService.logIn();
  }
}
