using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompilyatory_V1
{
    internal class InOut
    {
        StreamReader f1;
        int curRow=0;
        int curCol=0;
        string curStr = "";

        internal InOut(string path)
        {
            f1 = new StreamReader(path);
            curStr = f1.ReadLine();
        }

        public liter GetNextL()
        {
            string str = "";
            if (curStr != null && curCol >= curStr.Length || curStr == null || (curStr != null && curCol+1 < curStr.Length && curStr[curCol] == '/' && curStr[curCol+1] =='/'))
            {
                str = f1.ReadLine();
                curStr =str;
                curRow++;
                curCol=0;
            }
            else
            {
                str = curStr;
            }
            if (str !=null)
            {
                string sl = "";
                liter l = new liter();
                int i = curCol;
                while (i < str.Length && (str[i] == ' ' || str[i] == '\t'))
                    i++;
                if (curStr != null && i+1 < curStr.Length && curStr[i] == '/' && curStr[i+1] =='/')
                {
                    str = f1.ReadLine();
                    curStr =str;
                    curRow++;
                    curCol=0;
                    return GetNextL();
                }
                else
                if (i < str.Length && (str[i] != ' ' || str[i] != '\t'))
                {

                    sl += str[i];
                    switch (str[i])
                    {
                        case ('+' or '*' or '/' or ';' or '=' or '\'' or ',' or '{' or '}' or '(' or ')' or '[' or ']'):
                            break;
                        case (':' or '<' or '>'):
                            if (i+1 < str.Length)
                            {
                                if (str[i+1] == '=' || str[i+1] == '>')
                                {
                                    sl+=str[i+1];
                                    i++;
                                }
                            }
                            break;

                        case ('-'):
                                while (i+1<str.Length && (str[i + 1] >= '0' && str[i + 1] <= '9' || str[i + 1] == '.'))
                                {
                                    sl+=str[i+1];
                                    i++;
                                }
                            break;
                        case ('\"'):
                            while (i+1 < str.Length && str[i + 1] !='\"')
                            {
                                sl += str[i+1];
                                i++;
                            }
                            if (i+1 < str.Length && str[i + 1] == '\"')
                            {
                                sl += str[i+1];
                                i++;
                            }
                            break ;
                        default:
                            if (str[i] >= '0' && str[i] <= '9')
                            {
                                while (i+1<str.Length && (str[i + 1] >= '0' && str[i + 1] <= '9' || str[i + 1] == '.'))
                                {
                                    sl += str[i + 1];
                                    i++;
                                }
                            }
                            else
                            {
                                while (i+1 < str.Length && (str[i+1] >= 'a' && str[i+1] <= 'z' || str[i+1] >= 'A' && str[i+1] <= 'Z' || str[i+1] >= 'а' && str[i+1] <= 'я' || str[i+1] >= 'А' && str[i+1] <= 'Я' || str[i + 1] >= '0' && str[i + 1] <= '9' || str[i + 1] =='\"'))
                                {
                                    sl += str[i+1];
                                    i++;
                                }
                            }
                            break;
                    }

                    l.value = sl;
                    l.rowPl = curRow;
                    l.colPl = curCol;
                    //l.colPlf = i;             
                    Console.WriteLine(sl + " " + curRow.ToString() + " " + curCol.ToString());
                    i++;
                    curCol=i;
                }
                if (i == str.Length && sl == "")
                {
                    curCol = str.Length;
                    return GetNextL();
                }
                else
                    return l;
            }
            else
            {
                liter l = new liter();
                return l;
            }
        }    

    }
}
