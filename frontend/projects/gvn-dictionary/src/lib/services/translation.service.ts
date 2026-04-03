import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddTranslationRequest } from '../models/commands.model';
import { DICTIONARY_API_CONFIG } from './api-config';

@Injectable({ providedIn: 'root' })
export class TranslationService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(DICTIONARY_API_CONFIG);

  addTranslation(wordId: string, senseId: string, request: AddTranslationRequest): Observable<string> {
    return this.http.post<string>(
      `${this.config.baseUrl}/api/words/${wordId}/senses/${senseId}/translations`,
      request
    );
  }

  removeTranslation(wordId: string, senseId: string, translationId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.config.baseUrl}/api/words/${wordId}/senses/${senseId}/translations/${translationId}`
    );
  }
}
