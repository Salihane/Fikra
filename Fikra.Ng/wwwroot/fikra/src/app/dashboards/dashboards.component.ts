import { DashboardApiService } from './../services/dashboard.api.service';
import { Component } from '@angular/core';

@Component({
    selector: 'dashboards',
    templateUrl: './dashboards.component.html'
})
export class DashboardsComponent {
    dashboards;

    constructor(private dashboardApi: DashboardApiService) { }

    ngOnInit(){
        this.loadDashboards();
    }

    private loadDashboards() {
        this.dashboardApi.loadDashboards()
        .subscribe(res => {
            this.dashboards = res;
        });
    }
}