using Contellation.Custom.Enums.Control;
using Contellation.Custom.Interfaces.Control;
using System.Globalization;

namespace Contellation.Custom.Controls.Datas.DataGrids.Generic
{
    // Contributor : dankovics.jozsef
    internal class LanguageDictionary : ILanguageDictionary
    {
        public LanguageDictionary(string language, CultureInfo culture, Dictionary<TranslatableElements, string> dictionary)
        {
            Language = language;
            Culture = culture;
            Dictionary = dictionary;
        }

        public CultureInfo Culture { get; }
        public Dictionary<TranslatableElements, string> Dictionary { get; }
        public string Language { get; }
    }
}
