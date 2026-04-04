import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DICTIONARY_API_CONFIG } from './api-config';
import {
  QuizQuestionDto,
  QuizAnswerResultDto,
  SubmitAnswerRequest,
  QuizResultDto,
  LeaderboardEntryDto
} from '../models/quiz.model';

@Injectable({ providedIn: 'root' })
export class QuizService {
  private readonly http = inject(HttpClient);
  private readonly config = inject(DICTIONARY_API_CONFIG);

  private get baseUrl() { return `${this.config.baseUrl}/api/quiz`; }

  startQuiz(): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}/start`, {});
  }

  getNextQuestion(sessionId: string): Observable<QuizQuestionDto> {
    return this.http.get<QuizQuestionDto>(`${this.baseUrl}/${sessionId}/next`);
  }

  submitAnswer(sessionId: string, request: SubmitAnswerRequest): Observable<QuizAnswerResultDto> {
    return this.http.post<QuizAnswerResultDto>(`${this.baseUrl}/${sessionId}/answer`, request);
  }

  completeQuiz(sessionId: string): Observable<QuizResultDto> {
    return this.http.post<QuizResultDto>(`${this.baseUrl}/${sessionId}/complete`, {});
  }

  getLeaderboard(top: number = 10): Observable<LeaderboardEntryDto[]> {
    return this.http.get<LeaderboardEntryDto[]>(`${this.baseUrl}/leaderboard`, { params: { top: top.toString() } });
  }
}
