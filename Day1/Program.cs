using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        string input = File.ReadAllText("Input.txt");

        List<string> leftList = new List<string>();
        List<string> rightList = new List<string>();

        using (StringReader reader = new StringReader(input))
        {
            string line = string.Empty;
            do
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    string[] split = line.Split(new[] { "   " }, StringSplitOptions.None);
                    if (split.Length > 0)
                    {
                        leftList.Add(split[0]);
                        rightList.Add(split[1]);
                    }
                }
            } while (line != null);
        }
        Console.WriteLine("The answer to day 1 is: " + CalculateDistance(leftList, rightList));
        Console.WriteLine("The answer to day 1 part 2 is: " + CalculateSimilarity(leftList, rightList));
    }

    static double CalculateDistance(List<string> leftList, List<string> rightList)
    {
        double distance = 0;

        leftList.Sort();
        rightList.Sort();

        for (int i = 0; i < leftList.Count; i++)
        {
            distance += Math.Abs(int.Parse(leftList[i]) - int.Parse(rightList[i]));
        }

        return distance;
    }

    static double CalculateSimilarity(List<string> leftList, List<string> rightList)
    {
        double similarity = 0;
        foreach (string s in leftList) {
            similarity += int.Parse(s) * rightList.FindAll(x => x == s).Count();
        }

        return similarity;
    }
}
