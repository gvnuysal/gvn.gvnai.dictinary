export interface QuizQuestionDto {
  sessionId: string;
  wordId: string;
  lemma: string;
  definition: string;
  options: QuizOptionDto[];
  correctOptionId: string;
  questionNumber: number;
  totalScore: number;
  correctCount: number;
  wrongCount: number;
}

export interface QuizOptionDto {
  id: string;
  text: string;
}

export interface QuizAnswerResultDto {
  isCorrect: boolean;
  pointsEarned: number;
  totalScore: number;
  correctOptionId: string;
  selectedOptionId: string | null;
}

export interface SubmitAnswerRequest {
  wordId: string;
  selectedOptionId: string | null;
  correctOptionId: string;
  responseTimeMs: number;
}

export interface QuizResultDto {
  sessionId: string;
  totalScore: number;
  correctCount: number;
  wrongCount: number;
  totalQuestions: number;
  accuracy: number;
  answers: QuizAnswerDetailDto[];
}

export interface QuizAnswerDetailDto {
  wordLemma: string;
  correctTranslation: string;
  selectedTranslation: string | null;
  isCorrect: boolean;
  pointsEarned: number;
  responseTimeMs: number;
}

export interface LeaderboardEntryDto {
  userFullName: string;
  totalScore: number;
  gamesPlayed: number;
  averageAccuracy: number;
}
