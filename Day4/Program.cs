using System.Diagnostics.CodeAnalysis;

class Program
{
    static string[] input;
    static int rows;
    static int columns;
    static void Main(string[] args)
    {
        input = File.ReadAllLines("Input.txt");

        rows = input.Length;
        columns = input[0].Length;

        var count = 0;
        var count2 = 0;
        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                count += CountAllFromPoint(i, j);
            }
        }

        for(int i = 1; i < rows - 1; i++)
        {
            for(int j = 1; j <columns - 1; j++)
            {
                count2 += CountMASFromPoint(i, j);
            }
        }

        Console.WriteLine("The answer to day 4 is: " + count);
        Console.WriteLine("The answer to day part 2 is: " + count2);
    }

    static int CountAllFromPoint(int i, int j)
    {
        if (input[i][j] != 'X') return 0;

        var count = 0;


        // up
        if (i - 3 >= 0 && input[i - 1][j] == 'M' && input[i - 2][j] == 'A' && input[i - 3][j] == 'S')
            count++;

        // down
        if (i + 3 < rows && input[i + 1][j] == 'M' && input[i + 2][j] == 'A' && input[i + 3][j] == 'S')
            count++;

        // left
        if (j - 3 >= 0 && input[i][j - 1] == 'M' && input[i][j - 2] == 'A' && input[i][j - 3] == 'S')
            count++;

        // right
        if (j + 3 < columns && input[i][j + 1] == 'M' && input[i][j + 2] == 'A' && input[i][j + 3] == 'S')
            count++;

        // up-left
        if (i - 3 >= 0 && j - 3 >= 0 && input[i - 1][j - 1] == 'M' && input[i - 2][j - 2] == 'A' && input[i - 3][j - 3] == 'S')
            count++;

        // up-right
        if (i - 3 >= 0 && j + 3 < columns && input[i - 1][j + 1] == 'M' && input[i - 2][j + 2] == 'A' && input[i - 3][j + 3] == 'S')
            count++;

        // down-left
        if (i + 3 < rows && j - 3 >= 0 && input[i + 1][j - 1] == 'M' && input[i + 2][j - 2] == 'A' && input[i + 3][j - 3] == 'S')
            count++;

        // down-right
        if (i + 3 < rows && j + 3 < columns && input[i + 1][j + 1] == 'M' && input[i + 2][j + 2] == 'A' && input[i + 3][j + 3] == 'S')
            count++;

        return count;
    }

    static int CountMASFromPoint(int i, int j)
    {
        if (input[i][j] != 'A') return 0;

        // Check upper-left + lower-right diagonal combined with upper-right + lower-left diagonal
        var topLeftMAS = i > 0 && j > 0 && input[i - 1][j - 1] == 'M' && input[i + 1][j + 1] == 'S';
        var topLeftSAM = i > 0 && j > 0 && input[i - 1][j - 1] == 'S' && input[i + 1][j + 1] == 'M';
        var topRightMAS = i > 0 && j < columns - 1 && input[i - 1][j + 1] == 'M' && input[i + 1][j - 1] == 'S';
        var topRightSAM = i > 0 && j < columns - 1 && input[i - 1][j + 1] == 'S' && input[i + 1][j - 1] == 'M';

        if ((topLeftMAS || topLeftSAM) && (topRightMAS || topRightSAM)) return 1;

        return 0;
    }
}
