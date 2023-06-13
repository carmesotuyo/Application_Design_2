using BlogsApp.Domain.Entities;
using BlogsApp.IBusinessLogic.Interfaces;
using System.Reflection;
using BlogsApp.IImporter;

namespace BlogsApp.BusinessLogic.Logics
{
    public class ImporterLogic : IImporterLogic
    {
        private readonly IArticleLogic _articleLogic;

        public ImporterLogic(IArticleLogic articleLogic)
        {
            _articleLogic = articleLogic;
        }

        public List<string> GetAllImporters()
        {
            return GetImporterImplementations().Select(importer => importer.GetName()).ToList();
        }

        public List<Article> ImportArticles(string importerName, string path, User loggedUser)
        {
            List<IImporterInterface> importers = GetImporterImplementations();
            IImporterInterface? desiredImplementation = null;

            foreach (IImporterInterface importer in importers)
            { 
                if (importer.GetName() == importerName)
                {
                    desiredImplementation = importer;
                    break;
                }
            }

            if (desiredImplementation == null)
                throw new Exception("No se pudo encontrar el importador solicitado");
           
            List<Article> importedArticles = desiredImplementation.ImportArticles(path, loggedUser);
            Console.WriteLine(importedArticles);

            CreateArticles(importedArticles, loggedUser);
            return importedArticles;
        }

        private void CreateArticles(List<Article> importedArticles, User loggedUser)
        {
            foreach (var article in importedArticles)
            {
                article.User = loggedUser;
                _articleLogic.CreateArticle(article, loggedUser);
            }
        }

        private List<IImporterInterface> GetImporterImplementations()
        {
            List<IImporterInterface> availableImporters = new List<IImporterInterface>();

            string importersPath = "./Importers";
            string[] filePaths = Directory.GetFiles(importersPath);

            foreach (string filePath in filePaths)
            {
                if (filePath.EndsWith(".dll"))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    Assembly assembly = Assembly.LoadFile(fileInfo.FullName);

                    foreach (Type type in assembly.GetTypes())
                    {
                        if (typeof(IImporterInterface).IsAssignableFrom(type) && !type.IsInterface)
                        {
                            IImporterInterface importer = (IImporterInterface)Activator.CreateInstance(type);
                            if (importer != null)
                                availableImporters.Add(importer);
                        }
                    }
                }
            }

            return availableImporters;
        }
    }
}
