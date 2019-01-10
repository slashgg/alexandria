using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Alexandria.DTO.UserProfile;
using Alexandria.Interfaces.Services;
using Alexandria.Shared.Configuration;
using IdentityModel.Client;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Svalbard.Services;

namespace Alexandria.Orchestration.Services
{
  public class PassportClient : IPassportClient
  {
    private readonly IMemoryCache cache;
    private readonly ILogger<PassportClient> logger;

    public PassportClient(IMemoryCache cache, IOptions<PassportClientConfiguration> optionsAccessor, ILogger<PassportClient> logger)
    {
      this.cache = cache;
      this.logger = logger;

      Configuration = optionsAccessor.Value ?? new PassportClientConfiguration();
    }

    public PassportClientConfiguration Configuration { get; }
    public HttpClient Backchannel => new HttpClient { BaseAddress = new Uri(Configuration.BaseUrl) };

    public async Task<ServiceResult> UpdateProfile(Guid userId, UpdatePassportUser dto)
    {
      var result = new ServiceResult();
      var token = await GetBackchannelToken();
      using (var client = Backchannel)
      {
        var request = new HttpRequestMessage(HttpMethod.Put, string.Format(Configuration.UpdateUserEndpoint, userId));
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var payload = JsonConvert.SerializeObject(dto);
        request.Content = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await client.SendAsync(request);
        try
        {
          response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
          logger.LogError(e, "Failed to update the passport user");
          result.Error = Shared.ErrorKey.UserProfile.UserUpdateFailed;
          return result;
        }
      }

      result.Succeed();
      return result;
    }

    private async Task<string> GetBackchannelToken()
    {
      return await cache.GetOrCreateAsync("backchannel-token", async (entry) =>
      {
        using (var client = Backchannel)
        {
          var discovery = await client.GetDiscoveryDocumentAsync();
          if (discovery.IsError)
          {
            logger.LogError(discovery.Exception, "Failed to get the discovery document from passport.");
            return string.Empty;
          }

          var request = new ClientCredentialsTokenRequest
          {
            Address = discovery.TokenEndpoint,
            ClientId = Configuration.ClientId,
            ClientSecret = Configuration.ClientSecret,
            Scope = Configuration.Scope,
          };

          var response = await client.RequestClientCredentialsTokenAsync(request);
          if (response.IsError)
          {
            logger.LogError(response.Exception, "Failed to fetch backchannel token.");
            return string.Empty;
          }

          entry.SetValue(response.AccessToken);
          entry.SetAbsoluteExpiration(TimeSpan.FromMilliseconds(response.ExpiresIn));

          return response.AccessToken;
        }
      });
    }
  }
}
