import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, of, take } from 'rxjs';
import { ICreateArticle } from 'src/app/interfaces/create-article.interface';
import { Article } from 'src/app/models/article.model';
import { ArticleService } from 'src/app/services/article.service';
import { ValidateString } from 'src/app/validators/string.validator';

@Component({
  selector: 'app-article-form',
  templateUrl: './article-form.component.html',
  styleUrls: ['./article-form.component.scss'],
})
export class ArticleFormComponent implements OnInit {
  mostrarImg2 = false;
  public articleForm = new FormGroup({
    name: new FormControl('', [Validators.required, ValidateString]),
    body: new FormControl('', [Validators.required, ValidateString]),
    isPrivate: new FormControl(false, Validators.required),
    template: new FormControl(0, Validators.required),
    image1: new FormControl(''),
    image2: new FormControl('')
  });

  public isEditing = false;
  private articleId?: number;

  constructor(
    private articleService: ArticleService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  public get name() {
    return this.articleForm.get('name');
  }

  public get body() {
    return this.articleForm.get('body');
  }

  public get isPrivate() {
    return this.articleForm.get('isPrivate');
  }

  public get template() {
    return this.articleForm.get('template');
  }

  public get image1() {
    return this.articleForm.get('image1');
  }

  public get image2() {
    return this.articleForm.get('image2');
  }

  public ngOnInit(): void {
    //const id = this.route.snapshot.params?.['id'];
    const id = 15;
    if (!!id) {
      this.isEditing = true;
      this.articleId = id;
      this.articleService
        .getArticle(id)
        .pipe(
          take(1),
          catchError((err) => {
            console.log({ err });
            return of(err);
          })
        )
        .subscribe((article: Article) => {
          this.setArticle(article);
        });
    }
    this.template?.valueChanges.subscribe((value) => {
      if (Number(value) === 3) {
        console.log('template cambió a plantilla 3')
        this.mostrarImg2 = true;
      } else {
        this.mostrarImg2 = false;
      }
    });
  }

  public submitArticle(): void {
    if (this.articleForm.valid) {
      if (this.isEditing) {
        this.updateArticle();
      } else {
        this.createArticle();
      }
    }
  }

  private createArticle(): void {
    const image = `${this.articleForm.value.image1 ?? ''} ${this.articleForm.value.image2 ?? ''}`
    const article: Article = {
      id: 0,
      name: this.articleForm.value.name ?? '',
      username: '',
      body: this.articleForm.value.body ?? '',
      private: this.articleForm.value.isPrivate ?? false,
      template: this.articleForm.value.template ?? 0,
      image: image,
    };


    this.articleService
      .postArticle(article)
      .pipe(
        take(1),
        catchError((err) => {
          console.log({ err });
          return of(err);
        })
      )
      .subscribe((article: Article) => {
        if (!!article?.id) {
          alert('Artículo creado!!');
          this.cleanForm();
          this.router.navigateByUrl('/home');
        }
      });
  }

  private updateArticle(): void {
    if (!!this.articleId) {
      const image = `${this.articleForm.value.image1 ?? ''} ${this.articleForm.value.image2 ?? ''}`
      const article: Article = {
        username: 'FerSpi',
        id: this.articleId as number,
        name: this.articleForm.value.name ?? '',
        body: this.articleForm.value.body ?? '',
        private: this.articleForm.value.isPrivate ?? false,
        template: this.articleForm.value.template ?? 0,
        image: image,
      };

      this.articleService
        .putArticle(article)
        .pipe(
          take(1),
          catchError((err) => {
            console.log({ err });
            return of(err);
          })
        )
        .subscribe((article: Article) => {
          if (!!article?.id) {
            alert('Artículo modificado!!');
            this.router.navigateByUrl('/home');
          }
        });
    }
  }

  private setArticle(article: Article): void {
    const image = article.image?.split(' ') as string[] | undefined;
    this.name?.setValue(article.name != null ? String(article.name) : '');
    this.body?.setValue(article.body != null ? String(article.body) : '');
    this.isPrivate?.setValue(article.private != null ? article.private : false);
    this.template?.setValue(article.template != null ? article.template : 0);
    this.image1?.setValue(image != null ? String(image[0]) : '');
    this.image2?.setValue(image != null ? String(image[1]) : '');
  }

  private cleanForm(): void {
    this.name?.setValue('');
    this.body?.setValue('');
    this.isPrivate?.setValue(true);
    this.template?.setValue(0);
    this.image1?.setValue('');
    this.image2?.setValue('');
  }
}
