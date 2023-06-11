using BlogsApp.IDataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using BlogsApp.Domain.Entities;
using BlogsApp.DataAccess.Interfaces.Exceptions;
using System.Linq;

namespace BlogsApp.DataAccess.Repositories
{
    public class OffensiveWordRepository : IOffensiveWordRepository
    {
        private DbContext Context { get; }

        public OffensiveWordRepository(DbContext context)
        {
            Context = context;
        }

        public OffensiveWord Add(OffensiveWord value)
        {
            if (Exists(i => i.Id == value.Id || i.Word == value.Word))
                throw new AlreadyExistsDbException("La palabra ya existe");
            Context.Set<OffensiveWord>().Add(value);
            Context.SaveChanges();
            return value;
        }

        public bool Exists(Func<OffensiveWord, bool> func)
        {
            return Context.Set<OffensiveWord>().Where(func).Any();
        }

        public OffensiveWord Get(Func<OffensiveWord, bool> func)
        {
            OffensiveWord word = Context.Set<OffensiveWord>().FirstOrDefault(func);
            if (word == null)
                throw new NotFoundDbException("No se encontró la palabra entre las palabras ofensivas registradas");
            return word;
        }

        public ICollection<OffensiveWord> GetAll(Func<OffensiveWord, bool> func)
        {
            ICollection<OffensiveWord> words = Context.Set<OffensiveWord>().Where(func).ToArray();
            return words;
        }

        public void Remove(OffensiveWord word)
        {
            Context.Set<OffensiveWord>().Remove(word);
            Context.SaveChanges();
        }
    }
}

