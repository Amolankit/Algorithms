using System;

class MiniMaxTicTacToe
{
    // Represents the tic-tac-toe board
    public class Board
    {
        public char[,] State { get; private set; }

        public Board()
        {
            State = new char[3, 3];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    State[i, j] = ' ';
                }
            }
        }

        public void PrintBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(State[i, j] + " | ");
                }
                Console.WriteLine();
                Console.WriteLine("---------");
            }
        }

        public bool IsMoveValid(int row, int col)
        {
            return row >= 0 && row < 3 && col >= 0 && col < 3 && State[row, col] == ' ';
        }

        public void MakeMove(int row, int col, char player)
        {
            if (IsMoveValid(row, col))
            {
                State[row, col] = player;
            }
        }

        public void UndoMove(int row, int col)
        {
            if (IsMoveValid(row, col))
            {
                State[row, col] = ' ';
            }
        }

        public bool IsGameOver()
        {
            return IsWin('X') || IsWin('O') || IsBoardFull();
        }

        private bool IsWin(char player)
        {
            // Check rows and columns
            for (int i = 0; i < 3; i++)
            {
                if ((State[i, 0] == player && State[i, 1] == player && State[i, 2] == player) ||
                    (State[0, i] == player && State[1, i] == player && State[2, i] == player))
                {
                    return true;
                }
            }

            // Check diagonals
            return (State[0, 0] == player && State[1, 1] == player && State[2, 2] == player) ||
                   (State[0, 2] == player && State[1, 1] == player && State[2, 0] == player);
        }

        private bool IsBoardFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (State[i, j] == ' ')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    // MiniMax algorithm
    public class MiniMax
    {
        public static int[] FindBestMove(Board board, char player)
        {
            int[] bestMove = new int[] { -1, -1 };
            int bestScore = int.MinValue;

            // Iterate through all empty cells to find the best move
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board.IsMoveValid(i, j))
                    {
                        // Make the move
                        board.MakeMove(i, j, player);

                        // Calculate the score for this move
                        int score = MiniMaxAlgorithm(board, 0, false);

                        // Undo the move
                        board.UndoMove(i, j);

                        // Update the best move if a higher score is found
                        if (score > bestScore)
                        {
                            bestScore = score;
                            bestMove[0] = i;
                            bestMove[1] = j;
                        }
                    }
                }
            }

            return bestMove;
        }

        private static int MiniMaxAlgorithm(Board board, int depth, bool isMaximizingPlayer)
        {
            if (board.IsGameOver())
            {
                // Return the score based on the winner or a draw
                if (board.IsWin('X')) return -1;
                else if (board.IsWin('O')) return 1;
                else return 0;
            }

            if (isMaximizingPlayer)
            {
                int bestScore = int.MinValue;

                // Iterate through all empty cells
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board.IsMoveValid(i, j))
                        {
                            // Make the move
                            board.MakeMove(i, j, 'O');

                            // Recursively calculate the score
                            int score = MiniMaxAlgorithm(board, depth + 1, false);

                            // Undo the move
                            board.UndoMove(i, j);

                            // Update the best score
                            bestScore = Math.Max(bestScore, score);
                        }
                    }
                }

                return bestScore;
            }
            else
            {
                int bestScore = int.MaxValue;

                // Iterate through all empty cells
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (board.IsMoveValid(i, j))
                        {
                            // Make the move
                            board.MakeMove(i, j, 'X');

                            // Recursively calculate the score
                            int score = MiniMaxAlgorithm(board, depth + 1, true);

                            // Undo the move
                            board.UndoMove(i, j);

                            // Update the best score
                            bestScore = Math.Min(bestScore, score);
                        }
                    }
                }

                return bestScore;
            }
        }
    }

    static void Main(string[] args)
    {
        Board ticTacToeBoard = new Board();
        char currentPlayer = 'X';

        while (!ticTacToeBoard.IsGameOver())
        {
            ticTacToeBoard.PrintBoard();

            int[] move;

            if (currentPlayer == 'X')
            {
                // Human player's turn
                Console.WriteLine("Enter your move (row and column, separated by a space): ");
                string[] input = Console.ReadLine().Split(' ');
                int row = int.Parse(input[0]);
                int col = int.Parse(input[1]);
                move = new int[] { row, col };
            }
            else
            {
                // AI player's turn (using MiniMax)
                move = MiniMax.FindBestMove(ticTacToeBoard, currentPlayer);
                Console.WriteLine($"AI chooses move: {move[0]}, {move[1]}");
            }

            // Make the move
            ticTacToeBoard.MakeMove(move[0], move[1], currentPlayer);

            // Switch player for the next turn
            currentPlayer = (currentPlayer == 'X') ? 'O' : 'X';
        }

        ticTacToeBoard.PrintBoard();

        // Determine the winner or declare a draw
    }
