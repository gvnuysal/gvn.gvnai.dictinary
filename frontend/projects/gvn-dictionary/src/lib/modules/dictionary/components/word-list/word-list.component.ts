import { Component, ChangeDetectionStrategy, signal, computed, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UpperCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { WordService } from '../../../../services/word.service';
import { LookupService } from '../../../../services/lookup.service';
import { AuthService } from '../../../../services/auth.service';
import { WordSummaryDto, WordStatus } from '../../../../models/word.model';
import { LanguageDto } from '../../../../models/lookup.model';
import { PagedResult } from '../../../../models/paged-result.model';

@Component({
  selector: 'dict-word-list',
  standalone: true,
  imports: [FormsModule, UpperCasePipe],
  templateUrl: './word-list.component.html',
  styleUrl: './word-list.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WordListComponent implements OnInit {
  private readonly wordService = inject(WordService);
  private readonly lookupService = inject(LookupService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly languages = signal<LanguageDto[]>([]);
  readonly words = signal<WordSummaryDto[]>([]);
  readonly loading = signal(false);
  readonly isAuthenticated = signal(false);

  readonly selectedLanguageId = signal<string>('');
  readonly pageNumber = signal(1);
  readonly pageSize = signal(20);
  readonly totalPages = signal(0);
  readonly totalCount = signal(0);
  readonly hasPreviousPage = signal(false);
  readonly hasNextPage = signal(false);

  readonly pageNumbers = computed(() => {
    const total = this.totalPages();
    const current = this.pageNumber();
    const pages: number[] = [];
    const maxVisible = 5;
    let start = Math.max(1, current - Math.floor(maxVisible / 2));
    let end = Math.min(total, start + maxVisible - 1);
    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }
    return pages;
  });

  ngOnInit(): void {
    this.isAuthenticated.set(this.authService.isAuthenticated());
    this.loadLookups();
    this.loadWords();
  }

  private loadLookups(): void {
    this.lookupService.getLookups().subscribe({
      next: (lookups) => this.languages.set(lookups.languages),
    });
  }

  loadWords(): void {
    this.loading.set(true);
    const params: { languageId?: string; pageNumber?: number; pageSize?: number } = {
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize(),
    };
    const langId = this.selectedLanguageId();
    if (langId) {
      params.languageId = langId;
    }

    this.wordService.getWords(params).subscribe({
      next: (result: PagedResult<WordSummaryDto>) => {
        this.words.set(result.items);
        this.totalPages.set(result.totalPages);
        this.totalCount.set(result.totalCount);
        this.hasPreviousPage.set(result.hasPreviousPage);
        this.hasNextPage.set(result.hasNextPage);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  onLanguageChange(languageId: string): void {
    this.selectedLanguageId.set(languageId);
    this.pageNumber.set(1);
    this.loadWords();
  }

  goToPage(page: number): void {
    this.pageNumber.set(page);
    this.loadWords();
  }

  navigateToDetail(id: string): void {
    this.router.navigate(['/words', id]);
  }

  navigateToCreate(): void {
    this.router.navigate(['/words', 'new']);
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

  getStatusDotClass(status: WordStatus): string {
    const classes: Record<WordStatus, string> = {
      [WordStatus.Pending]: 'warning',
      [WordStatus.Enriched]: 'info',
      [WordStatus.Failed]: 'danger',
      [WordStatus.Approved]: 'success',
      [WordStatus.Archived]: 'neutral',
    };
    return classes[status] ?? 'neutral';
  }
}
