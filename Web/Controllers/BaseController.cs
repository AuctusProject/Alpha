using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Auctus.Util;
using Auctus.Service;
using System.Net.Http;
using Auctus.Util.NotShared;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILoggerFactory LoggerFactory;
        protected readonly ILogger Logger;
        protected readonly Cache MemoryCache;

        protected BaseController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider)
        {
            MemoryCache = cache;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType().Namespace);
        }
    }
}