using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        string input = File.ReadAllText("Input.txt");

        Console.WriteLine("The answer to day 3 is: " + CalculateCorruptMemory(input));
        Console.WriteLine("The answer to day 3, part 2, is: " + CalculateSelectiveMemory(input));

    }

    static decimal CalculateCorruptMemory(string input)
    {
        decimal result = 0;
        string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";
        Regex regex = new Regex(pattern);

        foreach (Match match in regex.Matches(input))
        {
            decimal num1 = decimal.Parse(match.Groups[1].Value);
            decimal num2 = decimal.Parse(match.Groups[2].Value);
            result += (num1 * num2);
        }
        return result;
    }

    static decimal CalculateSelectiveMemory(string input)
    {
        decimal result = 0;
        string sectionPattern = @"(do\(\)|don't\(\))";
        string mulPattern = @"mul\((\d{1,3}),(\d{1,3})\)";

        // Split input into sections based on `do()` and `don't()`
        Regex sectionRegex = new Regex(sectionPattern);
        string[] sections = sectionRegex.Split(input);

        bool executeMul = true;

        foreach (string section in sections)
        {
            if (section == "do()")
            {
                executeMul = true;
                continue;
            }
            if (section == "don't()")
            {
                executeMul = false;
                continue;
            }

            if (executeMul)
            {
                // Process `mul(x, y)` in the current section
                Regex mulRegex = new Regex(mulPattern);
                foreach (Match match in mulRegex.Matches(section))
                {
                    decimal num1 = decimal.Parse(match.Groups[1].Value);
                    decimal num2 = decimal.Parse(match.Groups[2].Value);
                    result += (num1 * num2);
                }
            }
        }

        return result;
    }
}
