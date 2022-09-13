using System;
using System.Collections.Generic;
using System.Text;

namespace HC.Model
{
    public class ChangesLog
    {
        public string State { get; set; }
        public string TableName { get; set; }
        public int RecordID { get; set; }
        public string ColumnName { get; set; }
        public int? ColumnId { get; set; }
        public string NewValue { get; set; }
        public string OriginalValue { get; set; }
        public int IndexNumber { get; set; }
    }
}
