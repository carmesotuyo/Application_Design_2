export class Article {
    id?: number;
    name: string;
    username?: string;
    body: string;
    private: boolean;
    template: number;
    image?: string;

    constructor(name: string, body: string, isPrivate: boolean, template: number, image: string, id: number, username: string) {
        this.id = id;
        this.name = name;
        this.username = username;
        this.body = body;
        this.private = isPrivate;
        this.template = template;
        this.image = image;
    }
  }