import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class SpeechService {
  readonly isListening = signal(false);
  readonly isSupported: boolean;

  private recognition: any = null;
  private currentResolve: ((text: string) => void) | null = null;

  constructor() {
    const SpeechRecognition = (window as any).SpeechRecognition || (window as any).webkitSpeechRecognition;
    this.isSupported = !!SpeechRecognition;

    if (this.isSupported) {
      this.recognition = new SpeechRecognition();
      this.recognition.continuous = false;
      this.recognition.interimResults = false;

      this.recognition.onresult = (event: any) => {
        const text = event.results[0][0].transcript;
        this.isListening.set(false);
        if (this.currentResolve) {
          this.currentResolve(text);
          this.currentResolve = null;
        }
      };

      this.recognition.onerror = () => {
        this.isListening.set(false);
        this.currentResolve = null;
      };

      this.recognition.onend = () => {
        this.isListening.set(false);
      };
    }
  }

  listen(lang: string = 'en-US'): Promise<string> {
    if (!this.isSupported) return Promise.reject('Speech recognition not supported');

    return new Promise((resolve, reject) => {
      this.currentResolve = resolve;
      this.recognition.lang = lang;
      this.isListening.set(true);

      try {
        this.recognition.start();
      } catch {
        this.isListening.set(false);
        reject('Failed to start recognition');
      }
    });
  }

  stop(): void {
    if (this.recognition && this.isListening()) {
      this.recognition.stop();
      this.isListening.set(false);
    }
  }
}
