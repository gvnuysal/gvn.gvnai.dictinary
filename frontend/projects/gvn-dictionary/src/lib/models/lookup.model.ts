export interface LanguageSummaryDto {
  id: string;
  code: string;
  name: string;
}

export interface LanguageDto {
  id: string;
  code: string;
  name: string;
  nativeName: string;
  direction: TextDirection;
}

export interface PartOfSpeechSummaryDto {
  id: string;
  code: string;
  name: string;
}

export interface PartOfSpeechDto {
  id: string;
  code: string;
  name: string;
  abbreviation: string;
}

export interface RegisterDto {
  id: string;
  code: string;
  name: string;
}

export interface SubjectDomainDto {
  id: string;
  code: string;
  name: string;
}

export interface LookupsDto {
  languages: LanguageDto[];
  partsOfSpeech: PartOfSpeechDto[];
  registers: RegisterDto[];
  domains: SubjectDomainDto[];
}

export enum TextDirection { LTR = 0, RTL = 1 }
