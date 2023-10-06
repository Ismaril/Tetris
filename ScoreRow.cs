using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class ScoreRow
    {
        // -----------------------------------------------------------------------------------------
        // PROPERTIES

        /// <summary>
        /// Players name which will be displayed at scoreboard.
        /// </summary>
        public string PlayerName { get; set; } = Consts.TEXT_BLANK_SPACE;

        /// <summary>
        /// Players score which will be displayed at scoreboard.
        /// </summary>
        public int Score { get ; set; } = 0;

        /// <summary>
        /// Highest level where the player has ended. This number will be displayed at scoreboard.
        /// </summary>
        public string Level { get; set; } = Consts.TEXT_BLANK_SPACE_SHORT;

        // -----------------------------------------------------------------------------------------
        // CONSTRUCTOR
        public ScoreRow(){}
    }
}
