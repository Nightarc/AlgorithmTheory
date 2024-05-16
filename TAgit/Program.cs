#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TAgit;

//  Var a : Arr_Com;
//  alf: Set Of Char;   { входной алфавит }
//  inp_str: String;    { входная строка }
//  Count: Integer;     { количество команд в алгоритме}
//  F: Boolean;         { признак корректности ввода }
internal class Program
{
    static List<char> alf = new List<char>();
    static List<MarkovCommand> commands = new List<MarkovCommand>();
    static string input;
    const bool DEBUG = true;

    private static void InitAlphabet(string alfStr)
    {
        alf = alfStr.ToList();
    }

    static void Init(string fileName)
    {
        using (StreamReader sr = new StreamReader(fileName))
        {
            InitAlphabet(sr.ReadLine());
            string inputStr = sr.ReadLine();
            var regex = new Regex("\\d+.");
            while (!sr.EndOfStream && regex.IsMatch(inputStr)) //what if alphabet contains /d and . as symbols?
            {
                inputStr = regex.Replace(inputStr, "", 1);
                string[] commandParts = inputStr.Split('>', '!');
                commands.Add(new MarkovCommand(commandParts[0], commandParts[1], inputStr.Contains('!')));

                inputStr = sr.ReadLine();
                if (inputStr.StartsWith("//") || inputStr == "")  inputStr = sr.ReadLine(); 
            }
            input = inputStr;
        }
    }

    static string Solve() {
        string result = input;
        MarkovCommand executingCommand = commands[0];
        int i = 0;
        bool stop = false;
        bool isCorrect = false;
        while(!stop)
        {
            string temp;
            bool executed = executingCommand.Execute(result, out temp);
            if (executed)
            {
                isCorrect = true;
                if (DEBUG) Console.WriteLine(String.Format("command {0} {1} executed {2} -> {3}", i + 1, executingCommand.ToString(), result, temp));
                i = -1; result = temp;
                stop = executingCommand.stop;
            }
            else
            {
                if (DEBUG) Console.WriteLine(String.Format("command {0} {1} failed", i + 1, executingCommand.ToString(), temp));
            }

            if ((i+1) % commands.Count == 0) {
                if(!isCorrect) {
                    Console.WriteLine("Algorithm ended because no operations were exceuted.");
                    break;
                }
                isCorrect = false;
            }
            executingCommand = commands[(++i) % commands.Count];
        }

        return result;
    }
    private static void Main(string[] args)
    {
        Init("task2c.txt");
        Console.WriteLine(input);
        string result = Solve();
        Console.WriteLine(result);
    }
}

#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
//task3 - слова из одной буквы не входят в область применимости