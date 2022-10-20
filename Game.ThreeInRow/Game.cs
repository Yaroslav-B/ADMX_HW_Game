using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ThreeInRow
{
    public class Game
    {
        private readonly int _rows;
        private readonly int _columns;
        private readonly int _minValue;
        private readonly int _maxValue;
        private readonly int _matches;
        private readonly Ilogger _logger;

        public Game(int rows, int columns, int minValue, int maxValue, int matches, Ilogger logger)
        {
            _rows = rows;
            _columns = columns;
            _minValue = minValue;
            _maxValue = maxValue;
            _matches = matches;
            _logger = logger;
        }

        public Game() : this(9, 9, 0, 3, 3, new ConsoleLogger()) { }

        public void Start()
        {

        }
    }
}
