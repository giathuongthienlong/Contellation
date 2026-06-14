namespace Contellation.Custom.Enums.Control
{
    /// <summary>
    /// Defines values that specify the input validation behavior of a <see cref="NumberBox"/> when invalid input is entered.
    /// </summary>
    public enum NumberBoxValidationMode
    {
        /// <summary>
        /// Input validation is disabled.
        /// </summary>
        InvalidInputOverwritten,

        /// <summary>
        /// Invalid input is replaced by <see cref="NumberBox"/> PlaceholderText text.
        /// </summary>
        Disabled
    }
}
