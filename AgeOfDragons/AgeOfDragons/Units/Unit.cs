// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Unit.cs" company="Baraue">
//   o/
// </copyright>
// <summary>
//   The base unit class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace AgeOfDragons.Units
{
    using System;

    using AgeOfDragons.Pathfinding;
    using AgeOfDragons.Sprite_Classes;
    using AgeOfDragons.Tile_Engine;
    using AgeOfDragons.Units.Unit_Types;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// The base unit class.
    /// </summary>
    public abstract class Unit
    {
        #region Field Region

        /// <summary>
        /// The name of the unit.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The sprite that represents the unit.
        /// </summary>
        private readonly AnimatedSprite sprite;

        /// <summary>
        /// Indicates whether the unit is selected or not.
        /// </summary>
        private bool selected;

        /// <summary>
        /// Indicates whether the unit has moved or not.
        /// </summary>
        private bool hasMoved;

        /// <summary>
        /// Indicates whether the unit has processed the FoW 
        /// around it.
        /// </summary>
        private bool hasProcessedFoW;

        #endregion

        #region Property Region

        /// <summary>
        /// Gets the name of the unit.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets or sets the current health of the unit.
        /// </summary>
        public int CurrentHealth { get; set; }

        /// <summary>
        /// Gets or sets the max health of the unit.
        /// </summary>
        public int MaxHealth { get; protected set; }

        /// <summary>
        /// Gets or sets the current mana of the unit.
        /// </summary>
        public int CurrentMana { get; set; }

        /// <summary>
        /// Gets or sets the max mana of the unit.
        /// </summary>
        public int MaxMana { get; protected set; }

        /// <summary>
        /// Gets or sets the move range.
        /// </summary>
        public int MoveRange { get; protected set; }

        /// <summary>
        /// Gets or sets the view range.
        /// </summary>
        public int ViewRange { get; protected set; }

        /// <summary>
        /// Gets or sets a value indicating whether the unit
        /// has moved this turn or not.
        /// </summary>
        public bool Selected
        {
            get { return this.selected; }
            set { this.selected = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the unit
        /// has moved this turn or not.
        /// </summary>
        public bool HasMoved
        {
            get { return this.hasMoved; }
            set { this.hasMoved = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the unit
        /// has processed the FOW around it or not.
        /// </summary>
        public bool HasProcessedFoW
        {
            get { return this.hasProcessedFoW; }
            set { this.hasProcessedFoW = value; }
        }

        /// <summary>
        /// Gets the sprite.
        /// </summary>
        public AnimatedSprite Sprite
        {
            get { return this.sprite; }
        }

        /// <summary>
        /// Gets the class of the unit.
        /// </summary>
        public UnitClass Class { get; private set; }

        /// <summary>
        /// Gets or sets the location of the unit.
        /// </summary>
        public Vector Location
        {
            get { return this.sprite.Position; }
            set { this.sprite.Position = value; }
        }

        #endregion

        #region Constructor Region

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="name"> The name.  </param>
        /// <param name="x"> The x location of the unit.  </param>
        /// <param name="y"> The y location of the unit.  </param>
        /// <param name="sprite"> The sprite. </param>
        /// <param name="unitClass"> The class of the unit. </param>
        protected Unit(string name, int x, int y, AnimatedSprite sprite, UnitClass unitClass)
        {
            this.name = name;
            this.CurrentHealth = 100;
            this.MaxHealth = 100;
            this.CurrentMana = 100;
            this.MaxMana = 100;
            this.MoveRange = 4;
            this.ViewRange = 3;
            this.sprite = sprite;
            this.Location = new Vector(x, y);
            this.sprite.CurrentAnimation = AnimationKey.Idle;
            this.sprite.IsAnimating = true;
            this.selected = false;
            this.hasMoved = false;
            this.hasProcessedFoW = false;
            this.Class = unitClass;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Unit"/> class.
        /// </summary>
        /// <param name="name"> The name.  </param>
        /// <param name="position"> The position of the unit.  </param>
        /// <param name="sprite"> The sprite. </param>
        /// <param name="unitClass"> The class of the unit. </param>
        protected Unit(string name, Vector position, AnimatedSprite sprite, UnitClass unitClass)
        {
            this.name = name;
            this.CurrentHealth = 100;
            this.MaxHealth = 100;
            this.CurrentMana = 100;
            this.MaxMana = 100;
            this.MoveRange = 4;
            this.ViewRange = 3;
            this.sprite = sprite;
            this.Location = position;
            this.sprite.CurrentAnimation = AnimationKey.Idle;
            this.sprite.IsAnimating = true;
            this.selected = false;
            this.hasMoved = false;
            this.hasProcessedFoW = false;
            this.Class = unitClass;
        }

        #endregion

        #region XNA Method Region

        /// <summary>
        /// Allows the game unit to update.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            this.sprite.Update(gameTime);
        }

        /// <summary>
        /// Draws the sprite.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="spriteBatch"> The sprite batch. </param>
        /// <param name="vect"> The locationToCheck position. </param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 vect)
        {
            this.sprite.Draw(gameTime, spriteBatch, vect);
        }

        #endregion

        #region Method Region

        /// <summary>
        /// Sets the selected value of the unit to true and changes the
        /// current animation to the Selected animation.
        /// </summary>
        public void Select()
        {
            this.selected = true;
            this.Sprite.SwitchAnimationTo(AnimationKey.Selected);
        }

        /// <summary>
        /// Sets the selected value of the unit to true and changes the
        /// current animation to the Selected animation.
        /// </summary>
        public void Deselect()
        {
            this.selected = false;
            this.Sprite.SwitchAnimationTo(AnimationKey.Idle);
        }

        /// <summary>
        /// Tells the unit that is has become the next turn and that
        /// it is able to move again.
        /// </summary>
        public void NextTurn()
        {
            this.hasMoved = false;
            this.hasProcessedFoW = false;
        }

        /// <summary>
        /// Moves the unit.
        /// Changes the properties that are related to a unit moving.
        /// </summary>
        public void MoveUnit()
        {
            this.hasMoved = true;
            this.hasProcessedFoW = false;
            this.Deselect();
        }

        /// <summary>
        /// Indicates whether the unit can traverse this CollisionType or not.
        /// </summary>
        /// <param name="toTraverse"> The CollisionType to traverse. </param>
        /// <returns>
        /// True if the unit can traverse it.
        /// False if the unit cannot.
        /// </returns>
        public bool CanTraverse(CollisionType toTraverse)
        {
            return this.Class.CanTraverse(toTraverse);
        }

        /// <summary>
        /// Checks if the location given is within the move range of the unit.
        /// </summary>
        /// <param name="locationToCheck"> The location to check. </param>
        /// <returns>
        /// True if it is within the move range of the unit.
        /// False if it is not.
        /// </returns>
        public bool PointWithinMoveRange(Vector locationToCheck)
        {
            if (Math.Abs(locationToCheck.X - this.Location.X) + Math.Abs(locationToCheck.Y - this.Location.Y) > this.MoveRange)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Virtual Method region

        #endregion
    }
}
