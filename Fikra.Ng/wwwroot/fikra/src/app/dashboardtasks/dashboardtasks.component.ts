import { Component } from '@angular/core';
import { DashboardApiService } from '../services/dashboard.api.service';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'tasks',
    templateUrl: './dashboardtasks.component.html'
})
export class DashboardTasksComponent{
    tasks;
    
    constructor(
        private dashboardApi: DashboardApiService,
        private route: ActivatedRoute){
    }

    ngOnInit(){
        this.loadTasks();
    }

    private loadTasks(){
        let dashboardId = +this.route.snapshot.paramMap.get('id');
       this.dashboardApi.loadDashboardTasks(dashboardId)
       .subscribe(res => {
           this.tasks = res;
       });
    }

}