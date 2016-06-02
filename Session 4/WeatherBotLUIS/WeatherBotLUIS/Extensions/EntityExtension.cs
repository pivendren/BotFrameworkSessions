using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis.Models;

namespace WeatherBotLUIS.Extensions
{
    public static class EntityExtension
    {
        public static EntityRecommendation TryFindEntity(this LuisResult result, string type)
        {
            var entity = result.Entities?.FirstOrDefault(e => e.Type == type);
            return entity;
        }
    }
}