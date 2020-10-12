using System;
using System.Collections.Generic;
using Lychee.Infrastructure.Interfaces;
using Lychee.Stocks.Entities;

namespace Lychee.Stocks.Domain.Interfaces.Repositories
{
    public interface IEventRepository : IRepository<Event>
    {
        Dictionary<(string, string), Event> GetEvents(DateTime date);
    }
}
