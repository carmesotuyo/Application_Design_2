using BlogsApp.IBusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogsApp.WebAPI.Controllers
{
    [Route("api/articles")]
    public class ArticleController : BlogsAppControllerBase
    {
        private readonly IArticleLogic articleLogic;

        public ArticleController(IArticleLogic articleLogic)
        {
            this.articleLogic = articleLogic;
        }

        // GET: api/Article
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Article/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Article
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Article/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Article/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}