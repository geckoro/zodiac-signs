using Grpc.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ZodiacSignsService.Models;

namespace SpringService.Services
{
    public class SpringService : Season.SeasonBase
    {
        public override Task<SeasonReply> GetZodiacSign(SeasonRequest request, ServerCallContext context)
        {
            string result = "Invalid";
            List<ZodiacSign> list = new List<ZodiacSign>();

            string jsonString = File.ReadAllText(@"D:\FACULT 2020-2021\CNA\zodiac-signs\Seasons\SpringService\Resources\spring.json");
            list = JsonSerializer.Deserialize<List<ZodiacSign>>(jsonString);

            foreach (var sign in list)
            {
                if (IsBetween(request.Date, sign.StartDate, sign.EndDate))
                {
                    result = sign.Name;
                }
            }

            return Task.FromResult(new SeasonReply
            {
                ZodiacSign = result
            });
        }

        public bool IsBetween(Date date, string startDate, string endDate)
        {
            if (date.Month == int.Parse(startDate.Substring(0, 2)))
            {
                if (date.Day >= int.Parse(startDate.Substring(3, 2)))
                {
                    return true;
                }
            }
            if (date.Month == int.Parse(endDate.Substring(0, 2)))
            {
                if (date.Day >= int.Parse(endDate.Substring(3, 2)))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
