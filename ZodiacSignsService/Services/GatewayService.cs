using System;
using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZodiacSignsService.Services
{
    public class GatewayService : Gateway.GatewayBase
    {
        private class Date
        {
            public int Month { get; set; }
            public int Day { get; set; }
            public int Year { get; set; }

        }

        public static bool IsDateValid(string input)
        {
            if (input.Length != 10 && input.Length != 8)
            {
                return false;
            }
            if (input.Count(c => c == '/') != 2)
            {
                return false;
            }
            var substrings = input.Split('/');
            var date = new Date();
            for (int index = 0; index < substrings.Length; index++)
            {
                int parsedInt;
                if (!int.TryParse(substrings[index], out parsedInt))
                {
                    return false;
                }
                if (parsedInt == 0)
                {
                    return false;
                }
                switch (index)
                {
                    case 0:
                        if (!(parsedInt >= 1 && parsedInt <= 12))
                        {
                            return false;
                        }
                        date.Month = parsedInt;
                        break;
                    case 1:
                        if (!(parsedInt >= 1 && parsedInt <= 31))
                        {
                            return false;
                        }
                        date.Day = parsedInt;
                        break;
                    case 2:
                        if (!(parsedInt >= 1900 && parsedInt <= 2021))
                        {
                            return false;
                        }
                        date.Year = parsedInt;
                        break;
                }
            }

            switch (date.Month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    if (!(date.Day >= 1 && date.Day <= 31))
                    {
                        return false;
                    }
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    if (!(date.Day >= 1 && date.Day <= 30))
                    {
                        return false;
                    }
                    break;
                case 2:
                    if (DateTime.IsLeapYear(date.Year))
                    {
                        if (!(date.Day >= 1 && date.Day <= 29))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!(date.Day >= 1 && date.Day <= 28))
                        {
                            return false;
                        }
                    }
                    break;
            }

            return true;
        }
        public override Task<GatewayReply> ProcessRequest(GatewayRequest request, ServerCallContext context)
        {
            return base.ProcessRequest(request, context);
        }
    }
}
