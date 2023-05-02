using System;
using BlogsApp.Domain.Entities;

namespace BlogsApp.IBusinessLogic.Interfaces
{
	public interface ILogLogic
    {
        IEnumerable<Log> GetLogs(User loggedUser, DateTime from, DateTime to);
    }
}

