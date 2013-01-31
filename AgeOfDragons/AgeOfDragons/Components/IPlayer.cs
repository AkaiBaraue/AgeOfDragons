// -----------------------------------------------------------------------
// <copyright file="IPlayer.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace AgeOfDragons.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class IPlayer
    {
        public Boolean IsTurnFinished { get; private set; }

        public Boolean IsCurrentPlayer { get; private set; }

        public IPlayer NextPlayer { get; set; }

        public abstract void StartTurn();

        public abstract void TakeTurn();

        public abstract void EndTurn();

        public IPlayer GetNextPlayer()
        {
            return this.NextPlayer;
        }
    }
}
