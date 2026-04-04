import { Component, ChangeDetectionStrategy, signal, inject, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { DatePipe, DecimalPipe, UpperCasePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProfileService } from '../../../../services/profile.service';
import { FavoriteService } from '../../../../services/favorite.service';
import { ProfileDto } from '../../../../models/profile.model';
import { FavoriteWordDto } from '../../../../models/favorite.model';

@Component({
  selector: 'dict-profile',
  standalone: true,
  imports: [ReactiveFormsModule, DatePipe, DecimalPipe, UpperCasePipe, RouterLink],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileComponent implements OnInit {
  private readonly profileService = inject(ProfileService);
  private readonly favoriteService = inject(FavoriteService);
  private readonly fb = inject(FormBuilder);

  readonly profile = signal<ProfileDto | null>(null);
  readonly favorites = signal<FavoriteWordDto[]>([]);
  readonly loading = signal(true);
  readonly editing = signal(false);
  readonly saving = signal(false);
  readonly successMessage = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    fullName: ['', [Validators.required, Validators.maxLength(200)]],
  });

  // API Settings form
  readonly editingApi = signal(false);
  readonly savingApi = signal(false);
  readonly apiForm = this.fb.nonNullable.group({
    translateProvider: ['claude'],
    claudeApiKey: [''],
    googleTranslateApiKey: [''],
  });

  ngOnInit(): void {
    this.loadProfile();
    this.loadFavorites();
  }

  private loadProfile(): void {
    this.profileService.getProfile().subscribe({
      next: (p) => {
        this.profile.set(p);
        this.form.patchValue({ fullName: p.fullName });
        this.apiForm.patchValue({ translateProvider: p.apiSettings.translateProvider });
        this.loading.set(false);
      },
      error: () => this.loading.set(false),
    });
  }

  private loadFavorites(): void {
    this.favoriteService.getFavorites().subscribe({
      next: (f) => this.favorites.set(f),
    });
  }

  startEdit(): void {
    this.editing.set(true);
    this.successMessage.set(null);
  }

  cancelEdit(): void {
    const p = this.profile();
    if (p) this.form.patchValue({ fullName: p.fullName });
    this.editing.set(false);
  }

  saveProfile(): void {
    if (this.form.invalid) return;
    this.saving.set(true);
    this.profileService.updateProfile({ fullName: this.form.getRawValue().fullName }).subscribe({
      next: () => {
        this.saving.set(false);
        this.editing.set(false);
        this.successMessage.set('Profil guncellendi.');
        this.loadProfile();
      },
      error: () => this.saving.set(false),
    });
  }

  startEditApi(): void {
    this.editingApi.set(true);
    this.successMessage.set(null);
  }

  cancelEditApi(): void {
    const p = this.profile();
    if (p) this.apiForm.patchValue({ translateProvider: p.apiSettings.translateProvider });
    this.apiForm.patchValue({ claudeApiKey: '', googleTranslateApiKey: '' });
    this.editingApi.set(false);
  }

  saveApiSettings(): void {
    this.savingApi.set(true);
    const v = this.apiForm.getRawValue();
    this.profileService.updateApiSettings({
      translateProvider: v.translateProvider,
      claudeApiKey: v.claudeApiKey || null,
      googleTranslateApiKey: v.googleTranslateApiKey || null,
    }).subscribe({
      next: () => {
        this.savingApi.set(false);
        this.editingApi.set(false);
        this.successMessage.set('API ayarlari guncellendi.');
        this.loadProfile();
      },
      error: () => this.savingApi.set(false),
    });
  }

  removeFavorite(wordId: string): void {
    this.favoriteService.removeFavorite(wordId).subscribe({
      next: () => {
        this.favorites.update(f => f.filter(x => x.wordId !== wordId));
        this.loadProfile(); // refresh stats
      },
    });
  }
}
