import { Component, ChangeDetectionStrategy, signal, inject, input, output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { TranslationService } from '../../../../services/translation.service';
import { LookupService } from '../../../../services/lookup.service';
import { LanguageDto } from '../../../../models/lookup.model';
import { EquivalenceType } from '../../../../models/word.model';

@Component({
  selector: 'dict-translation-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './translation-form.component.html',
  styleUrl: './translation-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TranslationFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly translationService = inject(TranslationService);
  private readonly lookupService = inject(LookupService);

  readonly wordId = input.required<string>();
  readonly senseId = input.required<string>();
  readonly saved = output<void>();
  readonly cancelled = output<void>();

  readonly languages = signal<LanguageDto[]>([]);
  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly equivalenceTypes = [
    { value: EquivalenceType.Exact, label: 'Tam' },
    { value: EquivalenceType.Near, label: 'Yakın' },
    { value: EquivalenceType.Loose, label: 'Gevşek' },
    { value: EquivalenceType.Gap, label: 'Boşluk' },
  ];

  readonly form = this.fb.nonNullable.group({
    targetLanguageId: ['', [Validators.required]],
    translationText: ['', [Validators.required]],
    equivalenceType: [EquivalenceType.Exact as number, [Validators.required]],
    confidenceScore: [0.8, [Validators.required, Validators.min(0), Validators.max(1)]],
  });

  constructor() {
    this.lookupService.getLookups().subscribe({
      next: (lookups) => this.languages.set(lookups.languages),
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set(null);

    const value = this.form.getRawValue();

    this.translationService.addTranslation(this.wordId(), this.senseId(), {
      targetLanguageId: value.targetLanguageId,
      translationText: value.translationText,
      partOfSpeechId: null,
      registerId: null,
      equivalenceType: value.equivalenceType,
      confidenceScore: value.confidenceScore,
    }).subscribe({
      next: () => {
        this.loading.set(false);
        this.saved.emit();
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(err.error?.message ?? 'Çeviri eklenemedi.');
      },
    });
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
