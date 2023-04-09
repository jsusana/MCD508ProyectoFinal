using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Helpers
{
    public static class Utils
    {
        public static string FromDateTimeToSqlDate(DateTime value) => $"{value.Year}-{value.Month}-{value.Day} {value.Hour}:{value.Minute}:{value.Second}";

        public static DateTime GetRandomDateTimeFromYearsRange(int begin, int end = 2023)
        {
            DateTime date = DateTime.Now;

            // Randomize Year
            Random random = new Random();
            int year = begin == end ? begin : random.Next(begin, end);
            //Randomize Month
            int month = random.Next(1, 12);
            //Randomize day
            int day = random.Next(1, 28); //Set max day to 28 to prevent February issues
            //Randomize hour
            int hour = random.Next(1, 23);
            //Randomize minutes
            int minute = random.Next(1, 59);
            //Randomize seconds
            int second = random.Next(1, 59);

            date = new DateTime(year, month, day, hour, minute, second);

            return date;
        }

        public static async Task<string> GetRandomName(int gender)
        {
            string gender_param = gender % 2 == 0 ? "boy_names" : "girl_names";
            string requestUrl = $"https://names.drycodes.com/1?nameOptions={gender_param}";
            HttpClient httpClient = new HttpClient();
            string name = await httpClient.GetStringAsync(requestUrl);
            return name.Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty);
        }
    }
}
