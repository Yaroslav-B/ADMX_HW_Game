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
        private readonly int _minMatches;
        private readonly Ilogger _logger;
        private readonly List<List<Cell>> _gameFiled;
        private readonly List<Cell> _matchesList;

        /// <summary>
        /// Initializes a new instance using the specified parameters.
        /// </summary>
        /// <param name="rows">Number of rows in a game field</param>
        /// <param name="columns">Number of columns in a game field</param>
        /// <param name="minValue">Minimum value allowed to be set in a cell</param>
        /// <param name="maxValue">Maximum value allowed to be set in a cell</param>
        /// <param name="minMatches">Minimum number of matches of values in row or column to be taken into account</param>
        /// <param name="logger">Instance of Ilogger</param>
        public Game(int rows, int columns, int minValue, int maxValue, int minMatches, Ilogger logger)
        {
            _rows = rows;
            _columns = columns;
            _minValue = minValue;
            _maxValue = maxValue;
            _minMatches = minMatches;
            _logger = logger;

            _gameFiled = BuildEmptyGameField();
            _matchesList = new List<Cell>();
        }

        /// <summary>
        /// nitializes a new instance using default parameters.
        /// </summary>
        public Game() : this(9, 9, 0, 3, 3, new ConsoleLogger()) { }

        /// <summary>
        /// Starts a cyclic processing of the game field until there are no matches
        /// </summary>
        public void Start()
        {
            int cycle = 1;
            int matchNumbers = 0;

            while (true)
            {
                _logger.Log($"\n============== Cycle - {cycle} ==============");
                _logger.Log("Init field:");
                PopulateEmptyCells();
                PrintGameField();

                matchNumbers = FindMatches();
                if (matchNumbers == 0)
                {
                    break;
                }

                _logger.Log($"Found - {matchNumbers} matches:");
                ClearMatches();
                PrintGameField();

                _logger.Log("Shifted cells down:");
                DeleteEmptyCells();
                AddNewEmptyCells();
                PrintGameField();

                cycle++;
            }

            _logger.Log("There is no match\n" +
                        "Game over.");
        }

        private List<List<Cell>> BuildEmptyGameField()
        {
            var columns = new List<List<Cell>>(_columns);

            for (int columnindex = 0; columnindex < _columns; columnindex++)
            {
                var column = new List<Cell>(_rows);

                for (int rowIndex = 0; rowIndex < _rows; rowIndex++)
                {
                    var cell = new Cell(columnindex, rowIndex);
                    column.Add(cell);
                }

                columns.Add(column);
            }

            return columns;
        }

        private void PopulateEmptyCells()
        {
            //int c = 0;
            var r = new Random();

            for (int columnIndex = 0; columnIndex < _gameFiled.Count; columnIndex++)
            {
                var column = _gameFiled[columnIndex];

                for (int cellIndex = 0; cellIndex < column.Count; cellIndex++)
                {
                    //column[cellIndex].Value ??= c++;
                    column[cellIndex].Value ??= r.Next(_minValue, _maxValue + 1);
                }
            }
        }

        private void PrintGameField()
        {
            int maxFieldCapacity = _columns * _rows + 50;
            var sb = new StringBuilder(maxFieldCapacity);
            string space = " ";

            for (int rowIndex = _rows - 1; rowIndex >= 0; rowIndex--)
            {
                for (int columnindex = 0; columnindex < _columns; columnindex++)
                {
                    int? value = _gameFiled[columnindex][rowIndex].Value;
                    sb.Append(value).Append(space);
                    if (value is null)
                    {
                        sb.Append(space);
                    }
                }

                sb.AppendLine();
            }

            _logger.Log(sb.ToString());
        }

        private int FindMatches()
        {
            _matchesList.Clear();
            var bufer = new List<Cell>();
            int? tempValue = null;

            //vertical checking
            foreach (var column in _gameFiled)
            {
                foreach (var cell in column)
                {
                    CheckCell(cell);
                }

                DumpAndClear();
                tempValue = null;
            }


            //horizontal check
            for (int rowIndex = 0; rowIndex < _rows; rowIndex++)
            {
                foreach (var column in _gameFiled)
                {
                    CheckCell(column[rowIndex]);
                }
                DumpAndClear();
                tempValue = null;
            }


            void CheckCell(Cell cell)
            {
                if (cell.Value != tempValue)
                {
                    DumpAndClear();
                }

                bufer.Add(cell);
                tempValue = cell.Value;
            }

            void DumpAndClear()
            {
                //dump matches
                if (bufer.Count >= _minMatches)
                {
                    _matchesList.AddRange(bufer);
                }

                bufer.Clear();
            }

            return _matchesList.Count;
        }

        private void ClearMatches()
        {
            foreach (var cell in _matchesList)
            {
                cell.Value = null;
            }
        }

        private void DeleteEmptyCells()
        {
            foreach (var column in _gameFiled)
            {
                column.RemoveAll(x => _matchesList.Contains(x));
            }
        }

        private void AddNewEmptyCells()
        {
            for (int column = 0; column < _columns; column++)
            {
                var col = _gameFiled[column];
                for (int row = col.Count; row < _rows; row++)
                {
                    col.Add(new Cell(0, 0));
                }
            }
        }
    }
}
