export class User {
    username: string;
    password: string;
    email: string;
    name: string;
    lastName: string;
    blogger: boolean;
    admin: boolean;

    constructor(username: string, password: string, email: string, name: string, lastName: string, blogger: boolean, admin: boolean) {
        this.username = username;
        this.password = password;
        this.email = email;
        this.name = name;
        this.lastName = lastName;
        this.blogger = blogger;
        this.admin = admin;
    }
}