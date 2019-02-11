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

    }
}
