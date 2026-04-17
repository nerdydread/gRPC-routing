using Grpc.Core;
using Coprocess;

namespace routing_plugin.Services;


public class DispatcherService(ILogger<DispatcherService> logger) : Dispatcher.DispatcherBase
{
    public override Task<Coprocess.Object> Dispatch(Coprocess.Object request, ServerCallContext context)
    {
        logger.LogInformation("Dispatch called: hook={HookType} name={HookName}",request.HookType, request.HookName);

     
        // TODO: add your routing/middleware logic here.
        // Inspect request.HookType to branch on Pre, Post, CustomKeyCheck, etc.
        // Modify request.Request, request.Session, or request.Response as needed,
        // then return the (possibly modified) object.
         if (request.HookName == "RouteRequest" && request.HookType.ToString() == "Pre" )
        {
            logger.LogInformation("Executing RouteRequest  - Forwarding Request");
            //string storeNumber = request.Request.Headers["Storenumber"];

            // TODO: Lookup Loyalty API Type Based on Store Number

            // Modify the URL to use the internal loopback scheme in Tyk
            // Format: tyk://<API_ID>/<path>
            // Where API_ID is the the api_id of your internal API
            request.Request.Url = "tyk://internal-vendor-api-1/get";
            return Task.FromResult(request);
        }
        // Return unmodified object for other hooks
        return Task.FromResult(request);    
    }

    public override Task<EventReply> DispatchEvent(Event request, ServerCallContext context)
    {
        logger.LogInformation("DispatchEvent called: payload={Payload}", request.Payload);
        return Task.FromResult(new EventReply());
    }

    // private static LookupLoyaltyType(string storeNumber) {
        
    // }
}
