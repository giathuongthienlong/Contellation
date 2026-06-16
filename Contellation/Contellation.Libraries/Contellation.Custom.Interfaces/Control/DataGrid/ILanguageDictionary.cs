using Contellation.Custom.Enums.Control;
using System.Globalization;

namespace Contellation.Custom.Interfaces.Control
{
    public interface ILanguageDictionary
    {
        CultureInfo Culture { get; }
        Dictionary<TranslatableElements, string> Dictionary { get; }
        string Language { get; }
    }
}
