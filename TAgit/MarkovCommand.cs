using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TAgit
{
    /// <summary>
    /// Класс, олицетворяющий одну команду в эмуляторе НАМ
    /// </summary>
    internal class MarkovCommand
    {
        string oldStr; // искомый фрагмент команды
        public bool stop { get; private set; }    // является ли команда остановкой
        string newStr; // подстановка выполняемая командой
        Regex regex;
        public MarkovCommand(string oldStr, string newStr, bool stop)
        {
            this.oldStr = oldStr;
            this.newStr = newStr;
            this.stop = stop;

            regex = new Regex(Regex.Escape(oldStr).Replace('.', '^'));
        }

        string doubleSlashes(string str)
        {
            if (str.Contains("/")) return str.Replace("/", "//");
            else return str;
        }
        public bool Execute(string str, out string outStr) {
            if(!regex.IsMatch(str)) { outStr = ""; return false; }
            outStr = regex.Replace(str, newStr, 1);
            return true;
        }
        public bool CanExecute(string str) => regex.IsMatch(str);
        public override string ToString()
        {
            return String.Format("{0} -> {1}", oldStr, newStr);
        }
    }
}
