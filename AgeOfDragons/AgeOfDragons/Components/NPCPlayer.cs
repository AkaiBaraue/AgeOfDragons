// -----------------------------------------------------------------------
// <copyright file="NPCPlayer.cs" company="">
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
    public class NPCPlayer : IPlayer
    {
        #region Field Region
        #endregion

        #region Property Region
        #endregion

        #region Constructor Region

        public NPCPlayer(IPlayer nextPlayer)
        {
            this.NextPlayer = nextPlayer;
        }

        #endregion

        #region Method Region
        #endregion

        #region Virtual Method region
        #endregion

        public override void TakeTurn()
        {
            throw new NotImplementedException();
        }

        public override void EndTurn()
        {
            throw new NotImplementedException();
        }
    }
}
