import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { API_URL } from '../config/api.config';
import { Observable } from 'rxjs';

export interface CategoriaDto {
  id: number;
  nombre: string;
}

export interface GastoDto {
  id: number;
  descripcion: string;
  monto: number;
  fechaHora: string;      // ISO o "yyyy-MM-ddTHH:mm"
  categoriaId: number;

  // si tu backend lo devuelve (opcional)
  categoriaNombre?: string;
}

export interface GastoCreateDto {
  descripcion: string;
  monto: number;
  categoriaId: number;
}

export interface CategoriaCreateDto {
  nombre: string;
}

// ✅ IMPORTANTE: tu backend GET /api/Gastos devuelve { total, gastos }
export interface GastosListResponse {
  total: number;
  registros: number;   // ✅ agregar
  gastos: GastoDto[];
}
@Injectable({ providedIn: 'root' })
export class GastosApiService {
  constructor(
    private http: HttpClient,
    @Inject(API_URL) private apiUrl: string
  ) {}

  // =========================
  // GASTOS CRUD
  // =========================

  // ✅ ahora devuelve wrapper { total, gastos }
  getGastos(): Observable<GastosListResponse> {
    return this.http.get<GastosListResponse>(`${this.apiUrl}/api/Gastos`);
  }

  getGastoById(id: number): Observable<GastoDto> {
    return this.http.get<GastoDto>(`${this.apiUrl}/api/Gastos/${id}`);
  }

  createGasto(payload: GastoCreateDto): Observable<GastoDto> {
  return this.http.post<GastoDto>(`${this.apiUrl}/api/Gastos`, payload);
}

updateGasto(id: number, payload: GastoCreateDto): Observable<GastoDto> {
  return this.http.put<GastoDto>(`${this.apiUrl}/api/Gastos/${id}`, payload);
}

  deleteGasto(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/api/Gastos/${id}`);
  }

  // =========================
  // CATEGORIAS CRUD (mínimo)
  // =========================

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