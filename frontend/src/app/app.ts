import { Component, inject } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService, ThemeService } from 'gvn-dictionary';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  private authService = inject(AuthService);
  readonly themeService = inject(ThemeService);

  get isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  get userEmail(): string | null {
    return localStorage.getItem('dict_user_email');
  }

  get themeIcon(): string {
    const mode = this.themeService.mode();
    if (mode === 'light') return '\u2600'; // ☀
    if (mode === 'dark') return '\uD83C\uDF19'; // 🌙
    return '\uD83D\uDD04'; // 🔄 auto
  }

  get themeTooltip(): string {
    const mode = this.themeService.mode();
    if (mode === 'light') return 'Acik Tema';
    if (mode === 'dark') return 'Koyu Tema';
    return 'Otomatik (18:00-06:00 Koyu)';
  }

  toggleTheme(): void {
    this.themeService.toggle();
  }

  logout(): void {
    this.authService.logout();
    window.location.href = '/';
  }
}
