import { Component, ChangeDetectionStrategy, signal, inject, OnInit } from '@angular/core';
import { DatePipe, UpperCasePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { WordService } from '../../../../services/word.service';
import { AuthService } from '../../../../services/auth.service';
import { FavoriteService } from '../../../../services/favorite.service';
import { WordDto, WordStatus, EquivalenceType, ExampleSource, DifficultyLevel } from '../../../../models/word.model';
import { SenseFormComponent } from '../sense-form/sense-form.component';
import { TranslationFormComponent } from '../translation-form/translation-form.component';
import { ExampleFormComponent } from '../example-form/example-form.component';

@Component({
  selector: 'dict-word-detail',
  standalone: true,
  imports: [DatePipe, UpperCasePipe, SenseFormComponent, TranslationFormComponent, ExampleFormComponent],
  templateUrl: './word-detail.component.html',
  styleUrl: './word-detail.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WordDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly wordService = inject(WordService);
  private readonly authService = inject(AuthService);
  private readonly favoriteService = inject(FavoriteService);

  readonly word = signal<WordDto | null>(null);
  readonly isFavorite = signal(false);
  readonly loading = signal(true);
  readonly isAuthenticated = signal(false);
  readonly enriching = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly showSenseForm = signal(false);
  readonly showTranslationForm = signal<string | null>(null);
  readonly showExampleForm = signal<string | null>(null);

  ngOnInit(): void {
    this.isAuthenticated.set(this.authService.isAuthenticated());
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadWord(id);
    }
  }

  private loadWord(id: string): void {
    this.loading.set(true);
    this.wordService.getWordById(id).subscribe({
      next: (word) => {
        this.word.set(word);
        this.loading.set(false);
        if (this.authService.isAuthenticated()) {
          this.favoriteService.checkFavorite(word.id).subscribe({
            next: (r) => this.isFavorite.set(r.isFavorite),
          });
        }
      },
      error: () => {
        this.loading.set(false);
        this.errorMessage.set('Kelime yuklenemedi.');
      },
    });
  }

  enrichWord(): void {
    const w = this.word();
    if (!w) return;

    this.enriching.set(true);
    this.wordService.enrichWord(w.id).subscribe({
      next: () => {
        this.enriching.set(false);
        this.loadWord(w.id);
      },
      error: (err) => {
        this.enriching.set(false);
        const msg = Array.isArray(err.error)
          ? err.error.map((e: any) => e.message).join(', ')
          : err.error?.detail ?? err.error?.message ?? 'Zenginlestirme basarisiz oldu. Claude API anahtarini kontrol edin.';
        this.errorMessage.set(msg);
      },
    });
  }

  toggleFavorite(): void {
    const w = this.word();
    if (!w) return;

    if (this.isFavorite()) {
      this.favoriteService.removeFavorite(w.id).subscribe({
        next: () => this.isFavorite.set(false),
      });
    } else {
      this.favoriteService.addFavorite(w.id).subscribe({
        next: () => this.isFavorite.set(true),
      });
    }
  }

  editWord(): void {
    const w = this.word();
    if (w) {
      this.router.navigate(['/words', w.id, 'edit']);
    }
  }

  deleteWord(): void {
    const w = this.word();
    if (!w) return;

    if (confirm('Bu kelimeyi silmek istediginize emin misiniz?')) {
      this.wordService.deleteWord(w.id).subscribe({
        next: () => this.router.navigate(['/words']),
        error: () => this.errorMessage.set('Silme islemi basarisiz oldu.'),
      });
    }
  }

  toggleSenseForm(): void {
    this.showSenseForm.update((v) => !v);
  }

  toggleTranslationForm(senseId: string): void {
    this.showTranslationForm.update((v) => (v === senseId ? null : senseId));
  }

  toggleExampleForm(senseId: string): void {
    this.showExampleForm.update((v) => (v === senseId ? null : senseId));
  }

  onSenseAdded(): void {
    this.showSenseForm.set(false);
    const w = this.word();
    if (w) this.loadWord(w.id);
  }

  onTranslationAdded(): void {
    this.showTranslationForm.set(null);
    const w = this.word();
    if (w) this.loadWord(w.id);
  }

  onExampleAdded(): void {
    this.showExampleForm.set(null);
    const w = this.word();
    if (w) this.loadWord(w.id);
  }

  getStatusLabel(status: WordStatus): string {
    const labels: Record<WordStatus, string> = {
      [WordStatus.Pending]: 'Bekliyor',
      [WordStatus.Enriched]: 'Zenginlestirildi',
      [WordStatus.Failed]: 'Basarisiz',
      [WordStatus.Approved]: 'Onaylandi',
      [WordStatus.Archived]: 'Arsivlendi',
    };
    return labels[status] ?? 'Bilinmiyor';
  }

  getStatusClass(status: WordStatus): string {
    const classes: Record<WordStatus, string> = {
      [WordStatus.Pending]: 'badge-warning',
      [WordStatus.Enriched]: 'badge-info',
      [WordStatus.Failed]: 'badge-danger',
      [WordStatus.Approved]: 'badge-success',
      [WordStatus.Archived]: 'badge-secondary',
    };
    return classes[status] ?? 'badge-secondary';
  }

  getStatusBadgeClass(status: WordStatus): string {
    const classes: Record<WordStatus, string> = {
      [WordStatus.Pending]: 'badge badge-warning',
      [WordStatus.Enriched]: 'badge badge-primary',
      [WordStatus.Failed]: 'badge badge-danger',
      [WordStatus.Approved]: 'badge badge-success',
      [WordStatus.Archived]: 'badge badge-neutral',
    };
    return classes[status] ?? 'badge badge-neutral';
  }

  getEquivalenceLabel(type: EquivalenceType): string {
    const labels: Record<EquivalenceType, string> = {
      [EquivalenceType.Exact]: 'Tam',
      [EquivalenceType.Near]: 'Yakin',
      [EquivalenceType.Loose]: 'Gevsek',
      [EquivalenceType.Gap]: 'Bosluk',
    };
    return labels[type] ?? '';
  }

  getExampleSourceLabel(source: ExampleSource): string {
    const labels: Record<ExampleSource, string> = {
      [ExampleSource.Corpus]: 'Korpus',
      [ExampleSource.Literature]: 'Edebiyat',
      [ExampleSource.Ai]: 'AI',
      [ExampleSource.Manual]: 'Manuel',
    };
    return labels[source] ?? '';
  }

  getDifficultyLabel(level: DifficultyLevel | null): string {
    if (level == null) return '';
    const labels: Record<DifficultyLevel, string> = {
      [DifficultyLevel.A1]: 'A1',
      [DifficultyLevel.A2]: 'A2',
      [DifficultyLevel.B1]: 'B1',
      [DifficultyLevel.B2]: 'B2',
      [DifficultyLevel.C1]: 'C1',
      [DifficultyLevel.C2]: 'C2',
    };
    return labels[level] ?? '';
  }
}
