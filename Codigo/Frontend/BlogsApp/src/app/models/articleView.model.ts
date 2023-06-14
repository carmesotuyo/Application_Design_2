import { CommentDto } from './comment.model';

export interface ArticleView {
  id: number;
  name: string;
  username: string;
  body: string;
  commentsDtos: CommentDto[] | null;
  private: boolean;
  template: number;
  image: string;
}