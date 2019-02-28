using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ThemeParkRater.Models.ThemeParkRatingModels;
using ThemeParkRater.Services;

namespace ThemeParkRater.WebAPI.Controllers
{
    [RoutePrefix("api/ThemeParkRating")]
    public class RatingController : ApiController
    {
        [Route("ByParkID/{id:int}")]
        [Authorize]
        public IHttpActionResult GetRatingsByRatingId(int id)
        {
            var service = GetRatingService();
            var rating = service.GetRatingsByRatingID(id);
            return Ok(rating);
        }

        [Route("ByParkID/{id:int}")]
        [Authorize]
        public IHttpActionResult GetRatingsByParkId(int id)
        {
            var service = GetRatingService();
            var ratings = service.GetRatingsByParkID(id);
            return Ok(ratings);
        }

        [HttpPost]
        [Route("NewRating/{id:int}")]
        public IHttpActionResult Post(ThemeParkRatingCreate rating)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = GetRatingService();

            if (!service.CreateRating(rating))
                return InternalServerError();

            return Ok();
        }

        [HttpPut]
        [Route("EditRating/{id:int}")]
        public IHttpActionResult Put(ThemeParkRatingEdit park)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var service = GetRatingService();

            if (!service.EditThemeParkRating(park))
                return InternalServerError();

            return Ok();
        }

        [HttpDelete]
        [Route("DeleteRating/{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            var service = GetRatingService();

            if (!service.DeleteThemeParkRating(id))
                return InternalServerError();

            return Ok();
        }

        private ThemeParkRatingService GetRatingService()
        {
            var userId = Guid.Parse(User.Identity.GetUserId());
            var service = new ThemeParkRatingService(userId);
            return service;
        }
    }
}
