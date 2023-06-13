using BlogsApp.Domain.Entities;

namespace BlogsApp.IImporter
{
    public interface IImporterInterface
    {
        string GetName();

        List<Article> ImportArticles(string path, User loggedUser);

    }

}
