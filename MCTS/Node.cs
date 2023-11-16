using MCTS.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MCTS;

public class Node : INode
{
    public long number_of_visits { get; set; }
    public long score { get; set; }
    public IList<INode> children { get ; set ; }

    // Константы для UCB
    private const double C = 1.4;

    // На какой байт ставим фигуру, какая строка
    public Tuple<byte, byte> place_figure { get; set; }

    /// <summary>.
    /// Стратегия выбора наилучшего узла с использованием формулы UCB
    /// Si = xi + C (const) * sqrt( lg(N) / ni )
    /// xi - ср. значение узла
    /// N - количество симуляций у родителя
    /// ni - количество симуляций у выбранного дитя
    /// </summary>
    /// <param name="child">Выбранный узел</param>
    /// <returns></returns>
    public double compute(INode child)
    {
        //Console.WriteLine($"computing {child.number_of_visits} " );
        return (child.score / Convert.ToDouble(child.number_of_visits) + C * Math.Sqrt(Math.Log(number_of_visits) / child.number_of_visits));
    }

    /// <summary>
    /// Найти наилучший узел исходя из его UCB значения
    /// </summary>
    /// <returns></returns>
    public INode select_node()
    {
        Node bestChild = new Node();
        double bestUCB = double.NegativeInfinity;
        
        foreach (Node child in children.ToList())
        {
            if (child.number_of_visits == 0)
                return child;

            double UCB = compute(child);
            if (UCB > bestUCB)
            {
                bestUCB = UCB;
                bestChild = child;
            }
        }
        return bestChild;
    }
}
