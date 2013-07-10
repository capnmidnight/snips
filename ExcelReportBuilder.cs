using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace XLGroup.ClaritySM
{
    public class ExcelReportBuilder : IDisposable
    {
        private Application app;
        private Workbook wb;
        private Worksheet ws;
        public string FileName { get; private set; }
        private ReportProcessor reportProc;
        public ExcelReportBuilder(ReportProcessor rep, string filePath)
        {
            this.reportProc = rep;
            // generate a mostly unique filename. We just need to avoid collisions for the
            // next few seconds before this file gets into the database.
            this.FileName = filePath + "temp_" + DateTime.Now.Ticks.ToString();
            this.app = new Application();
            this.wb = app.Workbooks.Add(Missing.Value); // no template
            this.ws = (Worksheet)wb.ActiveSheet;
        }

        public void DataBind()
        {
            this.StylizeHeader();
            BuildHeader();
            BuildBody();

            // Save the file to a temporary location so that we can then send it to the user.
            wb.SaveAs(
                this.FileName, // no file extension on the filename necessary
                XlFileFormat.xlExcel9795, //Excel 1995 / 1997, which is the lowest common denominator for most people
                Missing.Value, // no read password
                Missing.Value, // no write password
                false, // no recommending read-only status
                false, // no creating backups
                XlSaveAsAccessMode.xlExclusive, // do not let any other processes access the file while it is being saved.
                XlSaveConflictResolution.xlUserResolution, // do not do any automatic conflict resolution
                false, // don't add the file to the list of most-recently used Excel documents.
                Missing.Value, //don't specify a Unicode character page, as this is only necessary in Asian markets
                Missing.Value, //don't specify a visual layout, as this is only necessary in Asian markets
                true); // use the local language settings for saving the file.
        }
        private void BuildHeader()
        {
            // excel cells are 1-based indexing, whereas the ReportProcessor expects 0-based indexing
            int offset = 1;
            foreach (var group in this.reportProc.Groups)
            {
                if (this.reportProc.HeaderHeight == 2)
                {
                    ws.Cells[1, offset] = group.GroupName;
                    if (group.Fields.Length > 1)
                    {
                        var rowPart = "1";
                        var start = Base10ToBase26(offset - 1) + rowPart;
                        /* 
                        example:
                            I want to merge columns 2, 3, and 4. That's a width of 3 columns.
                            The starting column is 2, plus the width of 3 is 5, minus 1 gives me
                            me the ending column of 4. Then, convert to base 26.
                        */
                        var testNeg = offset + group.Fields.Length - 2;

                        //YOU ADDED THIS ON 07/09/2010 The if/else condition that is.
                        if (testNeg > 0)
                        {
                            var end = Base10ToBase26(testNeg) + rowPart;
                            var range = ws.get_Range(start, end);
                            range.Merge(false);
                        }
                        else
                        {
                            return;
                        }

                    }
                }

                for (int x = 0; x < group.Fields.Length; ++x)
                    ws.Cells[2, offset + x] = group.Fields[x].FieldDescription.Description;

                offset += group.Fields.Length;
            }

        }
        private void BuildBody()
        {
            for (int y = 0; y < this.reportProc.BodyHeight; ++y)
                for (int x = 0; x < this.reportProc.Width; ++x)
                    ws.Cells[y + 3, x + 1] = this.reportProc.Body[y][x];
        }

        private void StylizeHeader()
        {
            // pretty straight forward, similar to defining a CSS class.
            Style style = wb.Styles.Add("HeaderStyle", Missing.Value);
            style.Font.Bold = true;
            style.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            style.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Gray);
            style.Interior.Pattern = XlPattern.xlPatternSolid;


            //select the entire header. It is two rows deep when we have column grouping enabled.
            var end = Base10ToBase26(this.reportProc.Width - 1);
            Range row = (Range)ws.Cells.get_Range("A1", end + "2");
            row.Style = "HeaderStyle";

            //resize all of the columns to fit the data.
            ws.Columns.AutoFit();
        }

        /// <summary>
        /// Returns a base-26 "number" (i.e. A-Z character string) representing the last column of the report grid in Excel.
        /// Excel uses column names that increment in a sequence through the alphabet, i.e. A, B, C, ... Z, AA, AB, AC, ... AZ, BA, BB, etc.
        /// This is essentially a base-26 counting scheme. This function converts a base-10, positive integer into a base-26 column name.
        /// </summary>
        /// <param name="numColumns">How many columns wide the report is</param>
        /// <returns></returns>
        private static string Base10ToBase26(int N)
        {
            if (N < 0)
                throw new Exception("Cannot convert negative numbers to a base26 value");
            /* 
             First, figure out the number of numerals we will need. This is using base-10 logarithms to calculate
             the base-26 logarithm of the number of columns. The ceiling value of a base-B logarithm of a given
             number is the minimum number of numerals needed to write it out. Essentially, we're just finding the 
             first power of 26 that is larger than the number of columns.
            
             PROOF: 
               For a given base-B, any number N that is L digits long can be expressed as 
                          SUM(i from 0 to L - 1) { POW(B, i) * N[i] }
               where N[i] is the i'th digit of N, so by definition N[i] is always less than B
               
               This expands:
                          POW(B, 0) * N[0] + POW(B, 1) * N[1] + POW(B, 2) * N[2] + ... + POW(B, L - 1) * N[L - 1]
            
               The first power of B that is larger than N would be POW(B, L), but we do not yet know L. We do know that
               logarithms are defined as:
                    Given N = POW(B, C)
                        Log_B(N) = C
             
               Given 
                    A < B < C
               then 
                    log(A) < log(B) < log(C)
               So given 
                    POW(B, L - 1) <= N < POW(B, L)
               then 
                    Log_B(POW(B, L - 1)) <= Log_B(N) < Log_B(POW(B, L))
               i.e.
                    L - 1 <= Log_B(N) < L
              
               Since we want to find L, and the Log function is undefined for 0, 
               we add 1 and use the Ceiling function
                    Ceil(L - 1)) < Ceil(Log_B(N+1)) <= Ceil(L)
                    L - 1 < Ceil(Log_B(N+1)) = L
                    L = Ceil(Log_B(N+1))
              
               Finally, we do not have a logarithm function in base 26, we only have natural log.
               This is not a problem, as an identity for logs provides a way to convert between bases:
                    Log_A(B) = Log_C(B) / Log_C(A)
               So Log_26(N) = Log(N) / Log(26)
              
               And thus, out of all of that, we get this one line of code:
            */
            int numDigits = (int)Math.Floor(Math.Log(N + 1) / Math.Log(26)) + 1;

            // reserve space for each of the characters.
            char[] columns = new char[numDigits];

            // the value of the last column will be one less than the number of columns when counting from 0
            for (int i = 0; i < numDigits; ++i)
            {
                /* the value of the least significant digit in the number is the remainder of dividing the
                 number by 26, i.e. the modulus operation.
                 
                 PROOF:
                    For a given base B, any number N that is L digits long can be expressed as 
                            SUM(i from 0 to L - 1) { POW(B, i) * N[i] }
                    where N[i] is the i'th digit of N, so by definition N[i] is always less than B
               
                    This expands:
                          1 * N[0] + B * N[1] + POW(B, 2) * N[2] + ... + POW(B, L - 1) * N[L - 1]
                        = N[0] + B * N[1] + POW(B, 2) * N[2] + ... + POW(B, L - 1) * N[L - 1]

                    So N / B becomes:
                        SUM(i from 0 to L - 1) { POW(B, i) * N[i] / B }
                       =SUM(i from 0 to L - 1) { POW(B, i - 1) * N[i] }

                    which expands to 
                        POW(B, -1) * N[0] + POW(B, 0) * N[1] + POW(B, 1) * N[2] + ... + POW(B, L - 2) * N[L - 1]
                       =N[0] / B + N[1] + B * N[2] + ... + POW(B, L - 2) * N[L - 1]

                    N[0] / B is the non-integral portion of the division, so the moduls operation will return N[0]
                */
                columns[i] = (char)((N % 26) + 'A'); //adding the ASCII value of character A then gets us the ASCII value of the nth letter in the alphabet
                // we also don't have to check that this is in range, because the maximum value of N%26 is 25, so 'A' + 25 = 'Z'

                // performing an integer division truncates the remainder, essentially "shifting" the digits to the
                // right. We then repeat the process until we're out of digits.
                N /= 26;
            }

            // Why do we do all of this? Because it runs in O(log(n)) time instead of O(n)

            // Once we have our characters, just make a new string out of it.
            var end = new string(columns);
            return end;
        }

        public static int Build(IEnumerable<object> data, MainDataAccess dataAccess, string userName, string path)
        {
            var rep = new ReportProcessor(data, null);
            String fileName = (path + ".xls");
            using (var excel = new ExcelReportBuilder(rep, path))
            {
                excel.DataBind();
                fileName = (excel.FileName + ".xls");
            }

            var fso = dataAccess.SetFileData(
                userName,
                null,
                dataAccess.GetFileTypeID(userName, ".xls"),
                System.IO.Path.GetFileName(fileName),
                "Report",
                "Inventory Received Summary",
                System.IO.File.ReadAllBytes(fileName));
            //cleanup the temp file that the system created.
            System.IO.File.Delete(fileName);
            return fso;
        }

        public void Dispose()
        {
            //close the work book so that Excel releases the lock on the file
            wb.Close(false, this.FileName, false);
        }
    }
}
