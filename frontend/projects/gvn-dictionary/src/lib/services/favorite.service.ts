import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { FavoriteWordDto } from '../models/favorite.model';
import { DICTIONARY_API_CONFIG } from './api-config';

@Injectable({ providedIn: 'root' })
export class FavoriteService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(DICTIONARY_API_CONFIG);

  private get baseUrl(): string {
    return `${this.config.baseUrl}/api/favorites`;
  }

  getFavorites(): Observable<FavoriteWordDto[]> {
    return this.http.get<FavoriteWordDto[]>(this.baseUrl);
  }

  addFavorite(wordId: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${wordId}`, {});
  }

  removeFavorite(wordId: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${wordId}`);
  }

  checkFavorite(wordId: string): Observable<{ isFavorite: boolean }> {
    return this.http.get<{ isFavorite: boolean }>(`${this.baseUrl}/${wordId}/check`);
  }
}
