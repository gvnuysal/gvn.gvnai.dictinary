export interface ProfileDto {
  id: string;
  email: string;
  fullName: string;
  role: string;
  memberSince: string;
  stats: ProfileStatsDto;
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

export interface UpdateProfileRequest {
  fullName: string;
}
