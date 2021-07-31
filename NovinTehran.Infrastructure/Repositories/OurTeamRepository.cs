using NovinKargar.Core.Models;
using NovinKargar.Infrastructure;
using NovinKargar.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NovinKargar.Infrastructure.Repositories
{
    public class OurTeamRepository : BaseRepository<OurTeam, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public OurTeamRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}