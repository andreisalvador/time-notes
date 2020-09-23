using ClosedXML.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TimeNotes.Core.Extensions;

namespace TimeNotes.Infrastructure.Components.Exports.Excel
{
    public class ExcelExport<T>
    {
        private const byte ROW_COLUMNS_NAME_INDEX = 1;
        private const byte ROW_COLUMNS_VALUE_START_INDEX = 2;

        public byte[] ExportExcel(IEnumerable<T> records, string sheetName = "Sheet 1")
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (var workbook = new XLWorkbook())
                {
                    IXLWorksheet workSheet = workbook.AddWorksheet(sheetName);
                    PopulateColumnNames(workSheet, records);
                    PopulateColumnValues(workSheet, records);
                    workbook.SaveAs(stream);
                }

                return stream.ToArray();
            }
        }

        private void PopulateColumnValues(IXLWorksheet workSheet, IEnumerable<T> records)
        {
            string[] excludeFields = new string[] { "Id" };
            var columnData = records.Select(record => record.ToObjectArray(excludeFields).Where(w => w != null).ToArray()).ToArray();

            for (int index = 0; index < columnData.Count(); index++)
            {
                PopulateCells<object>(workSheet, columnData[index], ROW_COLUMNS_VALUE_START_INDEX + index);
            }
        }

        private void PopulateColumnNames(IXLWorksheet workSheet, IEnumerable<T> records)
        {
            string[] columns = GetColumns(records);

            PopulateCells(workSheet, columns, ROW_COLUMNS_NAME_INDEX);
        }

        private void PopulateCells<TRecordType>(IXLWorksheet worksheet, TRecordType[] records, int rowIndex)
        {
            for (int index = 1; index <= records.Length; index++)
                worksheet.Cell(rowIndex, index).Value = records[index - 1].ToString();
        }

        private bool HasRecords(IEnumerable<T> records)
            => records != null && records.Count() > 0;


        private string[] GetColumns(IEnumerable<T> records)
            => records.FirstOrDefault()
                       .GetType()
                       .GetProperties()
                       .Where(w => !w.Name.StartsWith("Id", System.StringComparison.OrdinalIgnoreCase) && !w.Name.EndsWith("Id", System.StringComparison.OrdinalIgnoreCase))
                       .Select(property => property.Name).ToArray();
    }
}
