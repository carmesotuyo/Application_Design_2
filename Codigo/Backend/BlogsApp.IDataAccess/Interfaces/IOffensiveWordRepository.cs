using System;
using BlogsApp.Domain.Entities;

namespace BlogsApp.IDataAccess.Interfaces
{
	public interface IOffensiveWordRepository : IRepository<OffensiveWord>
	{
		void Remove(OffensiveWord word);
	}
}

