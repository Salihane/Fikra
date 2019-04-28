import { Task } from './task';
import { Comment } from './comment';

export class Dashboard{
    id: string;
    createdon: Date;
    modifiedon: Date;
    name: string;
    tasks: Array<Task>;
    comments: Array<Comment>;

    constructor(){
        this.createdon = new Date();
        this.modifiedon = new Date();
        this.name = 'New Dashboard';
        this.tasks = [];
        this.comments = [];
    }
}