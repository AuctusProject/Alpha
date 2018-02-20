import { Injectable } from '@angular/core';
import { URLSearchParams } from '@angular/http'
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { NotificationsService } from "angular2-notifications";
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';

@Injectable()
export class HttpService {

  constructor(private http: HttpClient, private notificationService: NotificationsService, private router: Router) { }

  private jwt: string = "auc_jwt";
  private login: string = "auc_login";

  public isLoggedIn(): boolean {
    return this.getAccessToken() != null;
  }

  private getAccessToken(): string {
    return (this.getLocalStorage(this.jwt) as string);
  }

  private setAccessToken(newJwt: string): void {
    this.setLocalStorage(this.jwt, newJwt);
  }

  public setLoginData(loginData: string): void {
    this.setLocalStorage(this.login, loginData);
  }

  private setLocalStorage(key: string, value: any): void {
    if (window) window.localStorage.setItem(key, value);
  }

  private getLocalStorage(key: string): any {
    return window ? window.localStorage.getItem(key) : null;
  }

  private removeLocalStorage(key: string): void {
    if (window) window.localStorage.removeItem(key);
  }

  getUser(): string {
    return (this.getLocalStorage(this.login).Email as string);
  }

  logout(): void {
    this.removeLocalStorage(this.jwt);
    this.removeLocalStorage(this.login);
  }

  apiUrl(route: string): string {
    return environment.apiUrl + route;
  }

  baseHttpHeaders(): any {
    var token = this.getAccessToken();
    var header;
    if (token) {
      header = {
        'Content-Type': 'application/json',
        'Authorization': ('Bearer ' + token)
      };
    } else {
      header = { 'Content-Type': 'application/json' };
    }
    return new HttpHeaders(header);
  }

  getHttpOptions(httpOptions: any): any {
    if (!httpOptions) {
      httpOptions = { headers: this.baseHttpHeaders() };
    }
    else if (!httpOptions["headers"]) {
      httpOptions["headers"] = this.baseHttpHeaders();
    }
    return httpOptions;
  }

  post<T>(url: string, model: T, httpOptions: any = {}): Observable<any> {
    return this.http.post<any>(url, model, this.getHttpOptions(httpOptions)).pipe(
      tap((response: any) => {
        if (response && response.jwt) this.setAccessToken(response.jwt);
      }),
      catchError(this.handleError<T>(url))
    );
  }

  get(url: string, httpOptions: any = {}): Observable<any> {
    return this.http.get<any>(url, this.getHttpOptions(httpOptions))
      .pipe(
      tap((response: any) => {
        if (response && response.jwt) this.setAccessToken(response.jwt);
      }),
      catchError(this.handleError(url))
      );
  }

  put<T>(url: string, model: T, httpOptions: any = {}): Observable<any> {
    return this.http.put<any>(url, model, this.getHttpOptions(httpOptions))
      .pipe(
      tap((response: any) => {
        if (response && response.jwt) this.setAccessToken(response.jwt);
      }),
      catchError(this.handleError<T>(url))
      );
  }

  patch<T>(url: string, model: T, httpOptions: any = {}): Observable<any> {
    return this.http.patch<any>(url, model, this.getHttpOptions(httpOptions))
      .pipe(
      tap((response: any) => {
        if (response && response.jwt) this.setAccessToken(response.jwt);
      }),
      catchError(this.handleError<T>(url))
      );
  }

  delete<T>(url: string, httpOptions: any = {}): Observable<any> {
    return this.http.delete<any>(url, this.getHttpOptions(httpOptions))
      .pipe(
      tap((response: any) => {
        if (response && response.jwt) this.setAccessToken(response.jwt);
      }),
      catchError(this.handleError<T>(url))
      );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (response: any): Observable<T> => {
      if (response.status == "401") {
        this.logout();
        this.router.navigateByUrl('login');
      }
      // TODO: send the error to remote logging infrastructure
      if (response.status == "400") {
        if (response.error) {
          this.notificationService.error("Error", response.error.error);
        }
        else {
          this.notificationService.error("Error", "An unexpected error happened.");
        }
      }
      else if (!response.ok) {
        this.notificationService.error("Error", "An unexpected error happened.");
      }
      console.error(response); // log to console instead

      return of(result as T);
    };
  }

}
