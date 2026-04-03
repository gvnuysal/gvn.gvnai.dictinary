import { Component, ChangeDetectionStrategy, signal, inject, input, output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { ExampleService } from '../../../../services/example.service';
import { ExampleSource } from '../../../../models/word.model';

@Component({
  selector: 'dict-example-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './example-form.component.html',
  styleUrl: './example-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ExampleFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly exampleService = inject(ExampleService);

  readonly wordId = input.required<string>();
  readonly senseId = input.required<string>();
  readonly saved = output<void>();
  readonly cancelled = output<void>();

  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly exampleSources = [
    { value: ExampleSource.Corpus, label: 'Korpus' },
    { value: ExampleSource.Literature, label: 'Edebiyat' },
    { value: ExampleSource.Ai, label: 'AI' },
    { value: ExampleSource.Manual, label: 'Manuel' },
  ];

  readonly form = this.fb.nonNullable.group({
    sourceText: ['', [Validators.required]],
    targetText: [''],
    source: [ExampleSource.Manual as number, [Validators.required]],
  });

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading.set(true);
    this.errorMessage.set(null);

    const value = this.form.getRawValue();

    this.exampleService.addExample(this.wordId(), this.senseId(), {
      sourceText: value.sourceText,
      targetText: value.targetText || null,
      translationId: null,
      source: value.source,
    }).subscribe({
      next: () => {
        this.loading.set(false);
        this.saved.emit();
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(err.error?.message ?? 'Örnek eklenemedi.');
      },
    });
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
