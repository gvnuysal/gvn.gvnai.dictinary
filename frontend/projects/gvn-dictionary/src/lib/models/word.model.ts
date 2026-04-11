import { LanguageSummaryDto, PartOfSpeechSummaryDto } from './lookup.model';

export interface WordDto {
  id: string;
  lemma: string;
  language: LanguageSummaryDto;
  partOfSpeech: PartOfSpeechSummaryDto;
  status: WordStatus;
  frequencyRank: number | null;
  difficultyLevel: DifficultyLevel | null;
  isCompound: boolean;
  isIdiom: boolean;
  isProperNoun: boolean;
  synonyms: string | null;
  antonyms: string | null;
  senses: SenseDto[];
  pronunciations: PronunciationDto[];
  etymologies: EtymologyDto[];
  createdAt: string;
  updatedAt: string | null;
}

export interface WordSummaryDto {
  id: string;
  lemma: string;
  language: LanguageSummaryDto;
  partOfSpeech: PartOfSpeechSummaryDto;
  status: WordStatus;
  createdAt: string;
  firstDefinition: string | null;
  firstTranslation: string | null;
}

export interface SenseDto {
  id: string;
  senseNumber: number;
  definition: string;
  definitionShort: string | null;
  registerCode: string | null;
  registerName: string | null;
  domainCode: string | null;
  domainName: string | null;
  frequencyRank: number | null;
  difficultyLevel: DifficultyLevel | null;
  translations: TranslationDto[];
  examples: ExampleDto[];
}

export interface TranslationDto {
  id: string;
  targetLanguage: LanguageSummaryDto;
  translationText: string;
  partOfSpeech: PartOfSpeechSummaryDto | null;
  registerCode: string | null;
  equivalenceType: EquivalenceType;
  confidenceScore: number;
}

export interface ExampleDto {
  id: string;
  sourceText: string;
  targetText: string | null;
  source: ExampleSource;
}

export interface PronunciationDto {
  id: string;
  ipaTranscription: string;
  variant: string | null;
  isStandard: boolean;
}

export interface EtymologyDto {
  id: string;
  originLanguage: LanguageSummaryDto | null;
  text: string;
}

export enum WordStatus { Pending = 0, Enriched = 1, Failed = 2, Approved = 3, Archived = 4 }
export enum EquivalenceType { Exact = 0, Near = 1, Loose = 2, Gap = 3 }
export enum DifficultyLevel { A1 = 1, A2 = 2, B1 = 3, B2 = 4, C1 = 5, C2 = 6 }
export enum ExampleSource { Corpus = 0, Literature = 1, Ai = 2, Manual = 3 }
