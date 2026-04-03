import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LookupsDto } from '../models/lookup.model';
import { DICTIONARY_API_CONFIG } from './api-config';

@Injectable({ providedIn: 'root' })
export class LookupService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(DICTIONARY_API_CONFIG);

  getLookups(): Observable<LookupsDto> {
    return this.http.get<LookupsDto>(`${this.config.baseUrl}/api/lookups`);
  }
}
