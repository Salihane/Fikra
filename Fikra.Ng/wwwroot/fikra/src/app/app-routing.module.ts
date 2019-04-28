import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { DashboardsComponent } from "./dashboards/dashboards.component";
import { HomeComponent } from './home/home.component';
import { TaskComponent } from './task/task.component';
import { DashboardTasksComponent } from './dashboardtasks/dashboardtasks.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'dashboard/:id/tasks', component: DashboardTasksComponent },
  { path: 'dashboards', component: DashboardsComponent },
  { path: 'task', component: TaskComponent },
  { path: 'tasks', component: DashboardTasksComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
