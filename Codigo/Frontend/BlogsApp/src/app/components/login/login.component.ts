import { Component } from '@angular/core';
import { LoginService } from '../../services/login.service';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { CommentService } from 'src/app/services/comment.service';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/app/models/user.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  loginForm: FormGroup = new FormGroup({});
  user: User = new User('', '', '', '', '');
  

  ngOnInit() {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
    if (this.authService.isAuthenticated()) {
    }
  }

  constructor(
    private loginService: LoginService,
    private router: Router,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private commentService: CommentService,
    private userService: UserService,
    private toastr: ToastrService,
  ) {}

  login() {
    this.loginForm = this.formBuilder.group({
      username: [this.username, Validators.required],
      password: [this.password, Validators.required],
    });
    this.loginService.login(this.username, this.password).subscribe(
      (response) => {
        if (response.token) {
          const token = response.token;
          this.authService.setToken(token);
          this.authService.setUsername(this.username);
          this.commentService.setOfflineComments(response.comments);
          this.authService.setUserId(response.userId);
          this.getUserById(response.userId);
          this.toastr.success('Has iniciado sesión', 'Éxito');
        } else {
          console.log('Inicio de sesión fallido:', response.message);
          this.toastr.error(response.message, 'Error');
        }
      },
      (error) => {
        console.log('Error en la autenticación:', error);
        if (error.error) {
          this.toastr.error(error.error, 'Error');
        } else {
          this.toastr.error('Ha ocurrido un error desconocido durante la autenticación', 'Error');
        }
      }
    );
  }

  getUserById(id: number) {
    this.userService.getUserById(id).subscribe((response: User) => {
      this.user = response;
      this.authService.setAdmin(this.user.admin);
      this.authService.setBlogger(this.user.blogger);
      this.authService.setModerator(this.user.moderador);
      this.router.navigateByUrl('/home');
    });
  }
}
