import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { DICTIONARY_API_CONFIG } from './api-config';

@Injectable({ providedIn: 'root' })
export class TranslateService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(DICTIONARY_API_CONFIG);

  translate(text: string, source: string = 'en', target: string = 'tr'): Observable<string> {
    const params = new HttpParams()
      .set('text', text)
      .set('source', source)
      .set('target', target);

    return this.http
      .get<{ translatedText: string }>(`${this.config.baseUrl}/api/translate`, { params })
      .pipe(map(r => r.translatedText));
  }

  define(word: string): Observable<string> {
    const params = new HttpParams().set('word', word);
    return this.http
      .get<{ definition: string }>(`${this.config.baseUrl}/api/translate/define`, { params })
      .pipe(map(r => r.definition));
  }

  aiTranslate(word: string, target: string = 'tr'): Observable<string> {
    const params = new HttpParams().set('word', word).set('target', target);
    return this.http
      .get<{ translatedText: string }>(`${this.config.baseUrl}/api/translate/ai-translate`, { params })
      .pipe(map(r => r.translatedText));
  }

  detectPos(word: string): Observable<string> {
    const params = new HttpParams().set('word', word);
    return this.http
      .get<{ posCode: string }>(`${this.config.baseUrl}/api/translate/detect-pos`, { params })
      .pipe(map(r => r.posCode));
  }
}
