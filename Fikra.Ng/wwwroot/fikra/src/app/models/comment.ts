export class Comment{
    Id: string;
    CreatedOn: Date;
    ModifiedOn: Date;
    Content: string;

    constructor(){
        this.CreatedOn = new Date();
        this.ModifiedOn = new Date();
        this.Content = '';
    }
}