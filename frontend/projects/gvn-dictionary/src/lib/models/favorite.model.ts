export interface FavoriteWordDto {
  wordId: string;
  lemma: string;
  language: { id: string; code: string; name: string };
  partOfSpeech: { id: string; code: string; name: string };
  firstTranslation: string | null;
  addedAt: string;
}
