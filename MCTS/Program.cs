﻿using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace MCTS;

class Program
{
    static void Main(string[] args)
    {
        var _searcher = new MCTSSearcher();
        var _root_state = new BoardState();

        List<double> _time = new List<double>();
        while (!_root_state.is_finished(out int score))
        {
            var sw = Stopwatch.StartNew();
            var root_node = new Node();
            var best_move = _searcher.get_best_move(root_node, _root_state, 50);

            _time.Add(sw.Elapsed.TotalNanoseconds);
            Console.WriteLine($"Time used: {sw.Elapsed}");

            _root_state.play(best_move);

            Console.WriteLine(_root_state);

        }

        Console.WriteLine($"\n who won? {_time.Average()} ");

        //var summary = BenchmarkRunner.Run<MCTSSearcher>();

        //var _runner = new MCTSRunner('x');
        //var str = "xxxx____o_o_o_o__________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________________";
        //BoardState state = (BoardState)_runner.generate_bitboard(str);
        //int score = 0;
        //Console.WriteLine(state.is_finished(out score));
        //Console.WriteLine(state);

        //var sw = Stopwatch.StartNew();
        //Console.WriteLine(_runner.Run(str));
        //Console.WriteLine($"Time used: {sw.Elapsed}");
    }
}
