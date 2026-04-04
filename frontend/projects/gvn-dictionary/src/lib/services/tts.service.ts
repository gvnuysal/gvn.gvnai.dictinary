import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class TtsService {
  readonly isSpeaking = signal(false);
  readonly isSupported = typeof window !== 'undefined' && 'speechSynthesis' in window;

  speak(text: string, lang: string = 'en-US'): void {
    if (!this.isSupported) return;

    window.speechSynthesis.cancel();

    const utterance = new SpeechSynthesisUtterance(text);
    utterance.lang = lang;
    utterance.rate = 0.85;
    utterance.pitch = 1;

    // Uygun voice bul
    const voices = window.speechSynthesis.getVoices();
    const match = voices.find(v => v.lang.startsWith(lang.split('-')[0]));
    if (match) utterance.voice = match;

    utterance.onstart = () => this.isSpeaking.set(true);
    utterance.onend = () => this.isSpeaking.set(false);
    utterance.onerror = () => this.isSpeaking.set(false);

    window.speechSynthesis.speak(utterance);
  }

  stop(): void {
    if (this.isSupported) {
      window.speechSynthesis.cancel();
      this.isSpeaking.set(false);
    }
  }
}
