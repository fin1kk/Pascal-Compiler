using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompilyatory_V1
{
    internal class LexAnaliz
    {

        InOut inout;
        liter l;

        public CToken GetLexemToken()
        {
            CToken token = new CToken();
            l= inout.GetNextL();
            if (l.value !=null)
            {            
                TokenCode tokC = isOperator(l.value);
                if (tokC != TokenCode.none)
                {
                    token = new CToken(l);
                    token.tokenCode = tokC;
                    token.typeToken = "Оператор";
                }
                else
                {
                    tokC = isKeyWord(l.value);
                    if (tokC != TokenCode.none)
                    {
                        token = new CToken(l);
                        token.tokenCode = tokC;
                        token.typeToken = "Ключевое слово";
                    }
                    else
                    {
                        tokC = isConst(l.value);
                        if (tokC != TokenCode.none)
                        {
                            token = new CToken(l);
                            token.tokenCode = tokC;
                            token.typeToken = "Константа";
                        }
                        else
                        if (isIdent(l.value) != TokenCode.none)
                        {
                            token = new CToken(l);
                            token.tokenCode = TokenCode.ident;
                            token.typeToken = "Идентификатор";
                        }
                    }
                }

            }
            Console.WriteLine("Тип токена: '" + token.typeToken + "' код токена: '" + token.tokenCode + "'");
            return token;
        }

        internal LexAnaliz()
        {
            inout = new InOut(@"D:\Лабы\Компиляторы\2.txt");
        }

        private TokenCode isDigit(string s)
        {
            int i = 0;
            if (s!="")
            {
                if (s[i]=='-')
                    i++;
                while (i<s.Length && s[i] >= '0' && s[i] <= '9')
                    i++;
                if (i==s.Length)
                    return TokenCode.int_const;
                else
                if (s[i]=='.')
                {
                    i++;
                    while (i<s.Length && s[i] >= '0' && s[i] <= '9')
                        i++;
                    if (i==s.Length)
                        return TokenCode.real_const;
                }
            }
            return TokenCode.none;
        }

        private TokenCode isOperator(string s)
        {
            switch (s)
            {
                case ("<"): return TokenCode.later;
                case ("<="): return TokenCode.laterOrEqual;
                case (">"): return TokenCode.greater;
                case (">="): return TokenCode.greaterOrEqual;
                case ("="): return TokenCode.equal;
                case (":="): return TokenCode.assign;
                case (";"): return TokenCode.semicolon;
                case (":"): return TokenCode.colon;
                case ("."): return TokenCode.point;
                case (","): return TokenCode.comma;
                case ("+"): return TokenCode.plus;
                case ("-"): return TokenCode.minus;
                case ("*"): return TokenCode.star;
                case ("/"): return TokenCode.slash;
                case ("("): return TokenCode.leftPar;
                case (")"): return TokenCode.rightPar;
                case ("<>"): return TokenCode.notEqual;
            }

            return TokenCode.none;
        }

        private TokenCode isConst(string s)
        {
            TokenCode isD = isDigit(s);
            if (isD!= TokenCode.none)
                return isD;
            else
            if(s == "true" || s == "false" || s == "True" || s == "False")
                return TokenCode.bool_const;
            else
            if (s[0] == '"' && s[s.Length - 1] == '"' && s.Length > 1)
                return TokenCode.str_const;
            else
                return TokenCode.none;
        }

        private TokenCode isIdent(string s)
        {
            if (s[0] >= 'a' && s[0] <= 'z' || s[0] >= 'A' && s[0] <= 'Z')
            {
                for (int i = 1; i < s.Length; i++)
                    if (!((s[i] >= 'a' && s[i] <= 'z') || (s[i] >= 'A' && s[i] <= 'Z') || (s[i] >= '0' && s[i] <= '9')))
                        return TokenCode.none;
                return TokenCode.ident;
            }
            else return TokenCode.none;
        }

        private TokenCode isKeyWord(string s)
        {

            switch (s)
            {
                case ("var"): return TokenCode.varkw;
                case ("begin"): return TokenCode.beginkw;
                case ("end"): return TokenCode.endkw;
                case ("program"): return TokenCode.programkw;
                case ("real"): return TokenCode.realkw;
                case ("boolean"): return TokenCode.booleankw;
                case ("integer"): return TokenCode.integerkw;
                case ("string"): return TokenCode.stringkw;
            }
            return TokenCode.none;
        }
    }
}
