import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from '../config/api.config';

export interface CategoriaDto {
  id: number;
  nombre: string;
}

export interface CategoriaCreateDto {
  nombre: string;
}

@Injectable({ providedIn: 'root' })
export class CategoriaApiService {
  constructor(
    private http: HttpClient,
    @Inject(API_URL) private apiUrl: string
  ) {}

  getCategorias(): Observable<CategoriaDto[]> {
    return this.http.get<CategoriaDto[]>(`${this.apiUrl}/api/Categorias`);
  }

  getCategoriaById(id: number): Observable<CategoriaDto> {
    return this.http.get<CategoriaDto>(`${this.apiUrl}/api/Categorias/${id}`);
  }

  createCategoria(payload: CategoriaCreateDto): Observable<CategoriaDto> {
    return this.http.post<CategoriaDto>(`${this.apiUrl}/api/Categorias`, payload);
  }

  deleteCategoria(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/Categorias/${id}`);
  }
}