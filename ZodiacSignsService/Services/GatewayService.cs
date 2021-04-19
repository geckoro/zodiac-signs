using System;
using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using ZodiacSignsService.Models;
using System.Text.Json;
using Grpc.Net.Client;
using SeasonService;

namespace ZodiacSignsService.Services
{
    public class GatewayService : Gateway.GatewayBase
    {
        public override async Task<GatewayReply> ProcessRequest(GatewayRequest request, ServerCallContext context)
        {
            SeasonReply reply = null;

            var dateToBeSent = new SeasonService.Date()
            {
                Month = request.Date.Month,
                Day = request.Date.Day,
                Year = request.Date.Year
            };

            switch (request.Date.Month)
            {
                case 3:
                case 4:
                case 5:
                    Console.WriteLine("Communicating with Spring service...");
                    var channel = GrpcChannel.ForAddress("https://localhost:5003");
                    var client = new Season.SeasonClient(channel);

                    reply = await client.GetZodiacSignAsync(
                        new SeasonRequest
                        {
                            Date = dateToBeSent
                        });
                    break;
                case 6:
                case 7:
                case 8:
                    Console.WriteLine("Communicating with Summer service...");
                    channel = GrpcChannel.ForAddress("https://localhost:5005");
                    client = new Season.SeasonClient(channel);

                    reply = await client.GetZodiacSignAsync(
                        new SeasonRequest
                        {
                            Date = dateToBeSent
                        });
                    break;
                case 9:
                case 10:
                case 11:
                    Console.WriteLine("Communicating with Autumn service...");
                    channel = GrpcChannel.ForAddress("https://localhost:5009");
                    client = new Season.SeasonClient(channel);

                    reply = await client.GetZodiacSignAsync(
                        new SeasonRequest
                        {
                            Date = dateToBeSent
                        });
                    break;
                case 12:
                case 1:
                case 2:
                    Console.WriteLine("Communicating with Winter service...");
                    channel = GrpcChannel.ForAddress("https://localhost:5007");
                    client = new Season.SeasonClient(channel);

                    reply = await client.GetZodiacSignAsync(
                        new SeasonRequest
                        {
                            Date = dateToBeSent
                        });
                    break;
                default:
                    reply = null;
                    break;
            }

            if (reply == null)
            {
                return await Task.FromResult(new GatewayReply
                {
                    ZodiacSign = "Invalid"
                });
            }

            return await Task.FromResult(new GatewayReply
            {
                ZodiacSign = reply.ZodiacSign
            });
        }
    }
}
