using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class ScoreRow
    {
        public string PlayerName { get; set; } = Consts.TEXT_BLANK_SPACE;

        public string ScoreForDisplay { get; set; } = Consts.TEXT_BLANK_SPACE;

        public int Score { get ; set; } = 0;

        public string Level { get; set; } = Consts.TEXT_BLANK_SPACE_SHORT;

        public ScoreRow(){}

        


    }
}
