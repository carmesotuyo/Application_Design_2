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
    state: number;
  }

  export interface CommentBasic {
    body: string;
    articleId: number;
  }

  export interface CommentContent {
    body: string;
  }

  export interface CommentNotify {
    body: string;
    articleId: number;
    commentId: number;
    reply: string;
  }