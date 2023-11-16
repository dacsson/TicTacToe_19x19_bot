using BenchmarkDotNet.Columns;
using MCTS.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS;

// Wrapper for running MCTS by providing board as string
public class MCTSRunner
{
    private readonly char _figure;
    public MCTSRunner(char figure)
    {
        _figure = figure;
    }

    public IState generate_bitboard(string board_state)
    {
        BoardState _root_state = new BoardState();
        int count_x = 0;
        int count_o = 0;
        uint[] _x_board = Enumerable.Repeat((uint)0b0000000000000000000, 19).ToArray();
        uint[] _o_board = Enumerable.Repeat((uint)0b0000000000000000000, 19).ToArray();

        int c = 0;
        //for(int i = 0; i < board_state.Length; i++)
        Parallel.For(0, 361, i =>
        {
            if (i % 19 == 18)
            {
                string line = board_state.Substring(i - 18, 19);
                //Console.WriteLine(line);
                for (int j = 0; j < line.Length; j++)
                {
                    if (line[j] == 'x')
                        _x_board[c] |= _x_board[c] | (uint)(1 << j);
                    else if (line[j] == 'o')
                        _o_board[c] |= _o_board[c] | (uint)(1 << j);
                }
                c++;
            }
        });

        // Кто ходит
        bool _x_to_move = false;
        if (_figure == 'x')
            _x_to_move = true;

        _root_state = new BoardState
        {
            x_to_move = _x_to_move,
            x_board = _x_board,
            o_board = _o_board
        };

        //int count = 0;
        //foreach(var row in _x_board)
        //{
        //    Console.Write($" {Convert.ToString(row, 2)}");
        //    count++;
        //    if (count % 19 == 0) Console.WriteLine();
        //}
        //Console.WriteLine();

        //count = 0;
        //foreach (var row in _o_board)
        //{
        //    Console.Write($" {Convert.ToString(row, 2)}");
        //    count++;
        //    if (count % 19 == 0) Console.WriteLine();
        //}
        //Console.WriteLine();

        return _root_state;
    }

    public string Run(string board_state)
    {
        var _searcher = new MCTSSearcher();
        var _root_state = generate_bitboard(board_state);

        var root_node = new Node();
        var best_move = _searcher.get_best_move(root_node, _root_state, 100000);


        _root_state.play(best_move);

        return _root_state.ToString();
    }
}