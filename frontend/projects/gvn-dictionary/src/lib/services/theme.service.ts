import { Injectable, signal, effect } from '@angular/core';

export type ThemeMode = 'light' | 'dark' | 'auto';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  private readonly STORAGE_KEY = 'dict_theme_mode';
  private autoTimer: ReturnType<typeof setInterval> | null = null;

  readonly mode = signal<ThemeMode>(this.loadMode());
  readonly activeTheme = signal<'light' | 'dark'>(this.resolveTheme(this.loadMode()));

  constructor() {
    effect(() => {
      const m = this.mode();
      localStorage.setItem(this.STORAGE_KEY, m);
      this.activeTheme.set(this.resolveTheme(m));
      this.applyTheme(this.activeTheme());
      this.manageAutoTimer(m);
    });

    // Initial apply
    this.applyTheme(this.activeTheme());
    this.manageAutoTimer(this.mode());
  }

  setMode(mode: ThemeMode): void {
    this.mode.set(mode);
  }

  toggle(): void {
    const current = this.mode();
    if (current === 'auto') {
      this.mode.set('light');
    } else if (current === 'light') {
      this.mode.set('dark');
    } else {
      this.mode.set('auto');
    }
  }

  private loadMode(): ThemeMode {
    const stored = localStorage.getItem(this.STORAGE_KEY);
    if (stored === 'light' || stored === 'dark' || stored === 'auto') return stored;
    return 'auto';
  }

  private resolveTheme(mode: ThemeMode): 'light' | 'dark' {
    if (mode === 'light') return 'light';
    if (mode === 'dark') return 'dark';
    // Auto: 18:00-06:00 dark, otherwise light
    const hour = new Date().getHours();
    return (hour >= 18 || hour < 6) ? 'dark' : 'light';
  }

  private applyTheme(theme: 'light' | 'dark'): void {
    document.documentElement.setAttribute('data-theme', theme);
  }

  private manageAutoTimer(mode: ThemeMode): void {
    if (this.autoTimer) {
      clearInterval(this.autoTimer);
      this.autoTimer = null;
    }
    if (mode === 'auto') {
      // Check every minute if theme should change
      this.autoTimer = setInterval(() => {
        const newTheme = this.resolveTheme('auto');
        if (newTheme !== this.activeTheme()) {
          this.activeTheme.set(newTheme);
          this.applyTheme(newTheme);
        }
      }, 60_000);
    }
  }
}
