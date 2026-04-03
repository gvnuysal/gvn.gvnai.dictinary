import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AddExampleRequest } from '../models/commands.model';
import { DICTIONARY_API_CONFIG } from './api-config';

@Injectable({ providedIn: 'root' })
export class ExampleService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(DICTIONARY_API_CONFIG);

  addExample(wordId: string, senseId: string, request: AddExampleRequest): Observable<string> {
    return this.http.post<string>(
      `${this.config.baseUrl}/api/words/${wordId}/senses/${senseId}/examples`,
      request
    );
  }

  removeExample(wordId: string, senseId: string, exampleId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.config.baseUrl}/api/words/${wordId}/senses/${senseId}/examples/${exampleId}`
    );
  }
}
