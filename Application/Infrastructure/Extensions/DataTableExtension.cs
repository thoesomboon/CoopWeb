using System.Data;
using System.IO;
using System.Text;

namespace Coop.Infrastructure.Extensions
{
    public static class DataTableExtension
    {
        public static string ToCSV(this DataTable table)
        {
            var result = new StringBuilder();
            for (var i = 0; i < table.Columns.Count; i++)
            {
                result.Append(table.Columns[i].ColumnName);
                result.Append(i == table.Columns.Count - 1 ? "\r\n" : ",");
            }

            foreach (DataRow row in table.Rows)
            {
                for (var i = 0; i < table.Columns.Count; i++)
                {
                    result.Append(row[i]);
                    result.Append(i == table.Columns.Count - 1 ? "\r\n" : ",");
                }
            }

            return result.ToString();
        }

        public static void SaveAsCSV(this DataTable table, string path, int[] convertOADateTime = null)
        {
            File.WriteAllText(path, table.ToCSV(), Encoding.Unicode);
        }
    }
}