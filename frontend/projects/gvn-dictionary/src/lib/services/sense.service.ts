import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddSenseRequest, UpdateSenseRequest } from '../models/commands.model';
import { DICTIONARY_API_CONFIG } from './api-config';

@Injectable({ providedIn: 'root' })
export class SenseService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(DICTIONARY_API_CONFIG);

  addSense(wordId: string, request: AddSenseRequest): Observable<string> {
    return this.http.post<string>(
      `${this.config.baseUrl}/api/words/${wordId}/senses`,
      request
    );
  }

  updateSense(wordId: string, senseId: string, request: UpdateSenseRequest): Observable<void> {
    return this.http.put<void>(
      `${this.config.baseUrl}/api/words/${wordId}/senses/${senseId}`,
      request
    );
  }

  removeSense(wordId: string, senseId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.config.baseUrl}/api/words/${wordId}/senses/${senseId}`
    );
  }
}
