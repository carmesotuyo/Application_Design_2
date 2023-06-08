import { Component } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  loginForm: FormGroup = new FormGroup({});

  ngOnInit() {
    // Create the loginForm with formBuilder
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  constructor(private loginService: LoginService, private router: Router, private formBuilder: FormBuilder) {}

  login() {
    this.loginForm = this.formBuilder.group({
      username: [this.username, Validators.required],
      password: [this.password, Validators.required]
    });
    this.loginService.login(this.username, this.password).subscribe(
      (response) => {
        if (response.token) {
          // Almacenar el token en el almacenamiento local o en una cookie si es necesario
  
          // Redirigir al usuario a la pantalla principal (HomeComponent) u otra vista deseada
        } else {
          console.log('Inicio de sesión fallido:', response.message);
        }
      },
      (error) => {
        console.log('Error en la autenticación:', error);
      }
    );
  }
}
