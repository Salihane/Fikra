import { Component, Inject } from '@angular/core';
import { DashboardApiService } from '../services/dashboard.api.service';
import { ActivatedRoute } from '@angular/router';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

export interface DialogData {
    animal: string;
    name: string;
}

@Component({
    selector: 'tasks',
    templateUrl: './dashboardtasks.component.html'
})
export class DashboardTasksComponent {
    tasks;
    animal: string;
    name: string;
    constructor(
        private dashboardApi: DashboardApiService,
        private route: ActivatedRoute,
        public dialog: MatDialog) {
    }

    ngOnInit() {
        this.loadTasks();
    }

    private loadTasks() {
        let dashboardId = +this.route.snapshot.paramMap.get('id');
        this.dashboardApi.loadDashboardTasks(dashboardId)
            .subscribe(res => {
                this.tasks = res;
            });
    }

    openDialog(): void {
        const dialogRef = this.dialog.open(AddDashboardTaskDialog, {
          width: '400px',
          data: {name: this.name, animal: this.animal}
        });
    
        dialogRef.afterClosed().subscribe(result => {
          console.log('The dialog was closed');
          this.animal = result;
        });
      }
}
@Component({
    templateUrl: './add-dashboardtask.dialog.html'
  })
  export class AddDashboardTaskDialog {
  
    constructor(
      public dialogRef: MatDialogRef<AddDashboardTaskDialog>,
      @Inject(MAT_DIALOG_DATA) public data: DialogData) {}
  
    onNoClick(): void {
      this.dialogRef.close();
    }
  
  }