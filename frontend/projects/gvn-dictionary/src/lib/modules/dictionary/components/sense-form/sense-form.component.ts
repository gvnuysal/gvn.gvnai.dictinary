import { Component, ChangeDetectionStrategy, signal, inject, input, output } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { SenseService } from '../../../../services/sense.service';
import { LookupService } from '../../../../services/lookup.service';
import { RegisterDto, SubjectDomainDto } from '../../../../models/lookup.model';

@Component({
  selector: 'dict-sense-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './sense-form.component.html',
  styleUrl: './sense-form.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SenseFormComponent {
  private readonly fb = inject(FormBuilder);
  private readonly senseService = inject(SenseService);
  private readonly lookupService = inject(LookupService);

  readonly wordId = input.required<string>();
  readonly saved = output<void>();
  readonly cancelled = output<void>();

  readonly registers = signal<RegisterDto[]>([]);
  readonly domains = signal<SubjectDomainDto[]>([]);
  readonly loading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    definition: ['', [Validators.required]],
    definitionShort: [''],
    registerId: [''],
    domainId: [''],
  });

  constructor() {
    this.lookupService.getLookups().subscribe({
      next: (lookups) => {
        this.registers.set(lookups.registers);
        this.domains.set(lookups.domains);
      },
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

    this.senseService.addSense(this.wordId(), {
      definition: value.definition,
      definitionShort: value.definitionShort || null,
      registerId: value.registerId || null,
      domainId: value.domainId || null,
      frequencyRank: null,
      difficultyLevel: null,
    }).subscribe({
      next: () => {
        this.loading.set(false);
        this.saved.emit();
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(err.error?.message ?? 'Anlam eklenemedi.');
      },
    });
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}
