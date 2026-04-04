import { Component, ChangeDetectionStrategy, signal, inject, OnInit, OnDestroy } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, debounceTime, distinctUntilChanged, filter, switchMap, takeUntil } from 'rxjs';
import { WordService } from '../../../../services/word.service';
import { LookupService } from '../../../../services/lookup.service';
import { SpeechService } from '../../../../services/speech.service';
import { TranslateService } from '../../../../services/translate.service';
import { PartOfSpeechDto, RegisterDto, SubjectDomainDto } from '../../../../models/lookup.model';

@Component({
  selector: 'dict-word-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './word-form.component.html',
  styleUrl: './word-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class WordFormComponent implements OnInit, OnDestroy {
  private readonly fb = inject(FormBuilder);
  private readonly wordService = inject(WordService);
  private readonly lookupService = inject(LookupService);
  private readonly translateService = inject(TranslateService);
  private readonly route = inject(ActivatedRoute);
  readonly router = inject(Router);
  readonly speech = inject(SpeechService);

  readonly partsOfSpeech = signal<PartOfSpeechDto[]>([]);
  readonly registers = signal<RegisterDto[]>([]);
  readonly domains = signal<SubjectDomainDto[]>([]);
  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);
  readonly isEditMode = signal(false);
  readonly listeningField = signal<string | null>(null);
  readonly translating = signal(false);

  private wordId: string | null = null;
  private currentLanguageId = '';
  private destroy$ = new Subject<void>();

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
          this.form.patchValue({ lemma: word.lemma, partOfSpeechId: word.partOfSpeech.id });
          if (word.senses.length > 0) {
            const sense = word.senses[0];
            this.form.patchValue({ definition: sense.definition, definitionShort: sense.definitionShort ?? '' });
            if (sense.translations.length > 0) {
              this.form.patchValue({ translationText: sense.translations[0].translationText });
            }
          }
        },
      });
    } else {
      this.setupAutoTranslate();
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private setupAutoTranslate(): void {
    // [PASIF] Google Translate: Türkçe karşılık otomatik çevir
    // Aktif etmek için yorum satırını kaldırın ve appsettings.json'da GoogleTranslate:ApiKey ayarlayın
    // this.form.controls.lemma.valueChanges.pipe(...).subscribe(...)

    // Lemma değişince → Claude AI ile Türkçe karşılık getir
    this.form.controls.lemma.valueChanges.pipe(
      takeUntil(this.destroy$),
      debounceTime(600),
      distinctUntilChanged(),
      filter(v => v.trim().length >= 2),
      switchMap(text => {
        this.translating.set(true);
        return this.translateService.aiTranslate(text, 'tr');
      })
    ).subscribe({
      next: (translated) => {
        this.translating.set(false);
        if (!this.form.controls.translationText.dirty || !this.form.controls.translationText.value) {
          this.form.controls.translationText.setValue(translated);
        }
      },
      error: () => this.translating.set(false),
    });

    // Lemma değişince → Claude AI ile İngilizce tanım getir
    this.form.controls.lemma.valueChanges.pipe(
      takeUntil(this.destroy$),
      debounceTime(800),
      distinctUntilChanged(),
      filter(v => v.trim().length >= 2),
      switchMap(text => this.translateService.define(text))
    ).subscribe({
      next: (definition) => {
        if (!this.form.controls.definition.dirty || !this.form.controls.definition.value) {
          this.form.controls.definition.setValue(definition);
        }
      },
    });

    // Lemma değişince → Claude AI ile sözcük türü algıla
    this.form.controls.lemma.valueChanges.pipe(
      takeUntil(this.destroy$),
      debounceTime(700),
      distinctUntilChanged(),
      filter(v => v.trim().length >= 2),
      switchMap(text => this.translateService.detectPos(text))
    ).subscribe({
      next: (posCode) => {
        if (!this.form.controls.partOfSpeechId.dirty || !this.form.controls.partOfSpeechId.value) {
          const match = this.partsOfSpeech().find(p => p.code === posCode);
          if (match) {
            this.form.controls.partOfSpeechId.setValue(match.id);
          }
        }
      },
    });
  }

  startListening(field: 'lemma' | 'definition' | 'translationText' | 'definitionShort'): void {
    if (this.speech.isListening()) {
      this.speech.stop();
      this.listeningField.set(null);
      return;
    }

    const lang = (field === 'translationText') ? 'tr-TR' : 'en-US';
    this.listeningField.set(field);

    this.speech.listen(lang).then(text => {
      this.form.get(field)?.setValue(text);
      this.listeningField.set(null);
    }).catch(() => {
      this.listeningField.set(null);
    });
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
        id: this.wordId, lemma: v.lemma, languageId: this.currentLanguageId,
        partOfSpeechId: v.partOfSpeechId, frequencyRank: null, difficultyLevel: null,
        isCompound: false, isIdiom: false, isProperNoun: false,
      }).subscribe({
        next: () => { this.loading.set(false); this.router.navigate(['/words', this.wordId]); },
        error: (err) => { this.loading.set(false); this.errorMessage.set(this.extractError(err)); },
      });
    } else {
      this.wordService.createWordWithTranslation({
        lemma: v.lemma, partOfSpeechId: v.partOfSpeechId, definition: v.definition,
        translationText: v.translationText, definitionShort: v.definitionShort || null,
        registerId: v.registerId || null, domainId: v.domainId || null,
      }).subscribe({
        next: (id) => { this.loading.set(false); this.router.navigate(['/words', id]); },
        error: (err) => { this.loading.set(false); this.errorMessage.set(this.extractError(err)); },
      });
    }
  }

  private extractError(err: any): string {
    if (Array.isArray(err.error)) {
      return err.error.map((e: any) => {
        if (e.code === 'WORD_DUPLICATE') return `"${this.form.getRawValue().lemma}" kelimesi zaten mevcut.`;
        return e.message;
      }).join(', ');
    }
    return err.error?.message ?? err.error?.detail ?? 'Bir hata olustu.';
  }
}
