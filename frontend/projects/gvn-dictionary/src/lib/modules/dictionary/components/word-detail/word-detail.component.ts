import { Component, ChangeDetectionStrategy, signal, inject, OnInit } from '@angular/core';
import { DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { WordService } from '../../../../services/word.service';
import { AuthService } from '../../../../services/auth.service';
import { WordDto, WordStatus, EquivalenceType, ExampleSource, DifficultyLevel } from '../../../../models/word.model';
import { SenseFormComponent } from '../sense-form/sense-form.component';
import { TranslationFormComponent } from '../translation-form/translation-form.component';
import { ExampleFormComponent } from '../example-form/example-form.component';

@Component({
  selector: 'dict-word-detail',
  standalone: true,
  imports: [DatePipe, SenseFormComponent, TranslationFormComponent, ExampleFormComponent],
  templateUrl: './word-detail.component.html',
  styleUrl: './word-detail.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WordDetailComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly wordService = inject(WordService);
  private readonly authService = inject(AuthService);

  readonly word = signal<WordDto | null>(null);
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
      },
      error: () => {
        this.loading.set(false);
        this.errorMessage.set('Kelime yüklenemedi.');
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
      error: () => {
        this.enriching.set(false);
        this.errorMessage.set('Zenginleştirme başarısız oldu.');
      },
    });
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

    if (confirm('Bu kelimeyi silmek istediğinize emin misiniz?')) {
      this.wordService.deleteWord(w.id).subscribe({
        next: () => this.router.navigate(['/words']),
        error: () => this.errorMessage.set('Silme işlemi başarısız oldu.'),
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
      [WordStatus.Enriched]: 'Zenginleştirildi',
      [WordStatus.Failed]: 'Başarısız',
      [WordStatus.Approved]: 'Onaylandı',
      [WordStatus.Archived]: 'Arşivlendi',
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

  getEquivalenceLabel(type: EquivalenceType): string {
    const labels: Record<EquivalenceType, string> = {
      [EquivalenceType.Exact]: 'Tam',
      [EquivalenceType.Near]: 'Yakın',
      [EquivalenceType.Loose]: 'Gevşek',
      [EquivalenceType.Gap]: 'Boşluk',
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
