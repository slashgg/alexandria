using System.Text;
using System.Text.RegularExpressions;

namespace Alexandria.Shared.Utils
{
  public static class SlugGenerator
  {
    // Adapted from https://stackoverflow.com/a/14538799
    public static string Generate(string text)
    {
      var str = text.ToLowerInvariant();

      //Remove all accents
      var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(str);
      str = Encoding.ASCII.GetString(bytes);

      //Replace spaces
      str = Regex.Replace(str, @"\s", "-", RegexOptions.Compiled);

      //Remove invalid chars
      str = Regex.Replace(str, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);

      //Trim dashes from end
      str = str.Trim('-', '_');

      //Replace double occurences of - or _
      str = Regex.Replace(str, @"([-_]){2,}", "$1", RegexOptions.Compiled);

      return str;
    }
  }
}
