import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';import { HttpErrorResponse } from '@angular/common/http';

import { CategoriaApiService, CategoriaDto } from '../../services/categoria-api.service';

@Component({
  selector: 'app-categoria',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './categoria.component.html',
 styleUrls: ['./categoria.component.css'],
})
export class CategoriaComponent implements OnInit {
constructor(private api: CategoriaApiService, private cdr: ChangeDetectorRef) {}

  categorias: CategoriaDto[] = [];
  categoriaNombre = '';

  loading = false;
  submitting = false;

  errorMessage = '';
  successMessage = '';

  ngOnInit(): void {
    this.loadCategorias();
  }

  loadCategorias() {
    this.loading = true;
    this.api.getCategorias().subscribe({
        next: (data) => {
        this.categorias = data ?? [];
        this.loading = false;
        this.cdr.detectChanges(); // ✅ fuerza render
      },
      error: (e: HttpErrorResponse) => {
  this.loading = false;
  this.errorMessage = e.error?.message || e.message || 'Error cargando categorías.';
  this.cdr.detectChanges(); // ✅ fuerza render
},
    });
  }

  crearCategoria() {
    const nombre = this.categoriaNombre.trim();
    if (!nombre) return;

    this.errorMessage = '';
    this.successMessage = '';
    this.submitting = true;

    this.api.createCategoria({ nombre }).subscribe({
      next: () => {
        this.categoriaNombre = '';
        this.successMessage = 'Categoría creada.';
        this.submitting = false;
        this.loadCategorias();
      },
      error: (e: HttpErrorResponse) => {
        this.submitting = false;
        this.errorMessage = e.error?.message || e.message || 'No se pudo crear la categoría.';
      },
    });
  }

  eliminarCategoria(id: number) {
    this.errorMessage = '';
    this.successMessage = '';

    this.api.deleteCategoria(id).subscribe({
      next: () => {
        this.successMessage = 'Categoría eliminada.';
        this.loadCategorias();
      },
      error: (e: HttpErrorResponse) => {
        this.errorMessage = e.error?.message || e.message || 'No se pudo eliminar la categoría.';
      },
    });
  }
}