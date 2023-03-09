using Shared.Enums;

namespace BusinessLogic.Helpers;

public static class StringHelper
{
    public static CellStatus[][] GetBoardFromString(string str)
    {
        var strArray = str.Split('|').Select(s => s.Split(',')).ToArray();

        var board = new[]
        {
            new[] { CellStatus.Empty, CellStatus.Empty, CellStatus.Empty },
            new[] { CellStatus.Empty, CellStatus.Empty, CellStatus.Empty },
            new[] { CellStatus.Empty, CellStatus.Empty, CellStatus.Empty }
        };

        for (var i = 0; i < strArray.Length; i++)
        {
            for (var j = 0; j < strArray[i].Length; j++)
            {
                board[i][j] = (CellStatus) int.Parse(strArray[i][j]);
            }
        }

        return board;
    }

    public static string GetStringFromBoard(CellStatus[][] board)
    {
        var str = "";

        for (var i = 0; i < board.Length; i++)
        {
            for (var j = 0; j < board[i].Length; j++)
            {
                if (j == 2)
                {
                    str += (int) board[i][j];
                }
                else
                {
                    str += (int) board[i][j] + ",";
                }
            }

            if (i != 2)
            {
                str += "|";
            }
        }

        return str;
    }
}