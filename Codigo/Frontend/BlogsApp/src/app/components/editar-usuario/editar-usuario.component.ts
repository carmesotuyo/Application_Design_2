import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { UserService } from 'src/app/services/user.service';
import { User, UserComplete } from 'src/app/models/user.model';

@Component({
  selector: 'app-editar-usuario',
  templateUrl: './editar-usuario.component.html',
  styleUrls: ['./editar-usuario.component.scss'],
})
export class EditarUsuarioComponent implements OnInit {
  editUserForm: FormGroup;
  user: User = new User('', '', '', '', '');
  id: number = 0;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private userService: UserService
  ) {
    this.editUserForm = this.fb.group({
      name: ['', Validators.required],
      lastName: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.id = parseInt(this.authService.getUserId()!);
    this.getUser();
    
  }

  getUser(): void {
    this.userService.getUserById(this.id).subscribe((user) => {
      this.user = user;
      this.editUserForm.patchValue({
        name: user.name,
        lastName: user.lastName,
        // No set password for security reasons
      });
    });
  }

  saveChanges(): void {
    if (this.editUserForm.valid) {
      this.userService
        .updateUser(this.editUserForm.value, this.id)
        .subscribe(() => {
          alert('Changes saved successfully');
        });
    }
  }
}
