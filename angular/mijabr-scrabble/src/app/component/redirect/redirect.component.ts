import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/app/service/authentication.service';

@Component({
  selector: 'app-redirect',
  templateUrl: './redirect.component.html',
  styleUrls: ['./redirect.component.css']
})
export class RedirectComponent implements OnInit {

  public authenticating = true;
  public message = '';

  constructor(
    private authenticationService: AuthenticationService,
    private router: Router) { }

  ngOnInit() {
    this.authenticationService.completeAuthentication().subscribe(authState => {
      this.authenticating = false;
      if (authState === 'forbidden') {
        this.message = 'Forbidden';
      } else if (authState === 'unauthorized') {
        this.message = 'Unauthorized';
      } else if (authState === 'authorized') {
        this.router.navigate(['/']);
      }
    });
  }
}
