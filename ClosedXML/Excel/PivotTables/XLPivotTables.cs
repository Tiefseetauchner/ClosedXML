using System;
using System.Collections.Generic;
using System.Linq;

namespace ClosedXML.Excel
{
    internal class XLPivotTables : IXLPivotTables
    {
        private readonly Dictionary<String, XLPivotTable> _pivotTables = new Dictionary<string, XLPivotTable>(StringComparer.OrdinalIgnoreCase);

        public XLPivotTables(IXLWorksheet worksheet)
        {
            this.Worksheet = worksheet ?? throw new ArgumentNullException(nameof(worksheet));
        }

        public void Add(String name, IXLPivotTable pivotTable)
        {
            _pivotTables.Add(name, (XLPivotTable)pivotTable);
        }

        public IXLPivotTable AddNew(string name, IXLCell target, IXLRange source)
        {
            var pivotTable = new XLPivotTable(this.Worksheet)
            {
                Name = name,
                TargetCell = target,
                SourceRange = source
            };
            _pivotTables.Add(name, pivotTable);
            return pivotTable;
        }

        public IXLPivotTable AddNew(string name, IXLCell target, IXLTable table)
        {
            var dataRange = table.DataRange;
            var header = table.HeadersRow();
            var range = table.Worksheet.Range(header.FirstCell(), dataRange.LastCell());

            return AddNew(name, target, range);
        }

        public Boolean Contains(String name)
        {
            return _pivotTables.ContainsKey(name);
        }

        public void Delete(String name)
        {
            _pivotTables.Remove(name);
        }

        public void DeleteAll()
        {
            _pivotTables.Clear();
        }

        public IEnumerator<IXLPivotTable> GetEnumerator()
        {
            return _pivotTables.Values.Cast<IXLPivotTable>().GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public XLPivotTable PivotTable(String name)
        {
            return _pivotTables[name];
        }

        IXLPivotTable IXLPivotTables.PivotTable(String name)
        {
            return PivotTable(name);
        }

        public IXLWorksheet Worksheet { get; private set; }
    }
}
