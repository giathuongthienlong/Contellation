using System.ComponentModel;
using System.Globalization;

namespace Contellation.Custom.Controls
{
    public class BreakPoints : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public int XS { get; set; } = 0;
        public int SM { get; set; } = 576;
        public int MD { get; set; } = 768;
        public int LG { get; set; } = 992;
        public int XL { get; set; } = 1200;

        public static BreakPoints Default => new BreakPoints();

        public BreakPoints() { }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BreakPointsConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
                    => sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
                    => value is string ? BreakPoints.Default : base.ConvertFrom(context, culture, value);
    }
}
