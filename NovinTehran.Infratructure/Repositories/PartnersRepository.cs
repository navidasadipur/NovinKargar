﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using drugStore7.Core.Models;
using drugStore7.Infrastructure;
using drugStore7.Infrastructure.Repositories;

namespace drugStore7.Infratructure.Repositories
{
    public class PartnersRepository : BaseRepository<Partner, MyDbContext>
    {
        private readonly MyDbContext _context;
        private readonly LogsRepository _logger;
        public PartnersRepository(MyDbContext context, LogsRepository logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
    }
}
