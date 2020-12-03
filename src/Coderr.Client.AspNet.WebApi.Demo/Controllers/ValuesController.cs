using System;
using System.Collections.Generic;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace Coderr.Client.AspNet.WebApi.Demo.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            switch (id)
            {
                case 100:
                    throw new InvalidOperationException("Cannot be 100");
                case 103:
                    throw new HttpException(403, "For Biden!");
                case 101:
                    throw new HttpException(401, "Use the key");
                case 102:
                    Thread.Sleep(3321);
                    break;
                case 105:
                    ModelState.AddModelError("", "Need some model");
                    break;
            }
            return "value";
        }

        [Route("admin/"), Authorize(Roles = "Admin")]
        public string Get2()
        {
            return "";
        }

        public void Post([FromBody] string value)
        {
        }


        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
