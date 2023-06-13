export class User {
  username: string;
  password: string;
  email: string;
  name: string;
  lastName: string;
  blogger: boolean;
  admin: boolean;
  moderador: boolean;

  constructor(
    username: string,
    password: string,
    email: string,
    name: string,
    lastName: string
  ) {
    this.username = username;
    this.password = password;
    this.email = email;
    this.name = name;
    this.lastName = lastName;
    this.blogger = true;
    this.admin = false;
    this.moderador = false;
  }
}

export class UserRanking {
  id: number;
  username: string;

  constructor(id: number, username: string) {
    this.id = id;
    this.username = username;
  }
}

export class UserComplete {
  id: number;
  username: string;
  password: string;
  email: string;
  name: string;
  lastName: string;
  blogger: boolean;
  admin: boolean;
  moderador: boolean;
  dateDeleted: string;

  constructor(
    id: number,
    username: string,
    password: string,
    email: string,
    name: string,
    lastName: string,
    blogger: boolean,
    admin: boolean,
    moderador: boolean,
    dateDeleted: string
  ) {
    this.id = id;
    this.username = username;
    this.password = password;
    this.email = email;
    this.name = name;
    this.lastName = lastName;
    this.blogger = blogger;
    this.admin = admin;
    this.moderador = moderador;
    this.dateDeleted = dateDeleted;
  }
}
