using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceQuotes.Domain.Entities;

namespace ServiceQuotes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController<T> : ControllerBase
    {
        // returns the current authenticated account (null if not logged in)
        public Account Account => (Account)HttpContext.Items["Account"];
    }
}
