using System.Runtime.Serialization;

namespace Contellation.Custom.Enums.Control.Placements
{
    public enum HorizontalPlacement
    {
        /// <summary>
        /// Puts the element on the left.
        /// </summary>
        [EnumMember(Value = "Left")]
        Left,

        /// <summary>
        /// Puts the element on the right.
        /// </summary>
        [EnumMember(Value = "Right")]
        Right,
    }
}
