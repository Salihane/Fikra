import { Effort } from './effort';
import { Priority } from './priority';
import { Status } from './status';
import { Comment } from './comment';

export class Task {
    Id: string;
    CreatedOn: Date;
    ModifiedOn: Date;
    Due: Date;
    Name: string;
    Status: Status;
    Priority: Priority;
    Effort: Effort;
    Comments: Array<Comment>;

    constructor() {
        this.CreatedOn = new Date();
        this.ModifiedOn = new Date();
        this.Due = null;
        this.Name = 'New Task';
        this.Status = Status.New;
        this.Priority = Priority.Low;
        this.Effort = null;
        this.Comments = [];
    }
}