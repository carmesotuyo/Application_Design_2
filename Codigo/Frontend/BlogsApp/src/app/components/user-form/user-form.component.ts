import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { User } from 'src/app/models/user.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-form',
  templateUrl: './user-form.component.html',
  styleUrls: ['./user-form.component.scss']
})
export class UserFormComponent implements OnInit {
  registerForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, private userService: UserService, private router: Router) { }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      name: ['', Validators.required],
      lastName: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      const newUser = new User(this.registerForm.value.username, this.registerForm.value.password, this.registerForm.value.email, this.registerForm.value.name, this.registerForm.value.lastName);
      this.userService.postUser(newUser).subscribe(
        response => console.log('User created!', response),
        error => console.error('There was an error!', error)
      );
      this.registerForm.reset();
      this.router.navigate(['/login']);
    }
  }
}
