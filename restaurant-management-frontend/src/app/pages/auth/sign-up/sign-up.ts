import { Component } from '@angular/core';
import { SignUpForm } from '../../../features/auth/sign-up-form/sign-up-form';
import { AuthLayout } from '../auth-layout/auth-layout';
import { AuthCard } from '../../../shared/components/auth-card/auth-card';

@Component({
  selector: 'app-sign-up',
  imports: [SignUpForm, AuthLayout, AuthCard],
  templateUrl: './sign-up.html',
  styleUrl: './sign-up.scss'
})
export class SignUp {

}
