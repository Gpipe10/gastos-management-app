import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { finalize } from 'rxjs/operators';

import {
  GastosApiService,
  GastoDto,
  CategoriaDto,
  GastoCreateDto,
  GastosListResponse
} from '../../services/gastos-api.service';

@Component({
  selector: 'app-gastos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './gastos.component.html',
  styleUrls: ['./gastos.component.css'],
})
export class GastosComponent implements OnInit {

  constructor(
    private api: GastosApiService,
    private cdr: ChangeDetectorRef,
    private snackBar: MatSnackBar
  ) {}

  // =========================
  // PAGINACIÓN (solo UI)
  // =========================
  pageSize = 10;
  currentPage = 1;

  get totalPages(): number {
    return Math.max(1, Math.ceil(this.visibleGastos.length / this.pageSize));
  }

  setPage(page: number) {
    this.currentPage = Math.min(Math.max(1, page), this.totalPages);
  }

  nextPage() {
    this.setPage(this.currentPage + 1);
  }

  prevPage() {
    this.setPage(this.currentPage - 1);
  }

  // Lista paginada (lo que se renderiza en la tabla)
  get pagedGastos(): GastoDto[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.visibleGastos.slice(start, start + this.pageSize);
  }

  // =========================
  // VIEW
  // =========================
  view: 'LISTA' | 'REGISTRO' = 'LISTA';

  // =========================
  // DATA
  // =========================
  gastos: GastoDto[] = [];
  categorias: CategoriaDto[] = [];
  loading = false;

  // ✅ Totales calculados en backend
  totalBackend = 0;
  registrosBackend = 0;

  // =========================
  // FILTROS (solo UI)
  // =========================
  filtroTexto = '';
  filtroCategoriaId: string = '';

  limpiarFiltros() {
    this.filtroTexto = '';
    this.filtroCategoriaId = '';
    this.currentPage = 1;
  }

  // =========================
  // ORDENAMIENTO (solo UI)
  // =========================
  sortField: 'fechaHora' | 'monto' | 'descripcion' | 'categoria' | null = null;
  sortDir: 'asc' | 'desc' = 'desc';

  sortBy(field: 'fechaHora' | 'monto' | 'descripcion' | 'categoria') {
    if (this.sortField === field) {
      this.sortDir = this.sortDir === 'asc' ? 'desc' : 'asc';
    } else {
      this.sortField = field;
      this.sortDir = 'asc';
    }
    this.currentPage = 1; // ✅ siempre vuelve a la página 1 al ordenar
  }

  // =========================
  // GETTERS (solo UI)
  // =========================
  get visibleGastos(): GastoDto[] {
    let data = Array.isArray(this.gastos) ? [...this.gastos] : [];

    if (this.filtroTexto) {
      const t = this.filtroTexto.toLowerCase();
      data = data.filter(g => (g.descripcion || '').toLowerCase().includes(t));
    }

    if (this.filtroCategoriaId) {
      const id = Number(this.filtroCategoriaId);
      data = data.filter(g => g.categoriaId === id);
    }

    if (this.sortField) {
      data.sort((a, b) => {
        const dir = this.sortDir === 'asc' ? 1 : -1;

        switch (this.sortField) {
          case 'fechaHora': {
            const fa = new Date(a.fechaHora).getTime();
            const fb = new Date(b.fechaHora).getTime();
            return (fa - fb) * dir;
          }
          case 'monto':
            return ((a.monto ?? 0) - (b.monto ?? 0)) * dir;

          case 'descripcion':
            return (String(a.descripcion ?? '').localeCompare(String(b.descripcion ?? ''))) * dir;

          case 'categoria': {
            const ca = String((a as any).categoriaNombre ?? '');
            const cb = String((b as any).categoriaNombre ?? '');
            return ca.localeCompare(cb) * dir;
          }

          default:
            return 0;
        }
      });
    }

    return data;
  }

  // =========================
  // FORM
  // =========================
  form: any = {
    id: null,
    descripcion: '',
    monto: null,
    categoriaId: ''
  };

  submitting = false;
  errorMessage = '';

  guardarGasto() {
    if (!this.form.descripcion || !this.form.monto || !this.form.categoriaId) {
      this.errorMessage = 'Descripción, monto y categoría son obligatorios.';
      return;
    }

    if (Number(this.form.monto) <= 0) {
      this.errorMessage = 'El monto debe ser mayor a 0.';
      return;
    }

    this.errorMessage = '';
    this.submitting = true;

    const payload: GastoCreateDto = {
      descripcion: this.form.descripcion,
      monto: Number(this.form.monto),
      categoriaId: Number(this.form.categoriaId)
    };

    // ✅ UPDATE
    if (this.form.id) {
      this.api.updateGasto(this.form.id, payload)
        .pipe(finalize(() => {
          this.submitting = false;       // ✅ SIEMPRE se apaga
          this.cdr.detectChanges();
        }))
        .subscribe({
          next: (updated: GastoDto) => {
            const idx = this.gastos.findIndex(g => g.id === updated.id);
            if (idx !== -1) this.gastos[idx] = updated;

            this.afterSave('Gasto actualizado');
          },
          error: (e: HttpErrorResponse) => this.handleError(e),
        });

      return;
    }

    // ✅ CREATE
    this.api.createGasto(payload)
      .pipe(finalize(() => {
        this.submitting = false;         // ✅ SIEMPRE se apaga
        this.cdr.detectChanges();
      }))
      .subscribe({
        next: () => {
          this.afterSave('Gasto registrado');
          this.loadGastos(); // recarga para ver el nuevo y actualizar totales backend
        },
        error: (e: HttpErrorResponse) => this.handleError(e),
      });
  }

  afterSave(msg: string) {
    this.view = 'LISTA';
    this.resetForm();

    this.snackBar.open(`${msg} correctamente`, 'Cerrar', {
      duration: 3000,
      horizontalPosition: 'right',
      verticalPosition: 'top',
      panelClass: ['success-snackbar']
    });

    this.cdr.detectChanges();
  }

  editarGasto(g: GastoDto) {
    this.errorMessage = '';
    this.form = {
      id: g.id,
      descripcion: g.descripcion,
      monto: g.monto,
      categoriaId: g.categoriaId
    };
    this.view = 'REGISTRO';
    this.cdr.detectChanges();
  }

  eliminarGasto(id: number) {
    this.errorMessage = '';

    this.api.deleteGasto(id).subscribe({
      next: () => this.loadGastos(),
      error: (e: HttpErrorResponse) => this.handleError(e),
    });
  }

  resetForm() {
    this.form = {
      id: null,
      descripcion: '',
      monto: null,
      categoriaId: ''
    };
  }

  // =========================
  // LOADERS
  // =========================
  ngOnInit(): void {
    this.loadCategorias();
    this.loadGastos();
  }

  loadGastos() {
    this.loading = true;

    this.api.getGastos()
      .pipe(finalize(() => {
        this.loading = false; // ✅ SIEMPRE apaga loading
        this.cdr.detectChanges();
      }))
      .subscribe({
        next: (resp: GastosListResponse) => {
          this.gastos = resp?.gastos ?? [];
          this.totalBackend = Number(resp?.total ?? 0);
          this.registrosBackend = Number(resp?.registros ?? this.gastos.length);
        },
        error: () => {
          this.gastos = [];
          this.totalBackend = 0;
          this.registrosBackend = 0;
        },
      });
  }

  loadCategorias() {
    this.api.getCategorias().subscribe({
      next: (data: CategoriaDto[]) => {
        this.categorias = data ?? [];
        this.cdr.detectChanges();
      },
      error: () => {
        this.categorias = [];
        this.cdr.detectChanges();
      },
    });
  }

  handleError(e: HttpErrorResponse) {
    const msg =
      (typeof e.error === 'string' ? e.error : e.error?.message) ||
      e.message ||
      'Error inesperado';

    this.errorMessage = msg;
    this.submitting = false;
    this.cdr.detectChanges();
  }
}