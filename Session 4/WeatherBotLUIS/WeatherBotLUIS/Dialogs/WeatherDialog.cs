using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherBotLUIS.Extensions;
using WeatherBotLUIS.Models;

namespace WeatherBotLUIS.Dialogs
{
    [LuisModel("LUISAppId", "LUISSubscriptionId")]
    [Serializable]
    public class WeatherDialog : LuisDialog<object>
    {
        public const string EntityNameCity = "entity_city";
        public const string OpenWeatherMapApiKey = "OpenWeatherMapApiKey";

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            var message = $"Sorry I did not understand: " + string.Join(", ", result.Intents.Select(i => i.Intent));
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        /// <summary>
        /// Returns the current weather for a city.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        [LuisIntent("weather_current_city")] //Replace with the name of the intent you have defined in your LUIS model
        public async Task CurrentWeather(IDialogContext context, LuisResult result)
        {
            using (var client = new HttpClient())
            {
                var city = result.TryFindEntity(EntityNameCity).Entity;
                var url = $"http://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid={OpenWeatherMapApiKey}";

                var response = await client.GetAsync(url);
                var res = await response.Content.ReadAsStringAsync();
                var currentWeather = JsonConvert.DeserializeObject<CurrentWeatherResponse>(res);

                await context.PostAsync($"{currentWeather.weather.First().description} in {city} and a temperature of {currentWeather.main.temp}°C");
                context.Wait(MessageReceived);
            }
        }
    }
}