import { Injectable } from '@angular/core';
import { URLSearchParams } from '@angular/http'
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { NotificationsService } from "angular2-notifications";
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';
import { LocalStorageService } from "./local-storage.service";

@Injectable()
export class HttpService {

  constructor(private http: HttpClient, private notificationService: NotificationsService, private router: Router,
    private localStorageService: LocalStorageService) { }

  private jwt: string = "auc_jwt";
  private login: string = "auc_login";

  public isLoggedIn(): boolean {
    return this.getAccessToken() != null;
  }

  private getAccessToken(): string {
    return (this.localStorageService.getLocalStorage(this.jwt) as string);
  }

  private setAccessToken(newJwt: string): void {
    this.localStorageService.setLocalStorage(this.jwt, newJwt);
  }

  public setLoginData(loginData: string): void {
    this.localStorageService.setLocalStorage(this.login, JSON.stringify(loginData));
  }

  public getLoginData(): any {
    let loginData = this.localStorageService.getLocalStorage(this.login);
    return JSON.parse(loginData);
  }

  getUser(): string {
    return this.getLoginData().email;
  }

  logout(): void {
    this.localStorageService.removeLocalStorage(this.jwt);
    this.localStorageService.removeLocalStorage(this.login);
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

  post<T>(url: string, model?: T, httpOptions: any = {}): Observable<any> {
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
      else if (response.status != "200") {
        if (response.error) {
          this.notificationService.error("Error", response.error.error);
        }
        else if (response.statusText) {
          this.notificationService.error("Error", response.statusText);
        }
        else {
          this.notificationService.error("Error", "An unexpected error happened.");
        }
      }

      return of(result as T);
    };
  }

}
