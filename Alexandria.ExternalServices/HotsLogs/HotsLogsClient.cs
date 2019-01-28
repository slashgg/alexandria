using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Alexandria.ExternalServices.HotsLogs.DTO;
using Newtonsoft.Json;

namespace Alexandria.ExternalServices.HotsLogs
{
  public class HotsLogsClient
  {
    private HttpClient client;
    public HotsLogsClient()
    {
      this.client = new HttpClient();
      client.BaseAddress = new Uri("https://api.hotslogs.com");
    }

    public async Task<IList<LeaderboardRanking>> GetPlayerRankings(string battleTag, int regionId = 1)
    {
      var transformedBattleTag = battleTag.Replace('#', '_');

      var response = await this.client.GetAsync($"/Public/Players/{regionId}/{transformedBattleTag}");

      if (!response.IsSuccessStatusCode)
      {
        return null;
      }

      var body = await response.Content.ReadAsStringAsync();
      var playerResponse = JsonConvert.DeserializeObject<PlayerResponse>(body);
      return playerResponse?.Rankings;

    }
  }
}
