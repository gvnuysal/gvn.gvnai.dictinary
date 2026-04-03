import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest, LoginResponse, RegisterRequest } from '../models/auth.model';
import { DICTIONARY_API_CONFIG } from './api-config';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(DICTIONARY_API_CONFIG);

  private readonly TOKEN_KEY = 'dict_token';
  private readonly EXPIRES_KEY = 'dict_token_expires';

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.config.baseUrl}/api/auth/login`, request);
  }

  register(request: RegisterRequest): Observable<string> {
    return this.http.post<string>(`${this.config.baseUrl}/api/auth/register`, request);
  }

  logout(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.EXPIRES_KEY);
  }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    const token = this.getToken();
    const expiresAt = localStorage.getItem(this.EXPIRES_KEY);

    if (!token || !expiresAt) {
      return false;
    }

    return new Date(expiresAt) > new Date();
  }

  setToken(response: LoginResponse): void {
    localStorage.setItem(this.TOKEN_KEY, response.token);
    localStorage.setItem(this.EXPIRES_KEY, response.expiresAt);
  }
}
