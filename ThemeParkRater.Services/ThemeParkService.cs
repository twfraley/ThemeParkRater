using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThemeParkRater.Data;
using ThemeParkRater.Models.ThemeParkModels;
using ThemeParkRater.WebMVC.Data;

namespace ThemeParkRater.Services
{
    public class ThemeParkService
    {
        public bool CreateThemePark(ThemeParkCreate model)
        {
            ThemePark themePark = new ThemePark()
            {
                ThemeParkName = model.ThemeParkName,
                ThemeParkCity = model.ThemeParkCity,
                ThemeParkState = model.ThemeParkState
            };

            using (var ctx = new ApplicationDbContext())
            {
                ctx.ThemeParks.Add(themePark);
                return ctx.SaveChanges() == 1;
            }

        }

        public bool EditThemePark(ThemeParkEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.ThemeParks
                    .FirstOrDefault
                    (park => park.ThemeParkID == model.ThemeParkID);

                entity.ThemeParkID = model.ThemeParkID;
                entity.ThemeParkCity = model.ThemeParkCity;
                entity.ThemeParkState = model.ThemeParkState;
                entity.ThemeParkName = model.ThemeParkName;

                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<ThemeParkListItem> GetThemeParks()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.ThemeParks.Select(p => new ThemeParkListItem
                {
                    ThemeParkID = p.ThemeParkID,
                    ThemeParkName = p.ThemeParkName,
                    ThemeParkState = p.ThemeParkState,
                    GoodnessLevel = p.GoodnessLevel
                });
                return query.ToArray();
            }
        }

        public ThemeParkDetail GetParkByID(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.ThemeParks.FirstOrDefault(park => park.ThemeParkID == id);

                var model = new ThemeParkDetail
                {
                    ThemeParkCity = entity.ThemeParkCity,
                    ThemeParkID = entity.ThemeParkID,
                    ThemeParkName = entity.ThemeParkName,
                    ThemeParkState = entity.ThemeParkState,
                    GoodnessLevel = entity.GoodnessLevel
                };

                return model;
            }
        }

        public bool DeleteThemePark(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.ThemeParks.Single(
                    p => p.ThemeParkID == id);

                ctx.ThemeParks.Remove(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        private float CalculateGoodness(int parkID)
        {
            using(var ctx = new ApplicationDbContext())
            {
                var query = ctx.Ratings.Where(r => r.ThemeParkID == parkID).ToList();
                float totalGoodness = 0;
                foreach (var rating in query)
                {
                    totalGoodness += rating.GoodnessLevel;
                }
                totalGoodness /= query.Count;
                return totalGoodness;
            }
        }

    }
}
