using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThemeParkRater.Data;
using ThemeParkRater.Models.ThemeParkRatingModels;
using ThemeParkRater.WebMVC.Data;

namespace ThemeParkRater.Services
{
    public class ThemeParkRatingService
    {
        private Guid _userID;

        public ThemeParkRatingService(Guid userID)
        {
            _userID = userID;
        }

        public bool CreateRating(ThemeParkRatingCreate model)
        {
            var rating = new ThemeParkRating
            {
                ThemeParkID = model.ThemeParkID,
                GoodnessLevel = model.GoodnessLevel,
                OwnerID = _userID
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.Ratings.Add(rating);
                if (ctx.SaveChanges() == 1)
                {
                    CalculateGoodness(model.ThemeParkID);
                    return true;
                }
                return false;
            }
        }

        public IEnumerable<ThemeParkRatingListItem> GetRatingsByParkID(int parkID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Ratings.Where(r => r.ThemeParkID == parkID)
                    .Select(r => new ThemeParkRatingListItem
                    {
                        ThemeParkRatingID = r.ThemeParkRatingID,
                        ThemeParkName = r.ThemePark.ThemeParkName,
                        ThemeParkID = r.ThemeParkID,
                        GoodnessLevel = r.GoodnessLevel,
                        ThemeParkState = r.ThemePark.ThemeParkState
                    });

                return query.ToArray();
            }
        }

        public ThemeParkRatingDetails GetRatingsByRatingID(int ratingID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Ratings.Single(r => r.ThemeParkRatingID == ratingID);

                var model = new ThemeParkRatingDetails
                {
                    ThemeParkRatingID = query.ThemeParkRatingID,
                    ThemeParkName = query.ThemePark.ThemeParkName,
                    ThemeParkID = query.ThemeParkID,
                    GoodnessLevel = query.GoodnessLevel,
                };

                return model;
            }
        }

        public bool EditThemeParkRating(ThemeParkRatingEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Ratings.Single(r => r.ThemeParkRatingID == model.ThemeParkRatingID);
                entity.GoodnessLevel = model.GoodnessLevel;
                entity.ThemeParkID = model.ThemeParkID;

                ctx.Ratings.Add(entity);
                if (ctx.SaveChanges() == 1)
                {
                    CalculateGoodness(model.ThemeParkID);
                    return true;
                }
                return false;
            }
        }

        public bool DeleteThemeParkRating(int ratingID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var rating = ctx.Ratings.Single(r => r.ThemeParkRatingID == ratingID);

                ctx.Ratings.Remove(rating);
                if (ctx.SaveChanges() == 1)
                {
                    CalculateGoodness(rating.ThemeParkID);
                    return true;
                }
                return false;
            }
        }

        private bool CalculateGoodness(int parkID)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Ratings.Where(r => r.ThemeParkID == parkID).ToList();
                float totalGoodness = 0;
                foreach (var rating in query)
                {
                    totalGoodness += rating.GoodnessLevel;
                }
                totalGoodness /= query.Count;

                var park = ctx.ThemeParks.Single(p => p.ThemeParkID == parkID);
                park.GoodnessLevel = totalGoodness;

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
