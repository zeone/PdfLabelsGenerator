using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

namespace PdfLabels
{
    class Program
    {
        static Document _document;
        static readonly Color _fontColor = new Color(102, 102, 102);
        static Table _table;
        static Section _section;
        static void Main(string[] args)
        {
            _document = new Document { Info = { Title = "SomName" } };


            DefineStyles();
            CreateLabelPage();
            FillLabelRows();
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true) { Document = _document };
            pdfRenderer.RenderDocument();
            using (MemoryStream ms = new MemoryStream())
            {
                pdfRenderer.Save(ms, false);
                byte[] buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Flush();
                ms.Read(buffer, 0, (int)ms.Length);
                ms.Close();
                File.WriteAllBytes("test.pdf", buffer);
            }
            // Console.ReadKey();
        }



        static void DefineStyles()
        {
            // Get the predefined style Normal.
            Style style = _document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            style = _document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            //style = _document.Styles[StyleNames.Footer];
            //style.ParagraphFormat.AddTabStop("1cm", TabAlignment.Center);
            //style.Font.Name = "Verdana";
            //style.Font.Name = "Times New Roman";
            //style.Font.Size = 9;
            //style.Font.Color = _fontColor;

            // Create a new style called Table based on style Normal
            style = _document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;
            style.Font.Color = _fontColor;

            style = _document.Styles.AddStyle("TableLabel", "Normal");
            style.Font.Name = "Arial";
            style.Font.Name = "Arial";
            style.Font.Size = 9;
            style.Font.Color = Color.Empty;

            // Create a new style called Reference based on style Normal
            style = _document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        private static void CreateLabelPage()
        {
            // Each MigraDoc document needs at least one section.
            _section = _document.AddSection();
            //_section.PageSetup = _document.DefaultPageSetup.Clone();
            var pageSize = _document.DefaultPageSetup.Clone();
            pageSize.PageFormat = PageFormat.Letter;
            pageSize.Orientation = Orientation.Portrait;
            pageSize.LeftMargin = new Unit { Millimeter = 4.82 };
            pageSize.RightMargin = new Unit { Millimeter = 4.82 };
            pageSize.TopMargin = new Unit { Millimeter = 12.7 };
            pageSize.BottomMargin = new Unit { Inch = 0 };
            pageSize.PageWidth = new Unit { Millimeter = 215.9 };
            pageSize.PageHeight = new Unit { Millimeter = 279.4 };
            _section.PageSetup = pageSize;
            //  _section.PageSetup.PageWidth = _section.PageSetup.PageWidth - _section.PageSetup.LeftMargin - _section.PageSetup.RightMargin; ;
            //_section.PageSetup.PageHeight = new Unit { Millimeter = 279 };

            // Create the item table
            _table = _section.AddTable();
            _table.Style = "TableLabel";

            _table.Borders.Color = Color.Empty;
            _table.Borders.Width = 0;
            //_table.Borders.Left.Width = 0.5;
            //_table.Borders.Right.Width = 0.5;
            _table.Rows.LeftIndent = 0;
            _table.Rows.HeightRule = RowHeightRule.Exactly;
            _table.Rows.Height = new Unit { Millimeter = 25.4 };

            CreateLabelColumns();
            // _table.SetEdge(0, 0, 3, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);
        }

        private static void CreateLabelColumns()
        {
            Unit mainSize = new Unit { Millimeter = 66.80 };
            Unit innSize = new Unit { Millimeter = 3.04 };
            for (int i = 0; i < 5; i++)
            {
                Column column = _table.AddColumn();
                //column.LeftPadding = 0;
                //column.RightPadding = 0;

                column.Format.Alignment = ParagraphAlignment.Center;
                column.Width = (i % 2 != 0) ? innSize : mainSize;
            }
        }

        private static void FillLabelRows()
        {
            int countCols = 0;
            Row row = _table.AddRow();
            //foreach (ContactReportResultDto contact in contacts.Where(a => a.Addresses != null && a.Addresses.Any()))
            //{
            //if (filter.ReportType == TransFilterType.Family)
            //{
            foreach (var addr in Addresses.GetAddresses())
            {
                countCols++;
                if ((countCols % 2 == 0) && countCols != 0) countCols++;
                if (countCols > 5)
                {
                    countCols = 1;
                    row = _table.AddRow();
                }
                FillRow(row, countCols - 1, addr,
                    false, ParagraphAlignment.Left,
                    VerticalAlignment.Top, marginLeft: new Unit { Millimeter = 2 });
            }
            //}
            //if (filter.ReportType == TransFilterType.Member)
            //{
            //    foreach (ContactReportAddress addr in contact.Addresses)
            //    {
            //        foreach (ContactReportMambers member in contact.Members)
            //        {
            //            countCols++;
            //            if ((countCols % 2 == 0) && countCols != 0) countCols++;
            //            if (countCols > 5)
            //            {
            //                countCols = 1;
            //                row = _table.AddRow();
            //            }
            //            var memberName = (filter.SkipTitles ? "" : member.Title) + member.FirstName + " " + member.LastName;
            //            FillRow(row, countCols - 1, GetShortName(
            //                    string.Format("{0}\n{1}", memberName, GetFullAddressLine(addr, filter, true)), 132),
            //                false, ParagraphAlignment.Left,
            //                VerticalAlignment.Top, marginLeft: new Unit { Millimeter = 2 });
            //        }
            //    }
            //}
            // }
        }

        static void FillRow(Row row, int cellIndex, string colText, bool colBold, ParagraphAlignment colParAligment, VerticalAlignment colVerAlignment,
            Color? fontColor = null, float? marginLeft = null, int? fontSize = null)
        {

            if (fontSize != null) row.Cells[cellIndex].Format.Font.Size = (int)fontSize;
            if (fontColor != null) row.Cells[cellIndex].Format.Font.Color = (Color)fontColor;
            if (marginLeft != null) row.Cells[cellIndex].Format.LeftIndent = (float)marginLeft;
            row.Cells[cellIndex].AddParagraph(string.IsNullOrEmpty(colText) ? "" : colText);
            row.Cells[cellIndex].Format.Font.Bold = colBold;
            row.Cells[cellIndex].Format.Alignment = colParAligment;
            row.Cells[cellIndex].VerticalAlignment = colVerAlignment;
        }
    }
}
