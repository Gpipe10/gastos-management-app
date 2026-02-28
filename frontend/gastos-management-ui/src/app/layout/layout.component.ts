import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from '../shared/sidebar/sidebar.component';
import { TopbarComponent } from '../shared/topbar/topbar.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterOutlet, SidebarComponent, TopbarComponent],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent {
  sidebarCollapsed = true;

  private readonly STORAGE_KEY = 'pm_sidebar_collapsed';

  constructor() {
    const saved = localStorage.getItem(this.STORAGE_KEY);
    if (saved !== null) this.sidebarCollapsed = saved === 'true';
  }

  toggleSidebar() {
    this.sidebarCollapsed = !this.sidebarCollapsed;
    localStorage.setItem(this.STORAGE_KEY, String(this.sidebarCollapsed));
  }
}
