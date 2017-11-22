using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Auctus.Service;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using Auctus.Util;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base(loggerFactory, cache, serviceProvider) { }
        
        public IActionResult Index()
        {
            return View();
        }
    }
}
