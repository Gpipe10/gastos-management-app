import { Routes } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';

import { CategoriaComponent } from './pages/categoria/categoria.component';
import { GastosComponent } from './pages/gastos/gastos.component';


export const routes: Routes = [
  { path: '', redirectTo: 'app/gastos', pathMatch: 'full' },

  {
    path: 'app',
    component: LayoutComponent,
    children: [
   
      { path: 'categoria', component: CategoriaComponent },
  
      { path: 'gastos', component: GastosComponent },
     
    ],
  },

  { path: '**', redirectTo: 'app/gastos' }
];
