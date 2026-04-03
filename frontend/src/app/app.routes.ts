import { Routes } from '@angular/router';
import { authGuard } from 'gvn-dictionary';

export const routes: Routes = [
  { path: '', redirectTo: 'words', pathMatch: 'full' },
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
    loadComponent: () => import('gvn-dictionary').then(m => m.WordListComponent)
  },
  {
    path: 'words/new',
    loadComponent: () => import('gvn-dictionary').then(m => m.WordFormComponent),
    canActivate: [authGuard]
  },
  {
    path: 'words/search',
    loadComponent: () => import('gvn-dictionary').then(m => m.WordSearchComponent)
  },
  {
    path: 'words/:id',
    loadComponent: () => import('gvn-dictionary').then(m => m.WordDetailComponent)
  },
  {
    path: 'words/:id/edit',
    loadComponent: () => import('gvn-dictionary').then(m => m.WordFormComponent),
    canActivate: [authGuard]
  }
];
