export interface ProfileDto {
  id: string;
  email: string;
  fullName: string;
  role: string;
  memberSince: string;
  stats: ProfileStatsDto;
  apiSettings: ApiSettingsDto;
}

export interface ProfileStatsDto {
  totalGamesPlayed: number;
  totalScore: number;
  totalCorrect: number;
  totalWrong: number;
  totalQuestionsAnswered: number;
  averageAccuracy: number;
  favoriteWordsCount: number;
  bestGameScore: number;
}

export interface ApiSettingsDto {
  translateProvider: string;
  hasClaudeKey: boolean;
  hasGoogleKey: boolean;
  claudeApiKey: string | null;
  googleTranslateApiKey: string | null;
}

export interface UpdateProfileRequest {
  fullName: string;
}

export interface UpdateApiSettingsRequest {
  translateProvider: string;
  claudeApiKey: string | null;
  googleTranslateApiKey: string | null;
}
