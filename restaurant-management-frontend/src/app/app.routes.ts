import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth/auth-guard';

export const routes: Routes = [
    {
        path: '',
        canActivate: [authGuard],
        data: { authRequired: false, roles: [] },
        loadChildren: () =>
            import('./pages/landing/landing-page.routes').then(
                (m) => m.LANDING_PAGE_ROUTES
            ),
    },
    {
        path: 'auth',
        canActivate: [authGuard],
        data: { authRequired: false },
        loadChildren: () =>
            import('./pages/auth/auth.routes').then((m) => m.AUTH_ROUTES),
    },
];
