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
                    return CalculateGoodness(model.ThemeParkID);
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
