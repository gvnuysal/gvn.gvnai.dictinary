import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'dict-home',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  get isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }

  goToWords(): void {
    this.router.navigate(['/words']);
  }
}
