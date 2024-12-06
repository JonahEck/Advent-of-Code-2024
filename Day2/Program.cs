class Program
{
    static void Main(string[] args)
    {
        string input = File.ReadAllText("Input.txt");

        List<string> reports = new List<string>();

        using (StringReader reader = new StringReader(input))
        {
            string line = string.Empty;
            do
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    reports.Add(line);
                }
            } while (line != null);
        }
        Console.WriteLine("The answer to day 2 is: " + CalculateSafeReports(reports));
        Console.WriteLine("The answer to day 2 part 2 is: " + CalculateDampenedSafeReports(reports));
    }

    static int CalculateSafeReports(List<string> reports)
    {
        int safeCount = 0;

        foreach (var report in reports)
        {
            string[] levels = report.Split(' ');
            List<int> numbers = new List<int>();

            foreach (var level in levels)
            {
                if (int.TryParse(level, out int num))
                {
                    numbers.Add(num);
                }
            }

            if (IsSafe(numbers))
            {
                safeCount++;
            }
        }

        return safeCount;
    }

    static bool IsSafe(List<int> levels)
    {
        bool isIncreasing = true;
        bool isDecreasing = true;

        for (int i = 1; i < levels.Count; i++)
        {
            int difference = Math.Abs(levels[i] - levels[i - 1]);

            if (difference < 1 || difference > 3)
            {
                return false;
            }

            if (levels[i] > levels[i - 1])
            {
                isDecreasing = false; // Not decreasing
            }
            else if (levels[i] < levels[i - 1])
            {
                isIncreasing = false; // Not increasing
            }
        }
        return isIncreasing || isDecreasing;
    }

    static int CalculateDampenedSafeReports(List<string> reports)
    {
        int safeCount = 0;

        foreach (var report in reports)
        {
            string[] levels = report.Split(' ');
            List<int> numbers = new List<int>();

            foreach (var level in levels)
            {
                if (int.TryParse(level, out int num))
                {
                    numbers.Add(num);
                }
            }

            if (IsDampenedSafe(numbers))
            {
                safeCount++;
            }
        }
        return safeCount;
    }

    static bool IsDampenedSafe(List<int> levels)
    {
        // Check if the original sequence is safe
        if (IsSafe(levels))
        {
            return true;
        }

        // Try removing each number once and test the sequence
        for (int i = 0; i < levels.Count; i++)
        {
            // Create a new list excluding the current number
            List<int> modifiedLevels = new List<int>(levels);
            modifiedLevels.RemoveAt(i);

            // Check if the modified sequence is safe
            if (IsSafe(modifiedLevels))
            {
                return true;
            }
        }

        // If no single removal results in a safe sequence, return false
        return false;
    }
}

