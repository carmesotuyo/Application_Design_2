export class Content {
    id: number;
    type: number;
    body: string;
    articleName: string;
    offensiveWords: string[];

    constructor(id: number, type: number, body: string, articleName: string, offensiveWords: string[]) {
        this.id = id;
        this.type = type;
        this.body = body;
        this.articleName = articleName;
        this.offensiveWords = offensiveWords;
    }
}