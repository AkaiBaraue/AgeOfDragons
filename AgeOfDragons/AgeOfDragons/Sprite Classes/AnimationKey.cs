// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimationKey.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   The different types of animation that a character has.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Sprite_Classes
{
    /// <summary>
    /// The different types of animation that a character has.
    /// </summary>
    public enum AnimationKey
    {
        /// <summary>
        /// Represents the character going down.
        /// </summary>
        Down,

        /// <summary>
        /// Represents the character going left.
        /// </summary>
        Left,

        /// <summary>
        /// Represents the character going right.
        /// </summary>
        Right,

        /// <summary>
        /// Represents the character going up.
        /// </summary>
        Up,

        /// <summary>
        /// Represents the character standing idle.
        /// </summary>
        Idle,

        /// <summary>
        /// Represents the character when it is selected.
        /// </summary>
        Selected
    }
}
