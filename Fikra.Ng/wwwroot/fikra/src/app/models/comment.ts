export class Comment{
    id: string;
    createdon: Date;
    modifiedon: Date;
    content: string;

    constructor(){
        this.createdon = new Date();
        this.modifiedon = new Date();
        this.content = '';
    }
}