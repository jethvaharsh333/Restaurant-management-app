import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../service/auth-service';
import { ToastService } from '../../../shared/components/toast/toast-service/toast-service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-sign-up-form',
  imports: [ReactiveFormsModule],
  templateUrl: './sign-up-form.html',
  styleUrl: './sign-up-form.scss'
})

export class SignUpForm {
  private _authService = inject(AuthService);
  private _router = inject(Router);
  private _toast = inject(ToastService);

  registerForm: FormGroup = new FormGroup({
    username: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
  });

  isLoading = false;
  errorMessage = "";

  onRegister(){
    this.isLoading = true;
    this.errorMessage = "";

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    this._authService.registerService(this.registerForm.value)
      .pipe(
        finalize(() => this.isLoading = false)
      )
      .subscribe({
        next: (res) => {
          this._toast.success(res.message);
          this._router.navigate(['/auth','verify-email']);
          console.log(res);
        },
        error: (err) => {
          this.errorMessage = err.error.message;
          this._toast.error(err.error.message ?? "Something went wrong.");
          console.log(err);
        }
      })
  }

  get username() {
    return this.registerForm.get('username')!;
  }

  get email() {
    return this.registerForm.get('email')!;
  }

  get password() {
    return this.registerForm.get('password')!;
  }

}
