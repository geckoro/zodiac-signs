using System;
using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZodiacSignsService.Services
{
    public class GatewayService : Gateway.GatewayBase
    {
        public override Task<GatewayReply> ProcessRequest(GatewayRequest request, ServerCallContext context)
        {
            return Task.FromResult(new GatewayReply
            {
                ZodiacSign = "Leo"
            });
        }
    }
}
