using Grpc.Net.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using ZodiacSignsService;

namespace ZodiacSignsClient
{
    class Program
    {
        private class Date
        {
            public int Month { get; set; }
            public int Day { get; set; }
            public int Year { get; set; }

        }

        private static Date IsDateValid(string input)
        {
            if (input.Length != 10 && input.Length != 8)
            {
                return null;
            }
            if (input.Count(c => c == '/') != 2)
            {
                return null;
            }
            var substrings = input.Split('/');
            var date = new Date();
            for (int index = 0; index < substrings.Length; index++)
            {
                int parsedInt;
                if (!int.TryParse(substrings[index], out parsedInt))
                {
                    return null;
                }
                if (parsedInt == 0)
                {
                    return null;
                }
                switch (index)
                {
                    case 0:
                        if (!(parsedInt >= 1 && parsedInt <= 12))
                        {
                            return null;
                        }
                        date.Month = parsedInt;
                        break;
                    case 1:
                        if (!(parsedInt >= 1 && parsedInt <= 31))
                        {
                            return null;
                        }
                        date.Day = parsedInt;
                        break;
                    case 2:
                        if (!(parsedInt >= 1900 && parsedInt <= 2021))
                        {
                            return null;
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
                        return null;
                    }
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    if (!(date.Day >= 1 && date.Day <= 30))
                    {
                        return null;
                    }
                    break;
                case 2:
                    if (DateTime.IsLeapYear(date.Year))
                    {
                        if (!(date.Day >= 1 && date.Day <= 29))
                        {
                            return null;
                        }
                    }
                    else
                    {
                        if (!(date.Day >= 1 && date.Day <= 28))
                        {
                            return null;
                        }
                    }
                    break;
            }

            return date;
        }

        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Gateway.GatewayClient(channel);

            Console.Write("Type in your desired date: ");
            var input = Console.ReadLine();
            var date = IsDateValid(input);

            if (date == null)
            {
                throw new ArgumentException("The date must be valid.");
            }

            var dateToBeSent = new ZodiacSignsService.Date()
            {
                Month = date.Month,
                Day = date.Day,
                Year = date.Year
            };

            var reply = await client.ProcessRequestAsync(
                              new GatewayRequest
                              {
                                  Date = dateToBeSent
                              }); ;

            Console.WriteLine("The zodiac sign associated with that date is " + reply.ZodiacSign);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
