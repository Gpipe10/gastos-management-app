import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { routes } from './app.routes';
import { API_URL } from './config/api.config';


export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(),

    // 🔹 API base URL (backend .NET)
    { provide: API_URL, useValue: 'https://localhost:7001' }
  ]
};
