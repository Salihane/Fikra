import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Task } from '../models/task';

@Injectable()
export class TaskApiService {
  constructor(private http: HttpClient) { }
  taskApiUrl: string = 'https://localhost:44317/api/tasks';

  saveTask(task: Task) {
    this.http.post(this.taskApiUrl, task)
      .subscribe(res => {
        console.log(res);
      });
  }
}
