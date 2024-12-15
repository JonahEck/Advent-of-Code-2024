using System.Linq;
using System.Reflection.Metadata.Ecma335;

class Program
{
    static List<int[]> traversedLocations = new List<int[]>();
    static int rows;
    static int columns;
    static void Main(string[] args)
    {
       
        string[] input = File.ReadAllLines("Input.txt");
        rows = input.Length;
        columns = input[0].Length;

        int[] startPos = FindStartingPos(input);
        if (startPos == null)
        {
            throw new Exception("No starting position found in input data");
        }
        string[] inputCopy = (string[])input.Clone();
        var uniquePosCount = CalcUniquePos(startPos, input);
        var obstructionCount = ObstructionCount(inputCopy);
        Console.WriteLine("The answer to day 6 is: " + uniquePosCount);
        Console.WriteLine("The answer to day 6 part 2 is: " + obstructionCount);
    }

    static int[] FindStartingPos(string[] grid)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                var currentChar = grid[i][j];
                if (currentChar == '^' || currentChar == '>' || currentChar == '<' || currentChar == 'V')
                {
                    return [i, j];
                }
            }
        }
        return null; //We shouldn't get here
    }

    static int CalcUniquePos(int[] pos, string[] grid) 
    {
        bool isSolved = false;
        char movingDirection = ConvertChar(grid[pos[0]][pos[1]]);
        int uniquePosCount = 1; //Start at one
        //We'll just assume we start at a valid position
        //I don't feel like adding checks here ¯\_(ツ)_/¯
        while(!isSolved)
        {
            grid = SetCurrentPosChar(pos, movingDirection, grid);

            pos = MovePosition(pos, movingDirection);

            while(IsLegalPosition(pos, movingDirection, grid) == 1)
            {
                pos = ReversePosition(pos, movingDirection);
                movingDirection = RotatePosition(movingDirection);
                pos = MovePosition(pos, movingDirection);
            }

            if(IsLegalPosition(pos,movingDirection, grid) == 0)
            {
                if(grid[pos[0]][pos[1]] == '.') uniquePosCount++;
            }else
            {
                isSolved = true;
            }
        } 
        for(int i = 0; i < rows; i ++)
        {
            Console.WriteLine(grid[i]);
        }
        return uniquePosCount;
    }

    static int IsLegalPosition(int[] pos, char movingDirection, string[] grid)
    {
        //-----------------------------------------------
        // -1 means we are out of bounds or found a loop
        //  0 means we are valid and can proceed normally
        //  1 means we found a roadblock and need to backup and rotate
        //-----------------------------------------------
        if (pos[0] == -1 || pos[0] == rows || pos[1] == -1 || pos[1] == columns)
            return -1; //This position is out of bounds
        char nextPosition = grid[pos[0]][pos[1]]; //Grab our next position character
        if (nextPosition == '#')return 1; //A wall or object is valid
        //Check if we have already gone in this direction
        //If so that means we found a loop
        switch (movingDirection)
        {
            case '0': //We are going north
                List<char> NorthFacing = new List<char> { '0', '4', '5', '6', 'A', 'B', 'C', 'E' };
                if (NorthFacing.Contains(nextPosition))
                    return 2;
                break;
            case '1': //We are going south
                List<char> SouthFacing = new List<char> { '1', '4', '7', '8', 'A', 'B', 'D', 'E' };
                if (SouthFacing.Contains(nextPosition))
                    return 2;
                break;
            case '2': //We are going east
                List<char> EastFacing = new List<char> { '2', '5', '7', '9', 'A', 'C', 'D', 'E' };
                if (EastFacing.Contains(nextPosition))
                    return 2;
                break;
            case '3': //We are going west
                List<char> WestFacing = new List<char> { '3', '6', '8', '9', 'B', 'C', 'D', 'E' };
                if (WestFacing.Contains(nextPosition))
                    return 2;
                break;
        }
        return 0;
    }

    static string[] SetCurrentPosChar(int[] pos, char movingDirection, string[] grid)
    {
        char CurrentChar = grid[pos[0]][pos[1]];
        //If the Current space hasn't been naticated yet, just set 
        if (CurrentChar == '.' || CurrentChar == '^' || CurrentChar == 'V' || CurrentChar == '>' || CurrentChar == '<')
        {
            traversedLocations.Add(new int[] { pos[0], pos[1] });
            // Convert the string to a char array to modify a specific character
            char[] rowChars = grid[pos[0]].ToCharArray();
            rowChars[pos[1]] = movingDirection; // Update the character at the specific position
            grid[pos[0]] = new string(rowChars); // Replace the string in the input array
        }else
        {
            char newChar = ' ';
            switch (CurrentChar)
            {
                case '0':
                    if (movingDirection == '1') newChar = '4';
                    else if (movingDirection == '2') newChar = '5';
                    else if (movingDirection == '3') newChar = '6';
                    break;
                case '1':
                    if (movingDirection == '0') newChar = '4';
                    else if (movingDirection == '2') newChar = '7';
                    else if (movingDirection == '3') newChar = '8';
                    break;
                case '2':
                    if (movingDirection == '1') newChar = '7';
                    else if (movingDirection == '3') newChar = '9';
                    else if (movingDirection == '0') newChar = '5';
                    break;
                case '3':
                    if (movingDirection == '0') newChar = '6';
                    else if (movingDirection == '1') newChar = '8';
                    else if (movingDirection == '2') newChar = '9';
                    break;
                case '4':
                    if (movingDirection == '2') newChar = 'A';
                    else if (movingDirection == '3') newChar = 'B';
                    break;
                case '5':
                    if (movingDirection == '1') newChar = 'A';
                    else if (movingDirection == '3') newChar = 'C';
                    break;
                case '6':
                    if (movingDirection == '1') newChar = 'B';
                    else if (movingDirection == '2') newChar = 'C';
                    break;
                case '7':
                    if (movingDirection == '0') newChar = 'A';
                    else if (movingDirection == '3') newChar = 'D';
                    break;
                case '8':
                    if (movingDirection == '0') newChar = 'B';
                    else if (movingDirection == '2') newChar = 'D';
                    break;
                case '9':
                    if (movingDirection == '0') newChar = 'C';
                    else if (movingDirection == '1') newChar = 'D';
                    break;
                case 'A':
                    newChar = 'E';
                    break;
                case 'B':
                    newChar = 'E';
                    break;
                case 'C':
                    newChar = 'E';
                    break;
                case 'D':
                    newChar = 'E';
                    break;
                default:
                    //We shouldn't get here
                    Console.WriteLine("We are were we shoudn't be");
                    break;
            }
            char[] rowChars = grid[pos[0]].ToCharArray();
            rowChars[pos[1]] = newChar;
            grid[pos[0]] = new string(rowChars);
        }
        return grid;
    }

    static char ConvertChar(char c)
    {
        if (c == '^') return '0';
        if (c == 'V') return '1';
        if (c == '>') return '2';
        if (c == '<') return '3';
        return ' ';
    }

    static char RotatePosition(char c)
    {
        if (c == '0') return '2';
        else if (c == '1') return '3';
        else if (c == '2')return '1';
        else return '0';
    }

    static int[] MovePosition(int[] pos, char movingDirection)
    {
        if (movingDirection == '0')
        {
            //Move up a row
            pos[0]--;
        }
        else if (movingDirection == '1')
        {
            pos[0]++;
            //Move down a row
        }
        else if (movingDirection == '2')
        {
            pos[1]++;
            //Move right a column
        }
        else
        {
            pos[1]--;
            //Move left a column
        }
        return pos;
    }

    static int[] ReversePosition(int[] pos, char movingDirection)
    {
        if (movingDirection == '0')
        {
            //Move up a row
            pos[0]++;
        }
        else if (movingDirection == '1')
        {
            pos[0]--;
            //Move down a row
        }
        else if (movingDirection == '2')
        {
            pos[1]--;
            //Move right a column
        }
        else
        {
            pos[1]++;
            //Move left a column
        }
        return pos;
    }

    static int ObstructionCount(string[] grid)
    {
        int loopCount = 0;
        if (traversedLocations.Count > 0) {
            //Add the first location as our starting location and remove it since we can't add a barrier there
            int[] startPos = traversedLocations.FirstOrDefault();
            foreach (var point in traversedLocations)
            {
                var gridCopy = (string[])grid.Clone();
                var startPosCopy = (int[])startPos.Clone();
               if(DetermineLoop(startPosCopy, point, gridCopy)) loopCount++;
            }
        }
        return loopCount;
    }

    static bool DetermineLoop(int[] pos, int[] obstaclePos, string[] grid)
    {
        //Check we aren't putting an obsticle at the starting position
        if (pos[0] == obstaclePos[0] && pos[1] == obstaclePos[1]) return false;
        //Add our new obstacle
        char[] rowChars = grid[obstaclePos[0]].ToCharArray();
        rowChars[obstaclePos[1]] = '#';
        grid[obstaclePos[0]] = new string(rowChars);
        char movingDirection = ConvertChar(grid[pos[0]][pos[1]]);
        int gridState;

        do
        {
            grid = SetCurrentPosChar2(pos, movingDirection, grid);

            pos = MovePosition(pos, movingDirection);

            while (IsLegalPosition(pos, movingDirection, grid) == 1)
            {
                ReversePosition(pos, movingDirection);
                movingDirection = RotatePosition(movingDirection);
                pos = MovePosition(pos, movingDirection);
            }

            gridState = IsLegalPosition(pos, movingDirection, grid);

        } while (gridState == 1 || gridState == 0);

        //-1 Means we fell off the map
        //2 Means we found a loop
        if (gridState == 2) return true;
        else return false;
    }

    static string[] SetCurrentPosChar2(int[] pos, char movingDirection, string[] grid)
    {
        char CurrentChar = grid[pos[0]][pos[1]];
        //If the Current space hasn't been naticated yet, just set 
        if (CurrentChar == '.' || CurrentChar == '^' || CurrentChar == 'V' || CurrentChar == '>' || CurrentChar == '<')
        {
            // Convert the string to a char array to modify a specific character
            char[] rowChars = grid[pos[0]].ToCharArray();
            rowChars[pos[1]] = movingDirection; // Update the character at the specific position
            grid[pos[0]] = new string(rowChars); // Replace the string in the input array
        }
        else
        {
            char newChar = ' ';
            switch (CurrentChar)
            {
                case '0':
                    if (movingDirection == '1') newChar = '4';
                    else if (movingDirection == '2') newChar = '5';
                    else if (movingDirection == '3') newChar = '6';
                    break;
                case '1':
                    if (movingDirection == '0') newChar = '4';
                    else if (movingDirection == '2') newChar = '7';
                    else if (movingDirection == '3') newChar = '8';
                    break;
                case '2':
                    if (movingDirection == '1') newChar = '7';
                    else if (movingDirection == '3') newChar = '9';
                    else if (movingDirection == '0') newChar = '5';
                    break;
                case '3':
                    if (movingDirection == '0') newChar = '6';
                    else if (movingDirection == '1') newChar = '8';
                    else if (movingDirection == '2') newChar = '9';
                    break;
                case '4':
                    if (movingDirection == '2') newChar = 'A';
                    else if (movingDirection == '3') newChar = 'B';
                    break;
                case '5':
                    if (movingDirection == '1') newChar = 'A';
                    else if (movingDirection == '3') newChar = 'C';
                    break;
                case '6':
                    if (movingDirection == '1') newChar = 'B';
                    else if (movingDirection == '2') newChar = 'C';
                    break;
                case '7':
                    if (movingDirection == '0') newChar = 'A';
                    else if (movingDirection == '3') newChar = 'D';
                    break;
                case '8':
                    if (movingDirection == '0') newChar = 'B';
                    else if (movingDirection == '2') newChar = 'D';
                    break;
                case '9':
                    if (movingDirection == '0') newChar = 'C';
                    else if (movingDirection == '1') newChar = 'D';
                    break;
                case 'A':
                    newChar = 'E';
                    break;
                case 'B':
                    newChar = 'E';
                    break;
                case 'C':
                    newChar = 'E';
                    break;
                case 'D':
                    newChar = 'E';
                    break;
                default:
                    Console.WriteLine("We are were we shoudn't be");
                    break;
            }
            char[] rowChars = grid[pos[0]].ToCharArray();
            rowChars[pos[1]] = newChar;
            grid[pos[0]] = new string(rowChars);
        }
        return grid;
    }
}

//Quick lookup dictionary
/* N = 0
 * S = 1
 * E = 2
 * W = 3
 * NS = 4
 * NE = 5
 * NW = 6
 * SE = 7
 * SW = 8
 * EW = 9
 * NSE = A
 * NSW = B
 * NEW = C
 * SEW = D
 * NSEW = E
 */
