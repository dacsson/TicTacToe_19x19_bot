using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS.interfaces
{
    public interface INode
    {
        long number_of_visits { get; set; }
        long score { get; set; }
        IList<INode> children { get; set; }
        double compute(INode child);
        INode select_node();
    }
}
