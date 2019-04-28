import { Component } from '@angular/core';
import { Task } from '../models/task';
import { TaskApiService } from '../services/task.api.service';
import { Observable } from 'rxjs';

@Component({
    selector: 'tasks',
    templateUrl: './tasks.component.html'
})
export class TasksComponent{
    tasks;
    
    constructor(private taskApi: TaskApiService){
    }

    ngOnInit(){
        this.loadTasks();
    }

    private loadTasks(){
       this.taskApi.loadTasks()
       .subscribe(res => {
           this.tasks = res;
       });
    }

}