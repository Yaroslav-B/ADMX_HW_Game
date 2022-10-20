namespace Game.ThreeInRow
{
    class Cell
    {
        public int ColumnIndex { get; set; }
        public int RowIndex { get; set; }
        public int? Value { get; set; }

        public Cell(int columnIndex, int rowIndex)
        {
            ColumnIndex = columnIndex;
            RowIndex = rowIndex;
        }
    }
}
