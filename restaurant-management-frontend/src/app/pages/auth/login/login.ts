import { Component } from '@angular/core';
import { AuthLayout } from '../auth-layout/auth-layout';
import { AuthCard } from '../../../shared/components/auth-card/auth-card';
import { LoginForm } from '../../../features/auth/login-form/login-form';

@Component({
  selector: 'app-login',
  imports: [AuthLayout, AuthCard, LoginForm],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {

}
