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

/// <summary>

/// ''' Business logic layer for label formats. 

/// ''' </summary>

/// ''' <remarks>As this is a demo and there is no database to use, the BLL will do the DAL work aswell.</remarks>
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
using PdfSharp;

public class LabelFormatBLL
{
    private static List<LabelFormat> _mLabelFormats;

    /// <summary>
    ///     ''' Return a list of all label formats in the database.
    ///     ''' </summary>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    public static List<LabelFormat> GetLabelFormats()
    {
        if (_mLabelFormats == null)
        {
            // We are not using a database so manually create the labels here.
            // L7193
            // _mLabelFormats.Add(New LabelFormat(Id:=1,
            // Name:=,
            // Description:=,
            // PageWidth:=,
            // PageHeight:=,
            // TopMargin:=,
            // LeftMargin:=,
            // LabelWidth:=, 
            // LabelHeight:=,
            // VerticalPitch:=,
            // HorizontalPitch:=,
            // ColumnCount:=,
            // RowCount:=))
            _mLabelFormats = new List<LabelFormat>();
            _mLabelFormats.Add(new LabelFormat(Id: 1, Name: "L7163", Description: "A4 Sheet of 99.1 x 38.1mm address labels", PageWidth: 210, PageHeight: 297, TopMargin: 15.1, LeftMargin: 4.7, LabelWidth: 99.1, LabelHeight: 38.1, VerticalPitch: 38.1, HorizontalPitch: 101.6, ColumnCount: 2, RowCount: 7, LabelPaddingTop: 5.0, LabelPaddingLeft: 8.0));
            _mLabelFormats.Add(new LabelFormat(Id: 2, Name: "L7169", Description: "A4 Sheet of 99.1 x 139mm BlockOut (tm) address labels", PageWidth: 210, PageHeight: 297, TopMargin: 9.5, LeftMargin: 4.6, LabelWidth: 99.1, LabelHeight: 139, VerticalPitch: 139, HorizontalPitch: 101.6, ColumnCount: 2, RowCount: 2, LabelPaddingTop: 5.0, LabelPaddingLeft: 8.0));
        }
        return _mLabelFormats;
    }

    /// <summary>
    ///     ''' Return a single label format
    ///     ''' </summary>
    ///     ''' <param name="Id">integer reference for the label that is required.</param>
    ///     ''' <returns></returns>
    ///     ''' <remarks></remarks>
    public static LabelFormat GetLabelFormat(int Id)
    {
       // GetLabelFormat = null/* TODO Change to default(_) if this is not a reference type */;

        foreach (LabelFormat lf in GetLabelFormats())
        {
            if (lf.Id == Id)
                // Label format found. Return it.
                return lf;
        }
        return null;
    }
}
