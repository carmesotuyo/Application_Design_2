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

  export interface CommentNotify {
    body: string;
    articleId: number;
    commentId: number;
    reply: string;
  }