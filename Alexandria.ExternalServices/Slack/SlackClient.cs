using System.Data;
using Microsoft.Extensions.Options;

namespace Alexandria.ExternalServices.Slack
{
  public class SlackClient
  {
    private readonly SlackAPI.SlackClient client;

    public SlackClient(IOptions<Shared.Configuration.Slack> slackConfig)
    {
      var token = slackConfig.Value?.Token ?? throw new NoNullAllowedException("Configuration needs to be defined");
      this.client = new SlackAPI.SlackClient(token);
    }

    public void SendMessage(string message, string destinationChannel)
    {
      this.client.PostMessage(null, destinationChannel, message);
      return;
    }
  }
}
