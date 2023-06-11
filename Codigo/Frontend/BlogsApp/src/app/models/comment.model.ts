export interface CommentDto {
    id: number;
    user: {
      id: number;
      username: string;
    };
    articleId: number;
    body: string;
    dateCreated: string;
    dateDeleted: string | null;
    subComments: CommentDto[];
  }