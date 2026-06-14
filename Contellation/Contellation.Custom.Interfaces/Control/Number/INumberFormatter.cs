namespace Contellation.Custom.Interfaces.Control.Number
{
    /// <summary>
    /// An interface that returns a string representation of a provided value, using distinct format methods to format several data types.
    /// </summary>
    public interface INumberFormatter
    {
        /// <summary>
        /// Returns a string representation of a <see cref="double"/> value.
        /// </summary>
        string FormatDouble(double? value);

        /// <summary>
        /// Returns a string representation of an <see cref="int"/> value.
        /// </summary>
        string FormatInt(int? value);

        /// <summary>
        /// Returns a string representation of a <see cref="uint"/> value.
        /// </summary>
        string FormatUInt(uint? value);
    }
}
