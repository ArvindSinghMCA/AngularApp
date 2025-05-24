import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Position {
  securityCode: string;
  quantity: number;
}

@Injectable({
  providedIn: 'root'
})
export class PositionsService {
  private apiUrl = 'https://localhost:7139/api/Transactions/positions'; // your API URL

  constructor(private http: HttpClient) { }

  getPositions(): Observable<Position[]> {
    return this.http.get<Position[]>(this.apiUrl);
  }
}
