import { Component, ChangeDetectionStrategy, signal, computed, inject, OnInit, OnDestroy } from '@angular/core';
import { UpperCasePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';
import { WordService } from '../../../../services/word.service';
import { LookupService } from '../../../../services/lookup.service';
import { WordSummaryDto, WordStatus } from '../../../../models/word.model';
import { LookupsDto } from '../../../../models/lookup.model';
import { PagedResult } from '../../../../models/paged-result.model';

@Component({
  selector: 'dict-word-search',
  standalone: true,
  imports: [FormsModule, UpperCasePipe],
  templateUrl: './word-search.component.html',
  styleUrl: './word-search.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WordSearchComponent implements OnInit, OnDestroy {
  private readonly wordService = inject(WordService);
  private readonly lookupService = inject(LookupService);
  private readonly router = inject(Router);

  private readonly destroy$ = new Subject<void>();
  private readonly searchSubject = new Subject<string>();

  readonly lookups = signal<LookupsDto | null>(null);
  readonly words = signal<WordSummaryDto[]>([]);
  readonly loading = signal(false);
  readonly totalCount = signal(0);

  readonly searchQuery = signal('');
  readonly selectedLanguageId = signal('');
  readonly selectedPosId = signal('');
  readonly selectedDomainId = signal('');
  readonly selectedRegisterId = signal('');
  readonly pageNumber = signal(1);
  readonly pageSize = signal(20);
  readonly totalPages = signal(0);
  readonly hasNextPage = signal(false);
  readonly hasPreviousPage = signal(false);

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
    this.lookupService.getLookups().subscribe({
      next: (lookups) => this.lookups.set(lookups),
    });

    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.destroy$),
    ).subscribe((query) => {
      this.searchQuery.set(query);
      this.pageNumber.set(1);
      this.doSearch();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  onSearchInput(value: string): void {
    this.searchSubject.next(value);
  }

  onFilterChange(): void {
    this.pageNumber.set(1);
    this.doSearch();
  }

  doSearch(): void {
    this.loading.set(true);

    this.wordService.searchWords({
      q: this.searchQuery() || undefined,
      languageId: this.selectedLanguageId() || undefined,
      partOfSpeechId: this.selectedPosId() || undefined,
      domainId: this.selectedDomainId() || undefined,
      registerId: this.selectedRegisterId() || undefined,
      pageNumber: this.pageNumber(),
      pageSize: this.pageSize(),
    }).subscribe({
      next: (result: PagedResult<WordSummaryDto>) => {
        this.words.set(result.items);
        this.totalCount.set(result.totalCount);
        this.totalPages.set(result.totalPages);
        this.hasNextPage.set(result.hasNextPage);
        this.hasPreviousPage.set(result.hasPreviousPage);
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  goToPage(page: number): void {
    this.pageNumber.set(page);
    this.doSearch();
  }

  navigateToDetail(id: string): void {
    this.router.navigate(['/words', id]);
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
