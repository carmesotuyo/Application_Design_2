using BlogsApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsApp.IBusinessLogic.Interfaces
{
    public interface IImporterLogic
    {
        List<string> GetAllImporters();
        List<Article> ImportArticles(string importerName, string path, User loggedUser);

    }
}
