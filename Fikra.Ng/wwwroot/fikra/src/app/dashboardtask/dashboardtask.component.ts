import { DashboardTask } from './../models/dashboardtask';
import { Component, Input } from '@angular/core';
import { FormControl } from '@angular/forms';
import { TaskApiService } from '../services/task.api.service';
import { DateFormatterService } from '../services/date.formatter.service';

@Component({
  selector: 'dashboardtask',
  templateUrl: './dashboardtask.component.html'
})
export class DashboardTaskComponent {
  duedate;

  @Input()
  task: DashboardTask;

  constructor(
    private taskApi: TaskApiService,
    private dateFormatter: DateFormatterService) { }

  ngOnInit() {
    this.duedate = new FormControl(new Date());
    this.task = new DashboardTask();
    this.taskApi.taskSelected.subscribe(task => this.task = task);
  }

  private save(): void {
    this.taskApi.saveTask(this.task);
    this.reset();
  }

  private update(): void {
    this.taskApi.updateTask(this.task);
  }

  private reset(): void {
    this.task = new DashboardTask();
  }
}
