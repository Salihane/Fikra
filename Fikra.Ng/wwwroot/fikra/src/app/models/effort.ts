export class Effort{
    Id: string;
    CreatedOn: Date;
    ModifiedOn: Date;
    Estimated: number;
    Remaining: number;
    Completed: number;

    constructor(){
        this.CreatedOn = new Date();
        this.ModifiedOn = new Date();
        this.Estimated = 0;
        this.Remaining = 0;
        this.Completed = 0;
    }
}