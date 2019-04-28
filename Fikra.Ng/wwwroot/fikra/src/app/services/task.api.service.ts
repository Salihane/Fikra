import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DashboardTask } from '../models/dashboardtask';
import { Subject } from 'rxjs';

@Injectable()
export class TaskApiService {
    private selectedTask: Subject<DashboardTask> = new Subject<DashboardTask>();
    taskSelected = this.selectedTask.asObservable();

    private taskApiUrl: string = 'https://localhost:44317/api/tasks';

    constructor(private http: HttpClient) { }

    saveTask(task: DashboardTask) {
        this.http.post(this.taskApiUrl, task)
            .subscribe(res => {
                console.log(res);
            });
    }

    updateTask(task: DashboardTask){
        let taskUpdateUrl:string = `${this.taskApiUrl}/${task.id}`;
        this.http.put(taskUpdateUrl, task)
        .subscribe(res => {
            console.log(res);
        });
    }

    loadTasks() {
        return this.http.get(this.taskApiUrl);
    }

    selectTask(task: DashboardTask){
        this.selectedTask.next(task);
    }
}
