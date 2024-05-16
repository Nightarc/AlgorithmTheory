#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TAgit;

internal class Program
{
    static List<char> alf = new List<char>();
    static List<MarkovCommand> commands = new List<MarkovCommand>();
    static string input;

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
                while (inputStr.StartsWith("//") || inputStr == "") 
                    inputStr = sr.ReadLine(); 
            }
            input = inputStr;
        }
    }

    static string Solve(string str, bool DEBUG) {
        string result = str;
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

    static string Solve(string str) => Solve(str, false);
    private static void Main(string[] args)
    {
        const bool debug = false;
        Init("task2c.txt");
        while(true)
        {
            Console.WriteLine("Введите исходную строку или пустую, чтобы прочитать из файла");
            string result;
            string inStr = Console.ReadLine();
            if (inStr == "")
            {
                result = Solve(input, debug);
                Console.WriteLine(input);
            }
            else result = Solve(inStr, debug);
            Console.WriteLine("result is: " + result);
        }
    }
}

#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
//task3 - слова из одной буквы не входят в область применимости