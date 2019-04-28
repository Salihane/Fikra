export class Effort{
    id: string;
    createdon: Date;
    modifiedon: Date;
    estimated: number;
    remaining: number;
    completed: number;

    constructor(){
        this.createdon = new Date();
        this.modifiedon = new Date();
        this.estimated = 0;
        this.remaining = 0;
        this.completed = 0;
    }
}