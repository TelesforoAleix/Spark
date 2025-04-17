// App configuration for standalone Angular project
// Includes routing and HTTP client setup

import { ApplicationConfig } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideHttpClient } from '@angular/common/http'; // Allows HttpClient to work in services

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient() // Injects Angular's HttpClient globally
  ]
};
