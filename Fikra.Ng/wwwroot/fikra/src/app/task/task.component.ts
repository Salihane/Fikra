import { Component } from '@angular/core';
import { TaskApiService } from '../services/task.api.service';

@Component({
    selector: 'task',
    templateUrl: './task.component.html'
})
export class TaskComponent {
    task = {};

    constructor(private taskApi: TaskApiService) { }

    private save(): void {
        this.taskApi.saveTask(this.task);
    }
}