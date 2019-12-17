// Copyright (c) 2014 Craig Moore - Deadline Automation Limited

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

/// <summary>

/// ''' Utility class for creating labels within a PDF document

/// ''' </summary>

/// ''' <remarks></remarks>
public class PdfLabelUtil
{
    public static MemoryStream GeneratePdfLabels(List<string> Addresses, LabelFormat lf, int QtyEachLabel = 1)
    {
        var ms = new MemoryStream();

        // The label sheet is basically a table and each cell is a single label

        // Format related
        int CellsPerPage = lf.RowCount * lf.ColumnCount;
        int CellsThisPage = 0;
        XRect ContentRectangle;       // A single cell content rectangle. This is the rectangle that can be used for contents and accounts for margins and padding.
        XSize ContentSize;            // Size of content area inside a cell.
        double ContentLeftPos;        // left edge of current content area.
        double ContentTopPos;         // Top edge of current content area

        // Layout related
        XColor StrokeColor = XColors.DarkBlue;
        XColor FillColor = XColors.DarkBlue;
        XPen Pen = new XPen(StrokeColor, 0.1);
        XBrush Brush = new XSolidBrush(FillColor);
        XGraphics Gfx;
        XGraphicsPath Path;

        int LoopTemp = 0;         // Counts each itteration. Used with QtyEachLabel
        int CurrentColumn = 1;
        int CurrentRow = 1;
        PdfDocument Doc = new PdfDocument();
        PdfPage page = null/* TODO Change to default(_) if this is not a reference type */;
        AddPage(ref Doc, ref page, lf);
        Gfx = XGraphics.FromPdfPage(page);

        // Ensure that at least 1 of each label is printed.
        if (QtyEachLabel < 1)
            QtyEachLabel = 1;

        // Define the content area size
        ContentSize = new XSize(XUnit.FromMillimeter(lf.LabelWidth - lf.LabelPaddingLeft - lf.LabelPaddingRight).Point, XUnit.FromMillimeter(lf.LabelHeight - lf.LabelPaddingTop - lf.LabelPaddingBottom).Point);

        if (Addresses!=null)
        {
            if (Addresses.Count > 0)
            {
                // We actually have addresses to output.
                foreach (string Address in Addresses)
                {
                    // Once for each address
                    for (LoopTemp = 1; LoopTemp <= QtyEachLabel; LoopTemp++)
                    {
                        // Once for each copy of this address.
                        if (CellsThisPage == CellsPerPage)
                        {
                            // This pages worth of cells are filled up. Create a new page
                            AddPage(ref Doc, ref page, lf);
                            Gfx = XGraphics.FromPdfPage(page);
                            CellsThisPage = 0;
                        }

                        // Calculate which row and column we are working on.
                        CurrentColumn = (CellsThisPage + 1) % lf.ColumnCount;
                        CurrentRow = (int)((CellsThisPage + 1) / (double)lf.ColumnCount);

                        if (CurrentColumn == 0)
                            // This occurs when you are working on the last column of the row. 
                            // This affects the count for column and row
                            CurrentColumn = lf.ColumnCount;
                        else
                            // We are not viewing the last column so this number will be decremented by one.
                            CurrentRow = CurrentRow + 1;

                        // Calculate the left position of the current cell.
                        ContentLeftPos = ((CurrentColumn - 1) * lf.HorizontalPitch) + lf.LeftMargin + lf.LabelPaddingLeft;

                        // Calculate the top position of the current cell.
                        ContentTopPos = ((CurrentRow - 1) * lf.VerticalPitch) + lf.TopMargin + lf.LabelPaddingTop;

                        // Define the content rectangle.
                        ContentRectangle = new XRect(new XPoint(XUnit.FromMillimeter(ContentLeftPos).Point, XUnit.FromMillimeter(ContentTopPos).Point), ContentSize);

                        Path = new XGraphicsPath();

                        // Add the address string to the page.
                        Path.AddString(Address, new XFontFamily("Arial"), XFontStyle.Regular, 11, ContentRectangle, XStringFormats.TopLeft);

                        Gfx.DrawPath(Pen, Brush, Path);

                        // Increment the cell count
                        CellsThisPage = CellsThisPage + 1;
                    }
                }
                // Output the document
                Doc.Save(ms, false);
            }
        }
        return ms;
    }

    private static void AddPage(ref PdfDocument Doc, ref PdfPage Page, LabelFormat lf)
    {
        Page = Doc.AddPage();
        Page.Width = XUnit.FromMillimeter(lf.PageWidth);
        Page.Height = XUnit.FromMillimeter(lf.PageHeight);
    }
}
