import { Routes } from '@angular/router';
import { authGuard } from 'gvn-dictionary';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('gvn-dictionary').then(m => m.HomeComponent)
  },
  {
    path: 'login',
    loadComponent: () => import('gvn-dictionary').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('gvn-dictionary').then(m => m.RegisterComponent)
  },
  {
    path: 'words',
    loadComponent: () => import('gvn-dictionary').then(m => m.WordListComponent),
    canActivate: [authGuard]
  },
  {
    path: 'words/new',
    loadComponent: () => import('gvn-dictionary').then(m => m.WordFormComponent),
    canActivate: [authGuard]
  },
  {
    path: 'words/search',
    loadComponent: () => import('gvn-dictionary').then(m => m.WordSearchComponent),
    canActivate: [authGuard]
  },
  {
    path: 'words/:id',
    loadComponent: () => import('gvn-dictionary').then(m => m.WordDetailComponent),
    canActivate: [authGuard]
  },
  {
    path: 'words/:id/edit',
    loadComponent: () => import('gvn-dictionary').then(m => m.WordFormComponent),
    canActivate: [authGuard]
  },
  {
    path: 'quiz',
    loadComponent: () => import('gvn-dictionary').then(m => m.QuizComponent),
    canActivate: [authGuard]
  },
  {
    path: 'profile',
    loadComponent: () => import('gvn-dictionary').then(m => m.ProfileComponent),
    canActivate: [authGuard]
  }
];
