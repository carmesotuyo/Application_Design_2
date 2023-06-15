import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CommentService } from 'src/app/services/comment.service';
import { OffensivewordsService } from 'src/app/services/offensivewords.service';
import { ArticleService } from 'src/app/services/article.service';
import { Article } from 'src/app/models/article.model';
import { Content } from 'src/app/models/content.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-content-to-revision',
  templateUrl: './content-to-revision.component.html',
  styleUrls: ['./content-to-revision.component.scss'],
})
export class ContentToRevisionComponent implements OnInit {
  contents: Content[] = [];
  form!: FormGroup;
  editMode: boolean[] = [];
  articleToEdit: Article = new Article('', '', false, 0, '');
  editingContentId: number = 0;

  constructor(
    private offensivewordsService: OffensivewordsService,
    private commentService: CommentService,
    private articleService: ArticleService,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    this.getContents();
    this.form = new FormGroup({
      title: new FormControl(''),
      body: new FormControl('', Validators.required),
    });
  }

  getContents() {
    this.offensivewordsService.getContentToRevision().subscribe((contents) => {
      this.contents = contents;
      this.editMode = new Array(this.contents.length).fill(false);
    });
  }

  editContent(index: number, contentId: number) {
    this.editMode[index] = true;
    this.editingContentId = contentId;
    if (this.contents[index].type === 0) {
      (this.form.controls['title'] as FormControl).setValue(
        this.contents[index].articleName
      );
    }
    (this.form.controls['body'] as FormControl).setValue(
      this.contents[index].body
    );
  }

  saveContent(index: number) {
    if (this.form.valid) {
      const updatedContent = {
        id: this.contents[index].id,
        body: this.form.value.body,
        articleName:
          this.contents[index].type === 0 ? this.form.value.title : null,
      };
      if (this.contents[index].type === 0) {
        this.articleService
          .getArticle(this.contents[index].id)
          .subscribe((article) => {
            this.articleToEdit = article;
            this.articleToEdit.body = this.form.value.body;
            this.articleToEdit.name = this.form.value.title;
            this.articleService.putArticle(this.articleToEdit).subscribe(() => {
              this.editMode[index] = false;
              this.getContents();
              this.toastr.success('Artículo actualizado con éxito', 'Éxito');
            }, error => {
              this.toastr.error(error.error, 'Error');
            });
          });
      } else {
        this.commentService
          .updateComment(updatedContent.body, updatedContent.id)
          .subscribe(() => {
            this.editMode[index] = false;
            this.getContents();
            this.toastr.success('Comentario actualizado con éxito', 'Éxito');
          }, error => {
            this.toastr.error(error.error, 'Error');
          });
      }
    }
  }

  deleteContent(index: number) {
    if (this.contents[index].type === 0) {
      this.articleService
        .deleteArticle(this.contents[index].id)
        .subscribe(() => {
          this.toastr.success('Artículo eliminado con éxito', 'Éxito');
          this.getContents();
        });
    } else {
      this.commentService
        .deleteComment(this.contents[index].id)
        .subscribe(() => {
          this.toastr.success('Comentario eliminado con éxito', 'Éxito');
          this.getContents();
        });
    }
  }

  approveContent(index: number) {
    if (this.contents[index].type === 0) {
      this.articleService
        .aproveArticle(this.contents[index].id)
        .subscribe(() => {
          this.toastr.success('Artículo aprobado con éxito', 'Éxito');
          this.getContents();
          console.log('aproveArticle');
        });
    } else {
      this.commentService
        .aproveComment(this.contents[index].id)
        .subscribe(() => {
          this.toastr.success('Comentario aprobado con éxito', 'Éxito');
          this.getContents();
        });
    }
  }
}
