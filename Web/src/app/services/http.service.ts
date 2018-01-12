import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { of } from 'rxjs/observable/of';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, map, tap } from 'rxjs/operators';
import { NotificationsService } from "angular2-notifications";
import { environment } from '../../environments/environment';

@Injectable()
export class HttpService {

  constructor(private http: HttpClient, private notificationService: NotificationsService) { }

  private jwt: string = "auc_jwt";
  private api_url: string = "http://localhost:52448/api/"; 

  private getAccessToken(): string {
    return window ? window.localStorage.getItem(this.jwt) : null;
  }

  private setAccessToken(newJwt: string): void {
    if (window) window.localStorage.setItem(this.jwt, newJwt);
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

  post<T>(url: string, model: T, httpOptions: any = {}): Observable<any> {
    if (!httpOptions) {
      httpOptions = { headers: this.baseHttpHeaders() };
    }
    else if (!httpOptions["headers"]) {
      httpOptions["headers"] = this.baseHttpHeaders();
    }
    return this.http.post<any>(url, model, httpOptions).pipe(
      tap((response: any) => {
        if (response && response.jwt) this.setAccessToken(response.jwt);
      }),
      catchError(this.handleError<T>(url))
    );
  }

  get<T>(url: string, httpOptions: any = {}): Observable<any> {
    if (!httpOptions) {
      httpOptions = { headers: this.baseHttpHeaders() };
    }
    else if (!httpOptions["headers"]) {
      httpOptions["headers"] = this.baseHttpHeaders();
    }
    return this.http
    .get<any>(url, httpOptions)
    .pipe(
      tap((response: any) => {
        if (response && response.jwt) this.setAccessToken(response.jwt);
      }),
      catchError(this.handleError<T>(url))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (response: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      if (response.status == "400"){
        if (response.error){
          this.notificationService.error("Error", response.error.error);
        }
        else {
          this.notificationService.error("Error", "Error on request");
        }
      }
      console.error(response); // log to console instead

      // TODO: better job of transforming error for user consumption
      //this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

}
