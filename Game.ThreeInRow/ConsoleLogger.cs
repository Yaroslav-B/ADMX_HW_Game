using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ThreeInRow
{
    public class ConsoleLogger : Ilogger
    {
        public void Log(string text) => Console.WriteLine(text);
    }
}
