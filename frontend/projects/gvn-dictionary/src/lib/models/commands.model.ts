export interface CreateWordRequest {
  lemma: string;
  languageId: string;
  partOfSpeechId: string;
}

export interface CreateWordWithTranslationRequest {
  lemma: string;
  partOfSpeechId: string;
  definition: string;
  translationText: string;
  definitionShort?: string | null;
  registerId?: string | null;
  domainId?: string | null;
  synonyms?: string | null;
  antonyms?: string | null;
}

export interface UpdateWordRequest {
  id: string;
  lemma: string;
  languageId: string;
  partOfSpeechId: string;
  frequencyRank: number | null;
  difficultyLevel: number | null;
  isCompound: boolean;
  isIdiom: boolean;
  isProperNoun: boolean;
}

export interface AddSenseRequest {
  definition: string;
  definitionShort: string | null;
  registerId: string | null;
  domainId: string | null;
  frequencyRank: number | null;
  difficultyLevel: number | null;
}

export interface UpdateSenseRequest extends AddSenseRequest {}

export interface AddTranslationRequest {
  targetLanguageId: string;
  translationText: string;
  partOfSpeechId: string | null;
  registerId: string | null;
  equivalenceType: number;
  confidenceScore: number;
}

export interface AddExampleRequest {
  sourceText: string;
  targetText: string | null;
  translationId: string | null;
  source: number;
}
