import { Effort } from './effort';
import { Priority } from './priority';
import { Status } from './status';
import { Comment } from './comment';

export class DashboardTask {
    id: string;
    createdon: Date;
    modifiedon: Date;
    due: Date;
    name: string;
    status: Status;
    priority: Priority;
    effort: Effort;
    comments: Array<Comment>;

    constructor() {
        this.createdon = new Date();
        this.modifiedon = new Date();
        this.due = new Date();
        this.name = 'New Task';
        this.status = Status.New;
        this.priority = Priority.Low;
        this.effort = null;
        this.comments = [];
    }
}