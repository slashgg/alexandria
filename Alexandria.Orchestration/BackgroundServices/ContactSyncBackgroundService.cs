using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Processing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Alexandria.Orchestration.BackgroundServices
{
  public class ContactSyncBackgroundService : BackgroundService
  {
    private string queue;
    private IBackgroundWorker backgroundWorker;
    private IContactBook contactBook;
    private IServiceProvider provider;

    public ContactSyncBackgroundService(IOptions<Shared.Configuration.Queue> queues, IBackgroundWorker backgroundWorker, IServiceProvider provider)
    {
      this.queue = queues.Value.Contact;
      this.backgroundWorker = backgroundWorker;
      this.provider = provider;
      this.contactBook = this.provider.GetService<IContactBook>();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      while (!stoppingToken.IsCancellationRequested)
      {
        var messages = await this.backgroundWorker.ReceiveMessages<DTO.Marketing.ContactSync>(this.queue, 10, 15);
        if (messages != null && messages.Any())
        {
          var newUsers = messages.Where(p => p.Data.New).ToList();
          var updateUsers = messages.Where(p => !p.Data.New).ToList();

          if (newUsers.Any())
          {
            var contacts = await this.GetContacts(newUsers.Select(u => u.Data).ToList());
            await this.contactBook.CreateContacts(contacts);
          }

          if (updateUsers.Any())
          {
            var contacts = await this.GetContacts(updateUsers.Select(u => u.Data).ToList());
            await this.contactBook.UpdateContacts(contacts);
          }

          await this.backgroundWorker.AcknowledgeMessage(this.queue, messages.Select(m => m.Receipt).ToList());
        }

        Thread.Sleep(30 * 1000);
      }
    }


    private async Task<IList<DTO.Marketing.Contact>> GetContacts(IList<DTO.Marketing.ContactSync> toSync)
    {
      using (var scope = this.provider.CreateScope())
      {
        var context = scope.ServiceProvider.GetService<AlexandriaContext>();
        var userIds = toSync.Select(u => u.UserId);
        var users = await context.UserProfiles.Include(u => u.TeamMemberships)
                                              .ThenInclude(m => m.Team)
                                              .ThenInclude(t => t.Competition)
                                              .Include(u => u.TeamMemberships)
                                              .ThenInclude(u => u.TeamRole)
                                              .Where(u => userIds.Contains(u.Id))
                                              .ToListAsync();

        var contacts = users.Select(AutoMapper.Mapper.Map<DTO.Marketing.Contact>).ToList();
        return contacts;
      }
    }
  }
}
