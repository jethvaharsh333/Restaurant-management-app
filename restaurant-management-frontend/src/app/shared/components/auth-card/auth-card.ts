import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-auth-card',
  imports: [],
  templateUrl: './auth-card.html',
  styleUrl: './auth-card.scss'
})
export class AuthCard {
  @Input() title = '';
  @Input() label = '';
  @Input() backButtonLabel = '';
  @Input() backButtonName = '';
  @Input() backButtonLink = '';
  @Input() isSSO = false;
}