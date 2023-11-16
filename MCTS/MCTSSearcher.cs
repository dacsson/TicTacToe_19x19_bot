using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BenchmarkDotNet.Attributes;
using MCTS.interfaces;

namespace MCTS;

public class MCTSSearcher
{
    protected readonly Random _random = new Random();

    [Benchmark]
    public INode bot_call_10_first_move()
    {
        INode rootNode = new Node();
        IState rootState = new BoardState();
        int iter = 10;
        //Console.WriteLine("\nSearching for best solution...");
        Parallel.For(0, iter, i =>
        {
            var gameState = rootState.copy();

            var path = select_nodes(gameState, rootNode);
            //Console.WriteLine($"for {i}: path: ");
            //foreach(var node in path)
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
            if (!gameState.is_finished(out int score))
            {
                var node = path.Last();

                expand_node(gameState, node);

                node = node.children.First();
                //Console.WriteLine($"\n next x {((BoardState)rootState).x_board} o {((BoardState)rootState).o_board}");
                gameState.play(node);
                path.Add(node);

                score = simulate_random_outcome(gameState);
                //Console.WriteLine($"{score}");
            }
            back_propogation(path, score, rootState.x_to_move);
            //Console.WriteLine($"for {i}: rootnode: ");
            //foreach (var node in rootNode.children.ToList())
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
        });
        //Console.WriteLine("\n finding best child", rootNode.children.Count);
        //foreach (var child in rootNode.children)
        //{
        //    Console.WriteLine(" ", rootNode.compute(child).ToString());
        //}
        return rootNode.select_node();
    }

    [Benchmark]
    public INode bot_call_50_first_move()
    {
        INode rootNode = new Node();
        IState rootState = new BoardState();
        int iter = 50;
        //Console.WriteLine("\nSearching for best solution...");
        Parallel.For(0, iter, i =>
        {
            var gameState = rootState.copy();

            var path = select_nodes(gameState, rootNode);
            //Console.WriteLine($"for {i}: path: ");
            //foreach(var node in path)
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
            if (!gameState.is_finished(out int score))
            {
                var node = path.Last();

                expand_node(gameState, node);

                node = node.children.First();
                //Console.WriteLine($"\n next x {((BoardState)rootState).x_board} o {((BoardState)rootState).o_board}");
                gameState.play(node);
                path.Add(node);

                score = simulate_random_outcome(gameState);
                //Console.WriteLine($"{score}");
            }
            back_propogation(path, score, rootState.x_to_move);
            //Console.WriteLine($"for {i}: rootnode: ");
            //foreach (var node in rootNode.children.ToList())
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
        });
        //Console.WriteLine("\n finding best child", rootNode.children.Count);
        //foreach (var child in rootNode.children)
        //{
        //    Console.WriteLine(" ", rootNode.compute(child).ToString());
        //}
        return rootNode.select_node();
    }

    [Benchmark]
    public INode bot_call_10_midgame()
    {
        INode rootNode = new Node();
        IState rootState = new BoardState
        {
            x_to_move = true,
            x_board = new uint[19]
            {
                0b11000_00000_00000_00000_00000,
                0b00000_11000_00000_00000_00000,
                0b00000_00000_11000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_01000_00000_00000,
                0b00000_01000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00110_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00110_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00111_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000
            },
            o_board = new uint[19]
            {
                0b00110_00000_00000_00000_00000,
                0b00000_00110_00000_00000_00000,
                0b00000_00000_00110_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00100_00000_00000,
                0b00000_00100_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00001_10000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00110_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_11100_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00001
            },
        };
        int iter = 10;
        //Console.WriteLine("\nSearching for best solution...");
        Parallel.For(0, iter, i =>
        {
            var gameState = rootState.copy();

            var path = select_nodes(gameState, rootNode);
            //Console.WriteLine($"for {i}: path: ");
            //foreach(var node in path)
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
            if (!gameState.is_finished(out int score))
            {
                var node = path.Last();

                expand_node(gameState, node);

                node = node.children.First();
                //Console.WriteLine($"\n next x {((BoardState)rootState).x_board} o {((BoardState)rootState).o_board}");
                gameState.play(node);
                path.Add(node);

                score = simulate_random_outcome(gameState);
                //Console.WriteLine($"{score}");
            }
            back_propogation(path, score, rootState.x_to_move);
            //Console.WriteLine($"for {i}: rootnode: ");
            //foreach (var node in rootNode.children.ToList())
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
        });
        //Console.WriteLine("\n finding best child", rootNode.children.Count);
        //foreach (var child in rootNode.children)
        //{
        //    Console.WriteLine(" ", rootNode.compute(child).ToString());
        //}
        return rootNode.select_node();
    }

    [Benchmark]
    public INode bot_call_50_midgame()
    {
        INode rootNode = new Node();
        IState rootState = new BoardState
        {
            x_to_move = true,
            x_board = new uint[19]
            {
                0b11000_00000_00000_00000_00000,
                0b00000_11000_00000_00000_00000,
                0b00000_00000_11000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_01000_00000_00000,
                0b00000_01000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00110_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00110_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00111_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000
            },
            o_board = new uint[19]
            {
                0b00110_00000_00000_00000_00000,
                0b00000_00110_00000_00000_00000,
                0b00000_00000_00110_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00100_00000_00000,
                0b00000_00100_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00001_10000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00110_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_11100_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00001
            },
        };
        int iter = 50;
        //Console.WriteLine("\nSearching for best solution...");
        Parallel.For(0, iter, i =>
        {
            var gameState = rootState.copy();

            var path = select_nodes(gameState, rootNode);
            //Console.WriteLine($"for {i}: path: ");
            //foreach(var node in path)
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
            if (!gameState.is_finished(out int score))
            {
                var node = path.Last();

                expand_node(gameState, node);

                node = node.children.First();
                //Console.WriteLine($"\n next x {((BoardState)rootState).x_board} o {((BoardState)rootState).o_board}");
                gameState.play(node);
                path.Add(node);

                score = simulate_random_outcome(gameState);
                //Console.WriteLine($"{score}");
            }
            back_propogation(path, score, rootState.x_to_move);
            //Console.WriteLine($"for {i}: rootnode: ");
            //foreach (var node in rootNode.children.ToList())
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
        });
        //Console.WriteLine("\n finding best child", rootNode.children.Count);
        //foreach (var child in rootNode.children)
        //{
        //    Console.WriteLine(" ", rootNode.compute(child).ToString());
        //}
        return rootNode.select_node();
    }

    [Benchmark]
    public INode bot_call_361_midgame()
    {
        INode rootNode = new Node();
        IState rootState = new BoardState
        {
            x_to_move = true,
            x_board = new uint[19]
            {
                0b11000_00000_00000_00000_00000,
                0b00000_11000_00000_00000_00000,
                0b00000_00000_11000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_01000_00000_00000,
                0b00000_01000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00110_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00110_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00111_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000
            },
            o_board = new uint[19]
            {
                0b00110_00000_00000_00000_00000,
                0b00000_00110_00000_00000_00000,
                0b00000_00000_00110_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00100_00000_00000,
                0b00000_00100_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00001_10000_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00110_00000_00000_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_11100_00000,
                0b00000_00000_00000_00000_00000,
                0b00000_00000_00000_00000_00001
            },
        };
        int iter = 361;
        //Console.WriteLine("\nSearching for best solution...");
        Parallel.For(0, iter, i =>
        {
            var gameState = rootState.copy();

            var path = select_nodes(gameState, rootNode);
            //Console.WriteLine($"for {i}: path: ");
            //foreach(var node in path)
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
            if (!gameState.is_finished(out int score))
            {
                var node = path.Last();

                expand_node(gameState, node);

                node = node.children.First();
                //Console.WriteLine($"\n next x {((BoardState)rootState).x_board} o {((BoardState)rootState).o_board}");
                gameState.play(node);
                path.Add(node);

                score = simulate_random_outcome(gameState);
                //Console.WriteLine($"{score}");
            }
            back_propogation(path, score, rootState.x_to_move);
            //Console.WriteLine($"for {i}: rootnode: ");
            //foreach (var node in rootNode.children.ToList())
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
        });
        //Console.WriteLine("\n finding best child", rootNode.children.Count);
        //foreach (var child in rootNode.children)
        //{
        //    Console.WriteLine(" ", rootNode.compute(child).ToString());
        //}
        return rootNode.select_node();
    }

    public INode get_best_move(INode rootNode, IState rootState, int iter)
    {
        //Console.WriteLine("\nSearching for best solution...");
        Parallel.For(0, iter, i =>
        {
            var gameState = rootState.copy();

            var path = select_nodes(gameState, rootNode);
            //Console.WriteLine($"for {i}: path: ");
            //foreach(var node in path)
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
            if (!gameState.is_finished(out int score))
            {
                var node = path.Last();

                expand_node(gameState, node);

                node = node.children.First();
                //Console.WriteLine($"\n next x {((BoardState)rootState).x_board} o {((BoardState)rootState).o_board}");
                gameState.play(node);
                path.Add(node);

                score = simulate_random_outcome(gameState);
                //Console.WriteLine($"{score}");
            }
            back_propogation(path, score, rootState.x_to_move);
            //Console.WriteLine($"for {i}: rootnode: ");
            //foreach (var node in rootNode.children.ToList())
            //{
            //    Console.WriteLine($"{node.score}, {node.number_of_visits}, {((Node)node).place_figure}");
            //}
        });
        //Console.WriteLine("\n finding best child", rootNode.children.Count);
        //foreach (var child in rootNode.children)
        //{
        //    Console.WriteLine(" ", rootNode.compute(child).ToString());
        //}
        return rootNode.select_node();
    }

    /// <summary>
    /// Step 1. Selection
    /// </summary>
    /// <param name="gameState"></param>
    /// <param name="node"></param>
    /// <returns></returns>
    public List<INode> select_nodes(IState gameState, INode node)
    {
        var path = new List<INode>
        {
            node
        };

        while (node.children != null)
        {
            // Выбирает наилучший дитя узел на основе UCB
            node = node.select_node();
            
            gameState.play(node);
            path.Add(node);
        }
        return path;
    }

    /// <summary>
    /// Step 2. Expansion
    /// Добавление нового узла к тому который выбрали в первом шаге
    /// </summary>
    /// <param name="gameState"></param>
    /// <param name="node"></param>
    public void expand_node(IState gameState, INode node)
    {
        node.children = gameState.find_moves();
        // Shuffle
        for (int i = node.children.Count - 1; i > 0; i--)
        {
            var k = _random.Next(i + 1);
            var value = node.children[k];
            node.children[k] = node.children[i];
            node.children[i] = value;
        }
    }

    /// <summary>
    /// Step 3. Simulation
    /// Симуляция ходов пока не найдём выигрышный 
    /// </summary>
    /// <param name="gameState"></param>
    /// <returns></returns>
    public int simulate_random_outcome(IState gameState)
    {
        int score;
        int number_of_moves = 1;
        while (!gameState.is_finished(out score))
        {
            gameState.random_play(_random);
            //if (number_of_moves == 0)
            //    return 0;
        }
        return score;
    }

    /// <summary>
    /// Step 4. Backpropagation
    /// Обновление дерева возвращение к родителю
    /// </summary>
    /// <param name="path"></param>
    /// <param name="score"></param>
    /// <param name="x_to_move"></param>
    public void back_propogation(List<INode> path, int score, bool x_to_move)
    {
        foreach(var node in path)
        {
            node.number_of_visits++;

            if (!x_to_move)
                node.score += score;
            else
                node.score -= score;

            x_to_move = !x_to_move;
        }
        //foreach (var node in path)
        //{
        //    node.number_of_visits++;

        //    if (!x_to_move)
        //        node.score += score;
        //    else
        //        node.score -= score;

        //    x_to_move = !x_to_move;
        //}
    }
}
