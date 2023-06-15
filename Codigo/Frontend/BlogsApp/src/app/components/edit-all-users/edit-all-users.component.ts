import { Component, OnInit } from '@angular/core';
import { User, UserComplete } from 'src/app/models/user.model';
import { UserService } from 'src/app/services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ToastrService } from 'ngx-toastr';

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
  isEditingUser: boolean = false;

  constructor(private userService: UserService, private snackBar: MatSnackBar, private toastr: ToastrService) { }

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
      this.newUser = new User('', '', '', '', '');
      this.toastr.success('Usuario añadido con éxito', 'Añadir usuario');
    });
  }

  editUser(userId: number): void {
    this.userService.getUserById(userId).subscribe((user: User) => {
      this.isEditingUser = true;
      this.editedUser = user;
      this.editingUserId = userId;
    });
  }

  saveUserChanges(): void {
    this.userService.updateUser(this.editedUser, this.editingUserId).subscribe((user: User) => {
      this.getUsers();
      this.editedUser = new User('', '', '', '', '');
      this.editingUserId = 0;
      this.toastr.success('Cambios guardados con éxito', 'Guardar cambios');
    });
  }

  deleteUser(userId: number): void {
    this.userService.deleteUser(userId).subscribe(() => {
      this.getUsers();
      this.toastr.success('Usuario eliminado con éxito');
    });
  }
}
