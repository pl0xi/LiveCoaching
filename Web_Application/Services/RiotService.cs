using Web_Application.Services.Interfaces;

namespace Web_Application.Services;

public class RiotService : IRiotService
{
   private readonly HttpClient _client;
   
   public RiotService(HttpClient client)
   {
         _client = client;
   }
}