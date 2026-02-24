using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIMTools
{
    public static partial class StringExtentions
    {
        /// <summary>
        /// (Basicライクな)文字列の先頭から指定された文字数を取り出す。
        /// </summary>
        /// <param name="text">指定する文字</param>
        /// <param name="length">取り出す文字数</param>
        /// <returns>取り出された文字</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string Left(this string text, int length)
        {
            if (length < 0)
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            if (string.IsNullOrEmpty(text))
                return "";
            else if (text.Length > length)
                return text.Substring(0, length);
            else
                return text;
        }

        public static string Mid(this string str, int start)
        {
            return str.Mid(start, str.Length);
        }

        public static string Mid(this string str, int start, int len)
        {
            if (start <= 0)
            {
                throw new ArgumentException("引数'start'は1以上でなければなりません。");
            }
            if (len < 0)
            {
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            }
            if (str == null || str.Length < start)
            {
                return "";
            }
            if (str.Length < (start + len))
            {
                return str.Substring(start - 1);
            }
            return str.Substring(start - 1, len);
        }

        public static string Right(this string text, int length)
        {
            if (length < 0)
                throw new ArgumentException("引数'length'は0以上でなければなりません。");

            if (text == null)
                return "";
            else if (text.Length <= length)
                return text;
            return text.Substring(text.Length - length, length);
        }


        public static string Format(double value, string format)
        {
            return string.Format(format, value);
        }

        public static string Join(this List<string> list, string separator = ",") => string.Join(separator, list.ToArray());

        public static double HexStringToDouble(this string hexString)
        {


            byte[] doubleVals = hexString.ToByteSequence().ToArray();
            return BitConverter.ToDouble(doubleVals, 0);
        }

        /// <summary>
        /// 指定された文字列がnullまたは、String.Emptyであるかどうかを示します。
        /// </summary>
        /// <param name="value">文字列</param>
        /// <returns>nullまたは、String.Emptyのときtrue、それ以外の場合false。</returns>
        public static bool IsNullOrEmpty(this string value) =>
            System.String.IsNullOrEmpty(value);

        /// <summary>
        /// 文字列の値が空か空でないか結果を返し、空でない場合、その内容を文字列変数に入れて返す。
        /// </summary>
        /// <param name="value">検証する文字列</param>
        /// <param name="result">空白でない場合、代入された変数</param>
        /// <returns>空か空でないかの是非</returns>
        public static bool IsNotNullOrEmpty(this string value, out string result)
        {
            result = value;
            return !(System.String.IsNullOrEmpty(value));
        }


        public static byte[] Bytes(this string str)
        {
            var bs = new List<byte>();
            for (int i = 0; i < str.Length / 2; i++)
            {
                bs.Add(System.Convert.ToByte(str.Substring(i * 2, 2), 16));
            }
            return bs.ToArray();
        }

        public static IEnumerable<byte> ToByteSequence(this string source)
        {
            var sb = new StringBuilder(source);

            var str = sb
              .Replace("0x", "")
              .Replace(",", "")
              .Replace(" ", "")
              .ToString();

            for (int i = 0; i < str.Length / 2; i++)
            {
                yield return Convert.ToByte(str.Substring(i * 2, 2), 16);
            }
        }

        /// <summary>
        /// 文字列の後ろから指定した文字分削除した文字列を返す
        /// </summary>
        /// <param name="text"></param>
        /// <param name="countingFromBehind">後ろから何文字削除する</param>
        /// <returns>文字分削除した文字列</returns>
        public static string CutBack(this string text, int countingFromBehind) => text.Substring(0, text.Length - countingFromBehind);

        /// <summary>
        /// 文字列の前から指定した文字分削除した文字列を返す
        /// </summary>
        /// <param name="text"></param>
        /// <param name="countingFromBehind">前から何文字削除する</param>
        /// <returns>文字分削除した文字列</returns>
        public static string CutFlont(this string text, int countingFromFront) => text.Substring(countingFromFront);

        public static string Divide(this string A, char c, out string rest)
        {
            if (A.IndexOf(c) is int p && p > 0)
            {
                rest = A.Substring(p + 1);
                return A.Substring(0, p);
            }
            else
            {
                rest = "";
                return A;
            }
        }
        public static string DivideSlash(this string A, out string rest) => A.Divide('/', out rest);

        public static string[] Divide(this string sText, string delimiter)
        {
            if (sText.Length == 0 || delimiter.Length == 0)
            {
                return null;
            }
            List<string> list = new List<string>();
            string[] separator = new string[1]
            {
            delimiter
            };
            string[] array = sText.Split(separator, StringSplitOptions.None);
            string str = "";
            for (int i = 0; i < array.Length; i++)
            {
                str += array[i];
                string @string = Convert.ToChar(34).ToString();
                string[] array2 = Split(str, @string);
                string[] array3 = Split(str, "'");
                if (array2.Length % 2 == 0)
                {
                    str += delimiter;
                    continue;
                }
                if (array3.Length % 2 == 0)
                {
                    str += delimiter;
                    continue;
                }
                list.Add(str);
                str = "";
            }
            return list.ToArray();
        }
        public static string[] Split(string String1, string String2)
        {
            string[] separator = new string[1]
            {
            String2
            };
            return String1.Split(separator, StringSplitOptions.None);
        }
        public static string ToXPath(this string path, char parenthesis = '[')
        {
            if (path.IndexOf(parenthesis) is int p && p > 0)
            {
                var elementName = path.Divide('[', out string rest);

                switch (parenthesis)
                {
                    case '(': break;
                    case '[': rest = rest.EndsWith("]") ? rest.CutBack(1) : rest; break;
                }

                if (Guid.TryParse(rest, out Guid guid))
                    return $"{elementName}[@ID='{guid}']";
                else
                    return path;
            }
            else
            {
                return path;
            }
        }
        public static string GetElementName(this string path, out Guid id)
        {
            //HIMTools.Error.StartProc(MethodBase.GetCurrentMethod().DeclaringType, MethodBase.GetCurrentMethod().Name);

            if (path.IndexOf('[') > 0)
            {
                var elementName = path.Divide('[', out string rest);
                while (!rest.StartsWith("'") && rest.Length > 0)
                    rest = rest.Substring(1);
                while (!rest.EndsWith("'") && rest.Length > 0)
                    rest = rest.Substring(0, rest.Length - 1);
                //HIMTools.Error.WriteMessage(0, $"rest : 「{rest}」");

                if (Guid.TryParse(rest.Replace("'", ""), out Guid resultID))
                    id = resultID;
                else
                    id = Guid.Empty;

                //HIMTools.Error.WriteMessage(0, $"rest : 「{rest.Replace("'", "")}」=>{id}");
                return elementName;
            }
            id = Guid.Empty;
            //HIMTools.Error.ExitProc();
            return path;
        }

        #region Tryparse
        /// <summary>
        /// 指定された文字列がGuid型に変換できるか検証を行い、可能であればGUIDを返す。
        /// </summary>
        /// <param name="text">検証する文字列</param>
        /// <param name="guid">変換可能な時、out Guid guidで返す。</param>
        /// <returns>変換可能な時True</returns>
        public static bool GuidTryParse(this string text, out Guid guid)
        {
            return Guid.TryParse(text, out guid);
        }

        /// <summary>
        /// 指定された文字列がUInt型に変換できるか検証を行い、可能であればUIntを返す。
        /// </summary>
        /// <param name="numStr">指定文字列</param>
        /// <param name="result">変換されたUInt値</param>
        /// <returns>成功ならTrue</returns>
        public static bool UintTryParse(this string numStr, out uint result)
        {
            if (uint.TryParse(numStr, out result))
                return true;
            else if (numStr.StartsWith("0x"))
            {
                try
                {
                    result = Convert.ToUInt32(numStr, 16);
                    return true;
                }
                catch
                {
                }
            }
            result = 0;
            return false;
        }



        #endregion
    }
}
