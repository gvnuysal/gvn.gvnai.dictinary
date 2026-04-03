import { InjectionToken } from '@angular/core';

export interface DictionaryApiConfig {
  baseUrl: string;
}

export const DICTIONARY_API_CONFIG = new InjectionToken<DictionaryApiConfig>('DictionaryApiConfig');
