

using System.Text.RegularExpressions;

class Program
{
    static List<int>[] pageOrders = new List<int>[99]; //Define array of 99 items
    static List<List<int>> pageUpdates = new List<List<int>>();
    static void Main(string[] args)
    {
        int pageCount = 0;
        int fixedPageCount = 0;
        InitilizeData();
        foreach (var row in pageUpdates)
        {
            pageCount += checkPageOrder(row);
            fixedPageCount += checkPageOrderAndFix(row);
        }
        Console.WriteLine("The answer to day 5 is: " + pageCount);
        Console.WriteLine("The answer to day 5 part 2 is: " + fixedPageCount);
    }

    static void InitilizeData()
    {
        string input = File.ReadAllText("Input.txt");

        //Initilize our array with empty Lists
        for (int i = 0; i < pageOrders.Length; i++)
        {
            pageOrders[i] = new List<int>();
        }

        //Begin parsing input data
        using (StringReader reader = new StringReader(input))
        {
            string pattern = @"(\d{2})\|(\d{2})";
            string line = string.Empty;
            do
            {
                line = reader.ReadLine();
                if (line != "")
                {
                    Match match = Regex.Match(line, pattern);
                    pageOrders[int.Parse(match.Groups[1].Value)-1].Add(int.Parse(match.Groups[2].Value));
                }
            } while (line != "");

            do
            {
                line = reader.ReadLine();
                if (line != null)
                {
                    pageUpdates.Add(line.Split(',').Select(int.Parse).ToList());
                }
            } while (line != null);
        }
    }

    static int checkPageOrder(List<int> pages)
    {
        //I think we can start at position 2, there are no numbers before position 1 so it is valid
        for (int i = 1; i < pages.Count; i++)
        {
            var currentPageOrders = pageOrders[pages[i] - 1];
            if (currentPageOrders.Count == 0) break; //If the count is 0 that means there was no order for that page and we can skip it
                
            //Check all pages to the left are not found in that page's list
            for (int j = 0; j < i; j++)
            {
                //Return 0 if we found a page that is out of order
                if (currentPageOrders.Contains(pages[j])) return 0;
            }
        }
        return pages[pages.Count / 2];
    }

    static int checkPageOrderAndFix(List<int> pages)
    {
        bool wasOutOfOrder = false;
        //I think we can start at position 2, there are no numbers before position 1 so it is valid
        for (int i = 1; i < pages.Count; i++)
        {
            var currentPageOrders = pageOrders[pages[i] - 1];
            if (currentPageOrders.Count == 0) break; //If the count is 0 that means there was no order for that page and we can skip it

            //Check all pages to the left are not found in that page's list
            for (int j = 0; j < i; j++)
            {
                //Return 0 if we found a page that is out of order
                if (currentPageOrders.Contains(pages[j]))
                {
                    wasOutOfOrder = true;
                    (pages[i], pages[j]) = (pages[j], pages[i]);
                    i = 0; //Double check our swap didn't break any other orders
                    break; //Break out of j loop
                }
            }
        }
        if (wasOutOfOrder)
            return pages[pages.Count / 2];
        else return 0;
    }
}
