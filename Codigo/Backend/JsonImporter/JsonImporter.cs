using BlogsApp.Domain.Entities;
using BlogsApp.IImporter;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace BlogsApp.JsonImporter
{
    public class JsonImporter : IImporterInterface

    {
        public string GetName()
        {
            return "Json Importer";
        }

        public List<Article> ImportArticles(string path, User loggedUser)
        {
            string json = File.ReadAllText(path);

            List<Article> importedArticles = JsonConvert.DeserializeObject<List<Article>>(json);

            return importedArticles;
        }
    }
   
}