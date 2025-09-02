import { inject, Injectable, PLATFORM_ID } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { ToastService } from '../../../shared/components/toast/toast-service/toast-service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../../core/models/api-response';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly _BACKEND_URL = environment.BACKEND_URL + "/auth";
  private readonly _ROLE_KEY = environment.ROLE_KEY;

  private _http = inject(HttpClient);
  private _router = inject(Router);
  private _platformId = inject(PLATFORM_ID);

  registerService(data: RegisterRequest): Observable<ApiResponse<null>>{
    return this._http.post<ApiResponse<null>>(this._BACKEND_URL+"/register-customer", data);
  }
}

interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}
