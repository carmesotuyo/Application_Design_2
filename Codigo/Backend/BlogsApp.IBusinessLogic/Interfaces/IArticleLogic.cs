using BlogsApp.Domain.Entities;
namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IArticleLogic
    {
        Article GetArticleById(int id);
    }
}

