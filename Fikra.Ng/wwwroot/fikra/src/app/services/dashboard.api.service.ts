import { Dashboard } from './../models/dashboard';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Subject } from 'rxjs';

@Injectable()
export class DashboardApiService {
    private selectedDashboard: Subject<Dashboard> = new Subject<Dashboard>();
    dashboardSelected = this.selectedDashboard.asObservable();
    
    private dashboardApiUrl: string = 'https://localhost:44317/api/dashboards';

    constructor(private http: HttpClient) { }

    saveDashboard(dashboard: Dashboard){
        this.http.post(this.dashboardApiUrl, dashboard)
        .subscribe(res => {
            console.log(res);
        });
    }

    updateDashboard(dashboard: Dashboard){
        let dashboardUrl: string = `${this.dashboardApiUrl}/${dashboard.id}`;
        this.http.put(dashboardUrl, dashboard)
        .subscribe(res => {
            console.log(res);
        });
    }

    loadDashboards(){
        return this.http.get(this.dashboardApiUrl);
    }

    selectDashboard(dashboard: Dashboard){
        this.selectedDashboard.next(dashboard);
    }

    loadDashboardTasks(dashboardId: number){
        let dashboardTasksUrl: string = `${this.dashboardApiUrl}/${dashboardId}/tasks`;
        return this.http.get(dashboardTasksUrl);
    }
}