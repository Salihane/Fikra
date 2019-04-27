import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { TaskApiService } from '../services/task.api.service';
import { DateFormatterService } from '../services/date.formatter.service';
import { Task } from '../models/task';

@Component({
  selector: 'task',
  templateUrl: './task.component.html'
})
export class TaskComponent {
  duedate = new FormControl(new Date());
  task:Task = new Task;

  constructor(
    private taskApi: TaskApiService,
    private dateFormatter: DateFormatterService) {
  }

  private save(): void {
    this.task.Due = this.dateFormatter.formatDate(this.duedate.value);
    this.taskApi.saveTask(this.task);
  }
}
