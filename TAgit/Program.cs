using System.Collections.Specialized;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Input N: ");
        int n = int.Parse(Console.ReadLine());
        CountPairs(++n);
    }

    private static void CountPairs(int n)
    {
        Tuple<int, int>[,] M = new Tuple<int, int>[n + 1, n + 1];
        //Цикл заполнения до средней диагонали (или средней - 1 если n нечетно)
        for (int k = 0; k <= n; k++)
            for(int i = k, j = 0; j <= k; i--, j++)
                M[i, j] = Tuple.Create(i, j);
        //Цикл оставшихся диагоналей
        for (int k = n; k >= 0; k--)
            for (int i = n, j = n - k + 1; j <= n; i--, j++)
                M[i, j] = Tuple.Create(i, j);

        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
                Console.Write($"({M[i, j].Item1}, {M[i, j].Item2}) ");
            Console.WriteLine();
        }
    }
}