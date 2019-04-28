import { Dashboard } from './../models/dashboard';
import { Component, Input } from '@angular/core';
import { DashboardApiService } from '../services/dashboard.api.service';

@Component({
    selector: 'dashboard',
    templateUrl: './dashboard.component.html'
})
export class DashboardComponent {

    @Input()
    dashboard: Dashboard;

    constructor(
        private dashboardApi: DashboardApiService
    ) { }

    ngOnInit(){
        this.dashboard = new Dashboard();
        this.dashboardApi.dashboardSelected.subscribe(dashboard => this.dashboard = dashboard);
    }

    private save() {
        this.dashboardApi.saveDashboard(this.dashboard);
        this.reset();
    }

    private update(){
        this.dashboardApi.updateDashboard(this.dashboard);
    }

    private reset(){
        this.dashboard = new Dashboard();
    }
}