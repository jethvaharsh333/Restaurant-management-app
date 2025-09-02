import { Routes } from "@angular/router";
import { Login } from "./login/login";
import { AuthLayout } from "./auth-layout/auth-layout";
import { ForgotPassword } from "./forgot-password/forgot-password";
import { VerifyEmail } from "./verify-email/verify-email";
import { ResetPassword } from "./reset-password/reset-password";
import { SignUp } from "./sign-up/sign-up";

export const AUTH_ROUTES: Routes = [
    {
    path: '',
    children: [
      {
        path: 'login',
        component: Login
      },
      {
        path: 'sign-up',
        component: SignUp
      },
      {
        path: 'verify-email',
        component: VerifyEmail
      },
      {
        path: 'forgot-password',
        component: ForgotPassword
      },
      {
        path: 'send-email',
        component: ForgotPassword
      },
      {
        path: 'reset-password',
        component: ResetPassword
      },
      { path: '', redirectTo: 'login', pathMatch: 'full' }
    ]
  }
]