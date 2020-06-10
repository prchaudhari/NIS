import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ListComponent } from './list/list.component';
import { ViewComponent } from './view/view.component';
import { AddComponent } from './add/add.component';


const routes: Routes = [
    {
        path: '',
        children: [
            { path: '', component: ListComponent },
            { path: 'list', component: ListComponent },
            { path: 'View', component: ViewComponent },
            { path: 'Add', component: AddComponent },
            { path: 'List', component: ListComponent }
        ]
    }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TemplateRoutingModule { }
