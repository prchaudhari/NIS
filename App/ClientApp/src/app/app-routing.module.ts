import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';

const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: 'login', component: LoginComponent },
    { path: 'layout', loadChildren: () => import('../app/layout/layout.module').then(m => m.LayoutModule) },
    { path: 'dashboardDesigner', loadChildren: () => import('../app/dashboard-designer/dashboard-designer.module').then(m => m.DashboardDesignerModule) },

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
