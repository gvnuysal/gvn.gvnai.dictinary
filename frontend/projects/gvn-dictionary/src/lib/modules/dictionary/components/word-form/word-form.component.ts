import { Component, ChangeDetectionStrategy, signal, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { WordService } from '../../../../services/word.service';
import { LookupService } from '../../../../services/lookup.service';
import { PartOfSpeechDto, RegisterDto, SubjectDomainDto } from '../../../../models/lookup.model';

@Component({
  selector: 'dict-word-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './word-form.component.html',
  styleUrl: './word-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WordFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly wordService = inject(WordService);
  private readonly lookupService = inject(LookupService);
  private readonly route = inject(ActivatedRoute);
  readonly router = inject(Router);

  readonly partsOfSpeech = signal<PartOfSpeechDto[]>([]);
  readonly registers = signal<RegisterDto[]>([]);
  readonly domains = signal<SubjectDomainDto[]>([]);
  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly isEditMode = signal(false);

  private wordId: string | null = null;
  private currentLanguageId = '';

  readonly form = this.fb.nonNullable.group({
    lemma: ['', [Validators.required, Validators.maxLength(200)]],
    partOfSpeechId: ['', [Validators.required]],
    definition: ['', [Validators.required, Validators.maxLength(2000)]],
    translationText: ['', [Validators.required, Validators.maxLength(500)]],
    definitionShort: [''],
    registerId: [''],
    domainId: [''],
  });

  ngOnInit(): void {
    this.wordId = this.route.snapshot.paramMap.get('id');
    this.isEditMode.set(!!this.wordId);

    this.lookupService.getLookups().subscribe({
      next: (lookups) => {
        this.partsOfSpeech.set(lookups.partsOfSpeech);
        this.registers.set(lookups.registers);
        this.domains.set(lookups.domains);
      },
    });

    if (this.wordId) {
      this.wordService.getWordById(this.wordId).subscribe({
        next: (word) => {
          this.currentLanguageId = word.language.id;
          this.form.patchValue({
            lemma: word.lemma,
            partOfSpeechId: word.partOfSpeech.id,
          });
          if (word.senses.length > 0) {
            const sense = word.senses[0];
            this.form.patchValue({
              definition: sense.definition,
              definitionShort: sense.definitionShort ?? '',
            });
            if (sense.translations.length > 0) {
              this.form.patchValue({
                translationText: sense.translations[0].translationText,
              });
            }
          }
        },
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set(null);
    const v = this.form.getRawValue();

    if (this.isEditMode() && this.wordId) {
      this.wordService.updateWord(this.wordId, {
        id: this.wordId,
        lemma: v.lemma,
        languageId: this.currentLanguageId,
        partOfSpeechId: v.partOfSpeechId,
        frequencyRank: null,
        difficultyLevel: null,
        isCompound: false,
        isIdiom: false,
        isProperNoun: false,
      }).subscribe({
        next: () => {
          this.loading.set(false);
          this.router.navigate(['/words', this.wordId]);
        },
        error: (err) => {
          this.loading.set(false);
          this.errorMessage.set(this.extractError(err));
        },
      });
    } else {
      this.wordService.createWordWithTranslation({
        lemma: v.lemma,
        partOfSpeechId: v.partOfSpeechId,
        definition: v.definition,
        translationText: v.translationText,
        definitionShort: v.definitionShort || null,
        registerId: v.registerId || null,
        domainId: v.domainId || null,
      }).subscribe({
        next: (id) => {
          this.loading.set(false);
          this.router.navigate(['/words', id]);
        },
        error: (err) => {
          this.loading.set(false);
          this.errorMessage.set(this.extractError(err));
        },
      });
    }
  }

  private extractError(err: any): string {
    if (Array.isArray(err.error)) {
      return err.error.map((e: any) => e.message).join(', ');
    }
    return err.error?.message ?? err.error?.detail ?? 'Bir hata oluştu.';
  }
}
