using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Alexandria.DTO.Util.ProfanityFilter;
using Alexandria.EF.Context;
using Alexandria.Interfaces.Processing;
using Alexandria.Shared.Enums;
using Alexandria.Shared.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Alexandria.Orchestration.Utils
{
  public class ProfanityValidator : IProfanityValidator
  {
    private readonly AlexandriaContext alexandriaContext;
    private readonly IMemoryCache cache;
    private AsyncLazy<IList<ProfanityFilter>> profanityFilters;

    public ProfanityValidator(AlexandriaContext context, IMemoryCache cache)
    {
      this.alexandriaContext = context;
      this.cache = cache;
      this.profanityFilters = new AsyncLazy<IList<ProfanityFilter>>(async () =>
      {
        var profanityFilters = await this.cache.GetOrCreateAsync(Shared.Cache.ProfanityFilter.Words, async (cacheItem) =>
        {
          var filters = await alexandriaContext.ProfanityFilters.ToListAsync();
          var dtos = filters.Select(AutoMapper.Mapper.Map<ProfanityFilter>).ToList();
          cacheItem.SetAbsoluteExpiration(DateTimeOffset.UtcNow.AddMinutes(30));
          return dtos;
        });

        return profanityFilters;
      });
    }


    public async Task<ProfanityCheckResponse> Check(string phrase)
    {
      var profanityFilters = await this.profanityFilters.Value;
      var formattedPhrase = Regex.Replace(phrase, @"\s+", "").ToLowerInvariant();

      var currentSeverity = ProfanityFilterSeverity.Clear;
      string offendingWord = null;
      foreach (var filter in profanityFilters)
      {
        if (formattedPhrase.Contains(filter.Word, StringComparison.InvariantCultureIgnoreCase) && filter.Severity < currentSeverity)
        {
          currentSeverity = filter.Severity;
          offendingWord = filter.Word;
        }
      }

      return new ProfanityCheckResponse(currentSeverity, offendingWord);
    }
  }
}
