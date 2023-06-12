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

  export interface CommentBasic {
    body: string;
    articleId: number;
  }