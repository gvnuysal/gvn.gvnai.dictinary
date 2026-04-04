import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProfileDto, UpdateProfileRequest, UpdateApiSettingsRequest } from '../models/profile.model';
import { DICTIONARY_API_CONFIG } from './api-config';

@Injectable({ providedIn: 'root' })
export class ProfileService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(DICTIONARY_API_CONFIG);

  private get baseUrl(): string {
    return `${this.config.baseUrl}/api/profile`;
  }

  getProfile(): Observable<ProfileDto> {
    return this.http.get<ProfileDto>(this.baseUrl);
  }

  updateProfile(request: UpdateProfileRequest): Observable<void> {
    return this.http.put<void>(this.baseUrl, request);
  }

  updateApiSettings(request: UpdateApiSettingsRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/api-settings`, request);
  }
}
