using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ThemeParkRater.Models.ThemeParkModels;
using ThemeParkRater.Services;

namespace ThemeParkRater.WebAPI.Controllers
{
    [RoutePrefix("api/ThemePark")]
    public class ThemeParkController : ApiController
    {
        [Route("All")]
        public IHttpActionResult GetAll()
        {
            var service = new ThemeParkService();
            var parks = service.GetThemeParks();
            return Ok(parks);
        }

        [Route("Single/{id:int}")]
        public IHttpActionResult GetById(int id)
        {
            var service = new ThemeParkService();
            var park = service.GetParkByID(id);
            return Ok(park);
        }

        [HttpPost]
        public IHttpActionResult Post(ThemeParkCreate park)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = new ThemeParkService();

            if (!service.CreateThemePark(park))
                return InternalServerError();

            return Ok();
        }

        [HttpPut]
        public IHttpActionResult Put(ThemeParkEdit park)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = new ThemeParkService();

            if (!service.EditThemePark(park))
                return InternalServerError();

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            var service = new ThemeParkService();

            if (!service.DeleteThemePark(id))
                return InternalServerError();

            return Ok();
        }
    }
}
