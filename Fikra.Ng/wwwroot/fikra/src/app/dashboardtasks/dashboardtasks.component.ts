import { Component } from '@angular/core';
import { DashboardApiService } from '../services/dashboard.api.service';

@Component({
    selector: 'tasks',
    templateUrl: './dashboardtasks.component.html'
})
export class DashboardTasksComponent{
    tasks;
    
    constructor(private dashboardApi: DashboardApiService){
    }

    ngOnInit(){
        this.loadTasks();
    }

    private loadTasks(){
        let dashboardId = 1;
       this.dashboardApi.loadDashboardTasks(dashboardId)
       .subscribe(res => {
           this.tasks = res;
       });
    }

}