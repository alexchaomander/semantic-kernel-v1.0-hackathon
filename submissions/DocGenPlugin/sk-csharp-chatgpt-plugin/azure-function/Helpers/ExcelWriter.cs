using GemBox.Spreadsheet;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class ExcelWriter
    {
        public string Key { set; get; }
        public ExcelWriter()
        {
            var setting = AppSettings.LoadSettings();
            Key = setting.GemboxKey;
        }
        public byte[] WriteToXlsx(List<ContentObject> PageContents)
        {
            try
            {
                // If using the Professional version, put your serial key below.
                // If using the Professional version, put your serial key below.
                SpreadsheetInfo.SetLicense(Key);

                ExcelFile workbook = new ExcelFile();
                
               
                foreach (var content in PageContents)
                {
                    ExcelWorksheet worksheet = workbook.Worksheets.Add($"Page-{content.Page}");
                    var contentbreak = content.Content.Split("\n");
                    var count = 1;
                    foreach (var cb in contentbreak)
                    {
                        ExcelCell cell = worksheet.Cells[$"A{count}"];
                        count++;
                        cell.Value = cb;
                    }
                }

                var ms = new MemoryStream();
                workbook.Save(ms, SaveOptions.XlsxDefault);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
