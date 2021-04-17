using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZodiacSignsService.Services
{
    public class SpringService : Season.SeasonBase
    {
        public override Task<SeasonReply> GetZodiacSign(SeasonRequest request, ServerCallContext context)
        {
            return Task.FromResult(new SeasonReply
            {
                ZodiacSign = "Leo"
            });
        }
    }
}
