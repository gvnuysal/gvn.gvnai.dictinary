import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { WordDto, WordSummaryDto } from '../models/word.model';
import { PagedResult } from '../models/paged-result.model';
import { CreateWordRequest, CreateWordWithTranslationRequest, UpdateWordRequest } from '../models/commands.model';
import { DICTIONARY_API_CONFIG } from './api-config';

@Injectable({ providedIn: 'root' })
export class WordService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(DICTIONARY_API_CONFIG);

  private get baseUrl(): string {
    return `${this.config.baseUrl}/api/words`;
  }

  getWords(params?: {
    languageId?: string;
    pageNumber?: number;
    pageSize?: number;
  }): Observable<PagedResult<WordSummaryDto>> {
    let httpParams = new HttpParams();
    if (params?.languageId) httpParams = httpParams.set('languageId', params.languageId);
    if (params?.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
    if (params?.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());

    return this.http.get<PagedResult<WordSummaryDto>>(this.baseUrl, { params: httpParams });
  }

  getWordById(id: string): Observable<WordDto> {
    return this.http.get<WordDto>(`${this.baseUrl}/${id}`);
  }

  searchWords(params: {
    q?: string;
    languageId?: string;
    partOfSpeechId?: string;
    domainId?: string;
    registerId?: string;
    pageNumber?: number;
    pageSize?: number;
  }): Observable<PagedResult<WordSummaryDto>> {
    let httpParams = new HttpParams();
    if (params.q) httpParams = httpParams.set('q', params.q);
    if (params.languageId) httpParams = httpParams.set('languageId', params.languageId);
    if (params.partOfSpeechId) httpParams = httpParams.set('partOfSpeechId', params.partOfSpeechId);
    if (params.domainId) httpParams = httpParams.set('domainId', params.domainId);
    if (params.registerId) httpParams = httpParams.set('registerId', params.registerId);
    if (params.pageNumber) httpParams = httpParams.set('pageNumber', params.pageNumber.toString());
    if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());

    return this.http.get<PagedResult<WordSummaryDto>>(`${this.baseUrl}/search`, { params: httpParams });
  }

  createWord(request: CreateWordRequest): Observable<string> {
    return this.http.post<string>(this.baseUrl, request);
  }

  createWordWithTranslation(request: CreateWordWithTranslationRequest): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}/with-translation`, request);
  }

  updateWord(id: string, request: UpdateWordRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  deleteWord(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  enrichWord(id: string, targetLanguageCode?: string): Observable<void> {
    let httpParams = new HttpParams();
    if (targetLanguageCode) httpParams = httpParams.set('targetLanguageCode', targetLanguageCode);

    return this.http.post<void>(`${this.baseUrl}/${id}/enrich`, null, { params: httpParams });
  }
}
