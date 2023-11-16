using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS.interfaces
{
    public interface IState
    {
        bool x_to_move { get; set; }
        IList<INode> find_moves();
        void play(INode node);
        int random_play(Random rnd);
        IState copy();
        bool is_finished(out int score);
    }
}
