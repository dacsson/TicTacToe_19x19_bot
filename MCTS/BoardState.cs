using MCTS.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCTS;

public class BoardState : IState
{
    // Кто сейчас ходит
    public bool x_to_move { get; set; }
    
    // Битовая доска игрока Х
    public uint[] x_board;
    
    // Битовая доска игрока О
    public uint[] o_board;

    private static readonly uint[] _win_patterns_rows =
    {
        0b1111100000000000000,
        0b0111110000000000000,
        0b0011111000000000000,
        0b0001111100000000000,
        0b0000111110000000000,
        0b0000011111000000000,
        0b0000001111100000000,
        0b0000000111110000000,
        0b0000000011111000000,
        0b0000000001111100000,
        0b0000000000111110000,
        0b0000000000011111000,
        0b0000000000001111100,
        0b0000000000000111110,
        0b0000000000000011111,
    };

    private static readonly uint[] _win_patterns_diagnal =
    {
        0b1000000000000000000,
        0b0100000000000000000,
        0b0010000000000000000,
        0b0001000000000000000,
        0b0000100000000000000,
    };

    public BoardState() 
    {
        x_to_move= true;
        x_board = Enumerable.Repeat((uint)0b0000000000000000000, 19).ToArray();
        o_board = Enumerable.Repeat((uint)0b0000000000000000000, 19).ToArray();
    }

    public IState copy()
    {
        // Increase perfomance with blockcopy isnted of Array.Copy or Clone
        uint[] _x_board = new uint[19]; 
        uint[] _o_board = new uint[19];
        System.Buffer.BlockCopy(x_board, 0, _x_board, 0, 19 * sizeof(uint));
        System.Buffer.BlockCopy(o_board, 0, _o_board, 0, 19 * sizeof(uint));
        return new BoardState { x_to_move = x_to_move, o_board = _o_board, x_board = _x_board };
    }

    private uint[] find_full_board()
    {
        uint[] full_board = new uint[19];
        for (byte i = 0; i < 19; i++)
        {
            full_board[i] = x_board[i] | o_board[i];
        }

        return full_board;
    }

    // Найти все возможных ходы (узлы дерева в виде битовой доски поля)
    // запоминаем позицию строки и столба на которую ставим фигуру
    // затем будем сдвигать на i, т.е. 1 << i в строке j чтобы подставить
    // строка подразумевает номер бита в числе
    public IList<INode> find_moves()
    {
        var possible_moves = new List<INode>(361);
        uint[] full_board = find_full_board();
        for (byte i = 0; i < 19; i++)
        {
            for(byte j = 0; j < 19; j++)
            {
                // умножить на i-ый байт чтобы узнать вакантно ли место 
                // i - строка j - столбец
                if ( (full_board[i] & (1 << j) ) == 0) possible_moves.Add(new Node { place_figure = new Tuple<byte, byte>(i, j) });
            }
        }
        return possible_moves.ToList();
    }

    // Соответствует ли доска состоянию выигрыша победы в ряд
    // @ TODO
    // @ ОПТИМИЗАЦИЯ ПУТёМ БИТОВОГО СДВИГА
    private bool check_if_board_win_pattern_horizontal(uint[] board)
    {
        // horizontal win
        for(byte i = 0; i < 15; i++)
        {
            // проверяем все возможные состояние доски при победе в ряд
            uint[] horizontal_check = new uint[19];
            uint[] win_pattern = new uint[19];
            horizontal_check[0] = board[0] & _win_patterns_rows[i];
            win_pattern[0] = _win_patterns_rows[i];
            for (byte j = 1; j < 19; j++)
            {
                horizontal_check[j] = board[j] & 0b0000000000000000000;
                win_pattern[j] = 0b0000000000000000000;
            }
            if (new HashSet<uint>(horizontal_check).SetEquals(win_pattern)) return true;
        }

        return false;
    }

    // Соответствует ли доска состоянию выигрыша победы в столбец
    private bool check_if_board_win_pattern_vertical(uint[] board)
    {
        // По сути транспонирование матрицы (если представить числа как биты в 19) 19х19
        // ну или поворот доски)
        uint[] transp_board = new uint[19];
        for(byte i = 0; i < 19; i ++)
        {
            for(byte j = 0; j < 19; j++)
            {
                transp_board[i] |= ((board[j] << i) & (uint)0b1000000000000000000) >> j;
            }
        }
        return check_if_board_win_pattern_horizontal(transp_board);
    }

    // Заметка: проверка происходит на диагональ слева направо
    // для проверки справа налево просто реверсируем массив выигрыш состояний
    private bool check_if_board_win_pattern_diagnall(uint[] board, bool left_to_right)
    {
        uint[] _win_patterns = new uint[5];
        uint[] diagnal_check = new uint[5];
        // horizontal win byte i = 0; i < 5; i++
        for(byte h = 0; h < 19; h++)
        {
            // Сдвигаем матрицу победы наискок чтобы посмотреть все варианты a.k.a
            // 1, 0, 0, 0, 0         0, 1, 0, 0, 0
            // 0, 1, 0, 0, 0   =>    0, 0, 1, 0, 0
            // 0, 0, 1, 0, 0         0, 0, 0, 1, 0
            // ....                     ....
            _win_patterns = (uint[])_win_patterns_diagnal.Clone();

            if (left_to_right) _win_patterns.Reverse();

            _win_patterns[0] >>= h;
            _win_patterns[1] >>= h;
            _win_patterns[2] >>= h;
            _win_patterns[3] >>= h;
            _win_patterns[4] >>= h;
            // Заполняем проверочную матрицу 5x19 (т.к. 5 наискосок)
            for (byte i = 0; i < 5; i++)
            {
                // Проходимся шагом 5, иначе говоря рассматриваем все подматрицы 5x19 матрицы 19x19
                for (byte count = 0; count < 14; count += 5)
                {
                    // проверяем все возможные состояние доски при победе в ряд
                    diagnal_check = new uint[5];
                    diagnal_check[i] = board[count] & _win_patterns[i];
                }
                if (new HashSet<uint>(diagnal_check).SetEquals(_win_patterns)) return true;
            }
        }

        return false;
    }

    public bool is_finished(out int score)
    {
        if (check_if_board_win_pattern_horizontal(x_board) || 
            check_if_board_win_pattern_vertical(x_board) || 
            check_if_board_win_pattern_diagnall(x_board, true) ||
            check_if_board_win_pattern_diagnall(x_board, false)
        )
        {
            score = 1;
            return true;
        }
        // CheckForBlackWin
        if (check_if_board_win_pattern_horizontal(o_board) || 
            check_if_board_win_pattern_vertical(o_board) || 
            check_if_board_win_pattern_diagnall(o_board, true) ||
            check_if_board_win_pattern_diagnall(o_board, false)
        )
        {
            score = -1;
            return true;
        }
        // CheckIfDraw
        score = 0;
        uint[] full_board = find_full_board();
        foreach (var row in full_board)
        {
            if (row != 0x7FFFF) return false;
        }
        return true;
    }

    // Сделать ход
    public void play(INode _node)
    {
        Node node = (Node)_node;
        if (x_to_move)
        {
            x_board[node.place_figure.Item1] |= (uint)(1 << node.place_figure.Item2);
        }
        else
        {
            o_board[node.place_figure.Item1] |= (uint)(1 << node.place_figure.Item2);
        }

        x_to_move = !x_to_move;
    }

    // Симуляция различных исходов и возможных постановок фигуры
    public int random_play(Random rnd)
    {
        var possible_moves = new Tuple<byte, byte>[361];
        uint[] full_board = find_full_board();
        short number_of_moves = 0;
        for (byte i = 0; i < 19; i++)
        {
            for (byte j = 0; j < 19; j++)
            {
                if ((full_board[i] & (1 << j)) == 0) possible_moves[number_of_moves++] = new Tuple<byte, byte>(i, j);
            }
        }

        var _place_figure = possible_moves[rnd.Next(number_of_moves)];
        play(new Node { place_figure = _place_figure });
        return number_of_moves;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (byte i = 0; i < 19; i++)
        {
            for(byte j = 0; j < 19; j++)
            {
                if ((x_board[i] & (1 << j)) != 0)
                    sb.Append("x");
                else if ((o_board[i] & (1 << j)) != 0)
                    sb.Append("o");
                else
                    sb.Append("_");

                if (j % 19 == 18)
                    sb.AppendLine();
            }
        }
        return sb.ToString();
    }
}
