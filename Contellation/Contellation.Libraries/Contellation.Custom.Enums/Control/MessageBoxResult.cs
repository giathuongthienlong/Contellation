namespace Contellation.Custom.Enums.Control
{
    /// <summary>
    /// Specifies identifiers to indicate the return value of a <see cref="MessageBox"/>.
    /// </summary>
    public enum MessageBoxResult
    {
        /// <summary>
        /// No button was tapped.
        /// </summary>
        None,

        /// <summary>
        /// The primary button was tapped by the user.
        /// </summary>
        Primary,

        /// <summary>
        /// The secondary button was tapped by the user.
        /// </summary>
        Secondary
    }
}
