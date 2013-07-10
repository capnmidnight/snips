using System;
using System.Collections.Generic;
using System.Drawing;

namespace BarcodeGenerator
{
    class BarcodeGen
    {
        static Dictionary<char, string> asciiMap;
        static Dictionary<char, string> code39Map;
        static BarcodeGen()
        {
            asciiMap = new Dictionary<char, string>();
            asciiMap.Add((char)0, "%U");
            asciiMap.Add((char)1, "$A");
            asciiMap.Add((char)2, "$B");
            asciiMap.Add((char)3, "$C");
            asciiMap.Add((char)4, "$D");
            asciiMap.Add((char)5, "$E");
            asciiMap.Add((char)6, "$F");
            asciiMap.Add((char)7, "$G");
            asciiMap.Add((char)8, "$H");
            asciiMap.Add((char)9, "$I");
            asciiMap.Add((char)10, "$J");
            asciiMap.Add((char)11, "$K");
            asciiMap.Add((char)12, "$L");
            asciiMap.Add((char)13, "$M");
            asciiMap.Add((char)14, "$N");
            asciiMap.Add((char)15, "$O");
            asciiMap.Add((char)16, "$P");
            asciiMap.Add((char)17, "$Q");
            asciiMap.Add((char)18, "$R");
            asciiMap.Add((char)19, "$S");
            asciiMap.Add((char)20, "$T");
            asciiMap.Add((char)21, "$U");
            asciiMap.Add((char)22, "$V");
            asciiMap.Add((char)23, "$W");
            asciiMap.Add((char)24, "$X");
            asciiMap.Add((char)25, "$Y");
            asciiMap.Add((char)26, "$Z");
            asciiMap.Add((char)27, "%A");
            asciiMap.Add((char)28, "%B");
            asciiMap.Add((char)29, "%C");
            asciiMap.Add((char)30, "%D");
            asciiMap.Add((char)31, "%E");
            asciiMap.Add((char)32, " ");
            asciiMap.Add((char)33, "/A");
            asciiMap.Add((char)34, "/B");
            asciiMap.Add((char)35, "/C");
            asciiMap.Add((char)36, "/D");
            asciiMap.Add((char)37, "/E");
            asciiMap.Add((char)38, "/F");
            asciiMap.Add((char)39, "/G");
            asciiMap.Add((char)40, "/H");
            asciiMap.Add((char)41, "/I");
            asciiMap.Add((char)42, "/J");
            asciiMap.Add((char)43, "/K");
            asciiMap.Add((char)44, "/L");
            asciiMap.Add((char)45, "-");
            asciiMap.Add((char)46, ".");
            asciiMap.Add((char)47, "/O");
            asciiMap.Add((char)48, "0");
            asciiMap.Add((char)49, "1");
            asciiMap.Add((char)50, "2");
            asciiMap.Add((char)51, "3");
            asciiMap.Add((char)52, "4");
            asciiMap.Add((char)53, "5");
            asciiMap.Add((char)54, "6");
            asciiMap.Add((char)55, "7");
            asciiMap.Add((char)56, "8");
            asciiMap.Add((char)57, "9");
            asciiMap.Add((char)58, "/Z");
            asciiMap.Add((char)59, "%F");
            asciiMap.Add((char)60, "%G");
            asciiMap.Add((char)61, "%H");
            asciiMap.Add((char)62, "%I");
            asciiMap.Add((char)63, "%J");
            asciiMap.Add((char)64, "%V");
            asciiMap.Add((char)65, "A");
            asciiMap.Add((char)66, "B");
            asciiMap.Add((char)98, "+B");
            asciiMap.Add((char)67, "C");
            asciiMap.Add((char)68, "D");
            asciiMap.Add((char)69, "E");
            asciiMap.Add((char)70, "F");
            asciiMap.Add((char)71, "G");
            asciiMap.Add((char)72, "H");
            asciiMap.Add((char)73, "I");
            asciiMap.Add((char)74, "J");
            asciiMap.Add((char)75, "K");
            asciiMap.Add((char)76, "L");
            asciiMap.Add((char)77, "M");
            asciiMap.Add((char)78, "N");
            asciiMap.Add((char)79, "O");
            asciiMap.Add((char)80, "P");
            asciiMap.Add((char)81, "Q");
            asciiMap.Add((char)82, "R");
            asciiMap.Add((char)83, "S");
            asciiMap.Add((char)84, "T");
            asciiMap.Add((char)85, "U");
            asciiMap.Add((char)86, "V");
            asciiMap.Add((char)87, "W");
            asciiMap.Add((char)88, "X");
            asciiMap.Add((char)89, "Y");
            asciiMap.Add((char)90, "Z");
            asciiMap.Add((char)91, "%K");
            asciiMap.Add((char)92, "%L");
            asciiMap.Add((char)93, "%M");
            asciiMap.Add((char)94, "%N");
            asciiMap.Add((char)95, "%O");
            asciiMap.Add((char)96, "%W");
            asciiMap.Add((char)97, "+A");
            asciiMap.Add((char)99, "+C");
            asciiMap.Add((char)100, "+D");
            asciiMap.Add((char)101, "+E");
            asciiMap.Add((char)102, "+F");
            asciiMap.Add((char)103, "+G");
            asciiMap.Add((char)104, "+H");
            asciiMap.Add((char)105, "+I");
            asciiMap.Add((char)106, "+J");
            asciiMap.Add((char)107, "+K");
            asciiMap.Add((char)108, "+L");
            asciiMap.Add((char)109, "+M");
            asciiMap.Add((char)110, "+N");
            asciiMap.Add((char)111, "+O");
            asciiMap.Add((char)112, "+P");
            asciiMap.Add((char)113, "+Q");
            asciiMap.Add((char)114, "+R");
            asciiMap.Add((char)115, "+S");
            asciiMap.Add((char)116, "+T");
            asciiMap.Add((char)117, "+U");
            asciiMap.Add((char)118, "+V");
            asciiMap.Add((char)119, "+W");
            asciiMap.Add((char)120, "+X");
            asciiMap.Add((char)121, "+Y");
            asciiMap.Add((char)122, "+Z");
            asciiMap.Add((char)123, "%P");
            asciiMap.Add((char)124, "%Q");
            asciiMap.Add((char)125, "%R");
            asciiMap.Add((char)126, "%S");
            asciiMap.Add((char)127, "%Z");

            code39Map = new Dictionary<char, string>();
            code39Map.Add('*', "100101101101");
            code39Map.Add('-', "100101011011");
            code39Map.Add('$', "100100100101");
            code39Map.Add('%', "101001001001");
            code39Map.Add(' ', "100110101101");
            code39Map.Add('.', "110010101101");
            code39Map.Add('/', "100100101001");
            code39Map.Add('+', "100101001001");
            code39Map.Add('0', "101001101101");
            code39Map.Add('1', "110100101011");
            code39Map.Add('2', "101100101011");
            code39Map.Add('3', "110110010101");
            code39Map.Add('4', "101001101011");
            code39Map.Add('5', "110100110101");
            code39Map.Add('6', "101100110101");
            code39Map.Add('7', "101001011011");
            code39Map.Add('8', "110100101101");
            code39Map.Add('9', "101100101101");
            code39Map.Add('A', "110101001011");
            code39Map.Add('B', "101101001011");
            code39Map.Add('C', "110110100101");
            code39Map.Add('D', "101011001011");
            code39Map.Add('E', "110101100101");
            code39Map.Add('F', "101101100101");
            code39Map.Add('G', "101010011011");
            code39Map.Add('H', "110101001101");
            code39Map.Add('I', "101101001101");
            code39Map.Add('J', "101011001101");
            code39Map.Add('K', "110101010011");
            code39Map.Add('L', "101101010011");
            code39Map.Add('M', "110110101001");
            code39Map.Add('N', "101011010011");
            code39Map.Add('O', "110101101001");
            code39Map.Add('P', "101101101001");
            code39Map.Add('Q', "101010110011");
            code39Map.Add('R', "110101011001");
            code39Map.Add('S', "101101011001");
            code39Map.Add('T', "101011011001");
            code39Map.Add('U', "110010101011");
            code39Map.Add('V', "100110101011");
            code39Map.Add('W', "110011010101");
            code39Map.Add('X', "100101101011");
            code39Map.Add('Y', "110010110101");
            code39Map.Add('Z', "100110110101");
        }

        public static Bitmap Make(int xDim, string message)
        {
            string ascii = "";
            foreach (char c in message)
                ascii += asciiMap[c];
            string code = "";
            foreach (char c in ascii)
                code += code39Map[c];
            Bitmap output = new Bitmap(xDim * (code.Length + 24), xDim * 74);
            using (Graphics g = Graphics.FromImage(output))
            {
                int start = xDim * 12;
                int top = xDim * 12;
                int bottom = xDim * 62;
                Pen pen = new Pen(Brushes.Black, xDim);
                foreach(char c in code)
                {
                    if (c == '1')
                        g.DrawLine(pen, start, top, start, bottom);
                    start+=xDim;
                }
            }
            return output;
        }
    }
}
