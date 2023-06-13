import { Component, OnInit } from '@angular/core';
import { User, UserComplete } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-edit-all-users',
  templateUrl: './edit-all-users.component.html',
  styleUrls: ['./edit-all-users.component.scss']
})
export class EditAllUsersComponent implements OnInit {
  users!: UserComplete[];
  newUser: User = new User('', '', '', '', '');
  editedUser: User = new User('', '', '', '', '');
  editingUserId: number = 0;
  isCreatingUser: boolean = false;

  constructor(private userService: UserService, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers(): void {
    this.userService.getUsers().subscribe((users: UserComplete[]) => {
      this.users = users;
    });
  }

  addUser(): void {
    this.userService.postUser(this.newUser).subscribe((user: User) => {
      this.getUsers();
      //this.users.push(user);
      this.newUser = new User('', '', '', '', '');
      this.snackBar.open('Usuario añadido con éxito', 'Cerrar', { duration: 2000 });
    });
  }

  editUser(userId: number): void {
    this.userService.getUserById(userId).subscribe((user: User) => {
      this.editedUser = user;
      this.editingUserId = userId;
    });
  }

  saveUserChanges(): void {
    this.userService.updateUser(this.editedUser, this.editingUserId).subscribe((user: User) => {
      // const index = this.users.findIndex(u => u.username === user.username);
      // this.users[index] = user;
      this.getUsers();
      this.editedUser = new User('', '', '', '', '');
      this.editingUserId = 0;
      this.snackBar.open('Cambios guardados con éxito', 'Cerrar', { duration: 2000 });
    });
  }

  deleteUser(userId: number): void {
    this.userService.deleteUser(userId).subscribe(() => {
      this.getUsers();
      //this.users = this.users.filter(user => user.username !== userId);
      this.snackBar.open('Usuario eliminado con éxito', 'Cerrar', { duration: 2000 });
    });
  }
}
