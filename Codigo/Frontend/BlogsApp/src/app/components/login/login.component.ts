import { Component } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { CommentService } from 'src/app/services/comment.service';

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

    //Si hay una sesion activa que vaya al home
    if(this.authService.isAuthenticated()) {
      //this.router.navigateByUrl('/home');
    }


  }

  constructor(private loginService: LoginService, private router: Router, private formBuilder: FormBuilder, private authService: AuthService, private commentService: CommentService) {}

  login() {
    this.loginForm = this.formBuilder.group({
      username: [this.username, Validators.required],
      password: [this.password, Validators.required]
    });
    this.loginService.login(this.username, this.password).subscribe(
      (response) => {
        if (response.token) {
          // Almacenar el token en el almacenamiento local o en una cookie si es necesario
          const token = response.token;
          this.authService.setToken(token);
          this.authService.setUsername(this.username);
          this.commentService.setOfflineComments(response.comments);
          // Redirigir al usuario a la pantalla principal (HomeComponent) u otra vista deseada
          this.router.navigateByUrl('/home');
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
