using System.Threading.Tasks;

namespace Alexandria.Interfaces.Processing
{
  public interface IProfanityValidator
  {
    Task<DTO.Util.ProfanityFilter.ProfanityCheckResponse> Check(string phrase);
  }
}
