using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using word = Microsoft.Office.Interop.Word;
using excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.Office.Interop.Excel;

namespace IDZ_CLIENT.ViewModels
{
    public class ReportCreator
    {
        public List<Models.ArmorDefence> ArmorList = null;
        public object path;

        private readonly int count = 10;
        private excel.Application excelApp;
        private word.Application wordApp;
        private word.Document wDoc1;
        private word.Document wDoc2;
        private word.Document wDoc3;
        private Dictionary<string, List<Models.ArmorDefence>> topArmors = new Dictionary<string, List<Models.ArmorDefence>>();
        public void CreateReport()
        {
            excelApp = new excel.Application();
            wordApp = new word.Application();

            if (ArmorList != null)
            {
                getTopArmor();
                CreateWordReport();
                CreateExcelReport();

                excel.Workbook excelbook = excelApp.Workbooks.Open(Environment.CurrentDirectory + @"\ExcelReports\out.xlsx");
                excel.ChartObjects chartObjects = excelbook.Sheets["Лист1"].ChartObjects();

                object file = Environment.CurrentDirectory + @"\WordTemplates\report.docx";
                wDoc3 = wordApp.Documents.Add(ref file, false, word.WdNewDocumentType.wdNewBlankDocument, true);

                foreach (ChartObject chartObject in chartObjects)
                {
                    word.Range range = wDoc3.Content.Paragraphs.Last.Range;
                    chartObject.Copy();
                    range.Paste();
                    wDoc3.Content.Paragraphs.Add();
                }
                if (path.ToString() == "") path = Environment.CurrentDirectory + @"\WordReports\Report";
                wDoc3.SaveAs(path);
                wDoc3.Close();
                excelbook.Close();
            }
            excelApp.Quit();
            wordApp.Quit();
        }
        private void getTopArmor()
        {
            topArmors = new Dictionary<string, List<Models.ArmorDefence>>();
            foreach (var type in ArmorList.GroupBy(p => p.ArmorType))
            {
                List<Models.ArmorDefence> armors = ArmorList.Where(p => p.ArmorType == type.Key).
                    OrderBy(p => p.Defence).Take(count).ToList();
                topArmors.Add(type.Key, armors);
            }
        }
        private void CreateWordReport()
        {
            int entryCount = ArmorList.Count();
            
            object file1 = Environment.CurrentDirectory + @"\WordTemplates\template.docx";
            object file2 = Environment.CurrentDirectory + @"\WordTemplates\template1.docx";

            wDoc1 = wordApp.Documents.Add(ref file1, false, word.WdNewDocumentType.wdNewBlankDocument, true);
            wDoc2 = wordApp.Documents.Add(ref file2, false, word.WdNewDocumentType.wdNewBlankDocument, true);

            replace("{N}", entryCount.ToString());

            foreach(var item in topArmors)
            {
                List<Models.ArmorDefence> armors = item.Value;
                word.Range range = wDoc1.Content.Paragraphs.Last.Range;
                wDoc2.Content.Copy();
                range.Paste();
                replace("{A}", item.Key);
                string replaceString = "";
                foreach (Models.ArmorDefence _armor in armors)
                {
                    replaceString += _armor.ArmorName + "^p";
                }
                replaceString = replaceString.Remove(replaceString.Length - 2, 2);
                replace("{B}", replaceString);
            }
            wDoc1.SaveAs(Environment.CurrentDirectory + @"\WordTemplates\report");
            wDoc1.Close();
            wDoc2.Close();
        }
        private void CreateExcelReport()
        {
            excel.Workbook book = excelApp.Workbooks.Add();
            excel.Worksheet ws = book.ActiveSheet;
            int itemCount = 1;
            foreach(var item in topArmors)
            {
                for (int i = 0; i < count; i++)
                {
                    ws.Cells[itemCount, i + 1].Value = topArmors[item.Key][i].Defence;
                    ws.Cells[itemCount + 1, i + 1].Value = topArmors[item.Key][i].ArmorName;
                }
                excel.Chart chart = ((excel.ChartObjects)ws.ChartObjects(Type.Missing)).Add(50, itemCount * 200, 450, 250).Chart;
                chart.ChartType = excel.XlChartType.xlColumnClustered;
                chart.SetSourceData(ws.Range[ws.Cells[itemCount, 1], ws.Cells[itemCount, count]]);
                excel.Series series = chart.SeriesCollection(1);
                series.XValues = topArmors[item.Key].Select(p => p.ArmorName).ToArray();
                chart.HasTitle = true;
                chart.ChartTitle.Text = $"Топ {count} {item.Key}";
                itemCount += 2;
            }

            ws.SaveAs(Environment.CurrentDirectory + @"\ExcelReports\out");
            book.Close();
            excelApp.Quit();
        }

        private void replace(string find, string replace)
        {
            word.Range range = wDoc1.StoryRanges[word.WdStoryType.wdMainTextStory];
            range.Find.ClearFormatting();
            range.Find.Execute(FindText: find, ReplaceWith: replace);
        }
    }
}
