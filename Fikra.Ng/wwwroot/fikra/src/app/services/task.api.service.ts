import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Task } from '../models/task';
import { Subject } from 'rxjs';

@Injectable()
export class TaskApiService {
    private selectedTask: Subject<Task> = new Subject<Task>();
    taskSelected = this.selectedTask.asObservable();

    private taskApiUrl: string = 'https://localhost:44317/api/tasks';

    constructor(private http: HttpClient) { }

    saveTask(task: Task) {
        this.http.post(this.taskApiUrl, task)
            .subscribe(res => {
                console.log(res);
            });
    }

    updateTask(task: Task){
        let taskUpdateUrl:string = `${this.taskApiUrl}/${task.id}`;
        this.http.put(taskUpdateUrl, task)
        .subscribe(res => {
            console.log(res);
        });
    }

    loadTasks() {
        return this.http.get(this.taskApiUrl);
    }

    selectTask(task: Task){
        this.selectedTask.next(task);
    }
}
