import { Service, signal, inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, tap } from "rxjs";

export interface AuthResponse {
  token: string;
  email: string;
  fullName: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

@Service()
export class Auth {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5023/api/auth';

  // signal holding the current logged-in user's info (or null if logged out)
  currentUser = signal<{ email: string; fullName: string } | null>(null);

  constructor() {
    this.loadUserFromStorage();
  }

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, data).pipe(
      tap((response) => this.handleAuthSuccess(response))
    );
  }

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, data).pipe(
      tap((response) => this.handleAuthSuccess(response))
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    localStorage.removeItem('fullName');
    this.currentUser.set(null);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }

  private handleAuthSuccess(response: AuthResponse): void {
    localStorage.setItem('token', response.token);
    localStorage.setItem('email', response.email);
    localStorage.setItem('fullName', response.fullName);
    this.currentUser.set({ email: response.email, fullName: response.fullName })
  }

  private loadUserFromStorage(): void {
    const email = localStorage.getItem("email");
    const fullName = localStorage.getItem("fullName");
    if (email && fullName) {
      this.currentUser.set({ email, fullName });
    }
  }

}
