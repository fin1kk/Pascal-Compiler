using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompilyatory_V1
{
    internal class CCompiler
    {
        CToken curToken;
        CToken curIdent;
        LexAnaliz lA = new LexAnaliz();

        string identName="";
        string identType="";

        public Dictionary<string, string> identList = new Dictionary<string, string>();
        public Dictionary<string, bool> usedList = new Dictionary<string, bool>();
        public List<string> errorList = new List<string>();


        void skipBlock(List<TokenCode> starters)
        {
                while(!starters.Contains(curToken.tokenCode))
                        curToken = lA.GetLexemToken();                  
        }

        private void accept(TokenCode tokenCode)
        {
            if (curToken == null || curToken.tokenCode != tokenCode)
            {
                throw new Exception("expected another Token Code: " + tokenCode + " in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
            }
            curToken = lA.GetLexemToken();
            if(tokenCode == TokenCode.point && curToken.tokenCode != TokenCode.none)
                throw new Exception("extra characters after the point ");

        }

        ///////////////Start//////////////////
        public void programBlock()
        {
            try
            {
                curToken = lA.GetLexemToken();
                identType = curToken.l.value;
                accept(TokenCode.programkw);
                identName = curToken.l.value;
                accept(TokenCode.ident);
                accept(TokenCode.semicolon);
            }
            catch (Exception e)
            {
                errorList.Add(e.Message);
                skipBlock(new List<TokenCode>(){ TokenCode.programkw, TokenCode.varkw, TokenCode.beginkw, TokenCode.none });
            }

            identList.Add(identName, identType);
            usedList.Add(identName, false);
            mainBlock();

            try
            {

                accept(TokenCode.point);
            }
            catch (Exception e)
            {
                errorList.Add(e.Message);
            }

        }

        void mainBlock()
        {

            varBlock();          

            try
            {
                    accept(TokenCode.beginkw);
            }
            catch (Exception e)
            {
                errorList.Add(e.Message);
                skipBlock(new List<TokenCode>() { TokenCode.beginkw, TokenCode.none });
            }

            while (curToken.tokenCode == TokenCode.ident)
            {
                
                try
                {
                    if (!identList.TryGetValue(curToken.l.value, out var listv))
                        throw new Exception("non-existent identifier: '" + curToken.l.value + "'  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
                    curIdent=curToken;                  
                    accept(TokenCode.ident);
                    accept(TokenCode.assign);
                }
                catch (Exception e)
                {
                    errorList.Add(e.Message);
                    skipBlock(new List<TokenCode>() { TokenCode.semicolon, TokenCode.endkw, TokenCode.none });
                }
                if (curToken.tokenCode != TokenCode.endkw && curToken.tokenCode != TokenCode.semicolon && curToken.tokenCode != TokenCode.none)
                    try
                    {
                        Expression();
                        usedList[curIdent.l.value]=true;
                    }
                    catch (Exception e)
                    {
                        errorList.Add(e.Message);
                        curToken = lA.GetLexemToken();
                        skipBlock(new List<TokenCode>() { TokenCode.ident, TokenCode.endkw, TokenCode.none });
                    }
                if(curToken.tokenCode == TokenCode.semicolon)
                    curToken = lA.GetLexemToken();

            }

            try
            {
                accept(TokenCode.endkw);
            }
            catch (Exception e)
            {
                errorList.Add(e.Message);
                skipBlock(new List<TokenCode>() { TokenCode.point, TokenCode.none });
            }

        }

        void varBlock()          //проверка ключевого слова и части описания переменных одного типа
        {
            if (curToken.tokenCode == TokenCode.varkw)
            {
                curToken = lA.GetLexemToken();

                do
                {
                    varDeclaration();
                 
                } while (curToken.tokenCode == TokenCode.ident);
            }
            else
                skipBlock(new List<TokenCode>() { TokenCode.varkw, TokenCode.beginkw, TokenCode.none });
        }

        void varDeclaration()   //проверка описания переменных одного типа
        {
            List<string> IdName = new List<string>();

            try
            {
                if (curToken.tokenCode == TokenCode.ident)
                {
                    if (identList.TryGetValue(curToken.l.value, out var listv))
                        throw new Exception("such an identifier already exists:  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
                    IdName.Add(curToken.l.value);
                }
                accept(TokenCode.ident);

                while (curToken.tokenCode == TokenCode.comma)
                {
                    curToken = lA.GetLexemToken();

                    if (curToken.tokenCode == TokenCode.ident)
                    {
                        if (identList.TryGetValue(curToken.l.value, out var listv))
                            throw new Exception("such an identifier already exists:  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
                        IdName.Add(curToken.l.value);
                    }
                    accept(TokenCode.ident);
                }
                accept(TokenCode.colon);

                identType = curToken.l.value;
                type();

                accept(TokenCode.semicolon);
            }
            catch (Exception e)
            {
                errorList.Add(e.Message);
                skipBlock(new List<TokenCode>() { TokenCode.semicolon, TokenCode.beginkw, TokenCode.none });
                if (curToken.tokenCode == TokenCode.semicolon)
                    curToken = lA.GetLexemToken();
            }

            for (int i = 0; i<IdName.Count; i++)
            {
                identList.Add(IdName[i], identType);
                usedList.Add(IdName[i], false);
            }
        }

        void type()             //проверка ключевого слова, отвечающего за тип переменных
        {
            if (curToken.tokenCode != TokenCode.none)
            {
                switch (curToken.tokenCode)
                {
                    case TokenCode.realkw:
                    case TokenCode.booleankw:
                    case TokenCode.integerkw:
                    case TokenCode.stringkw:
                        curToken = lA.GetLexemToken();
                        break;
                    default:
                        throw new Exception("expected another type of variable:  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
                }
            }
            else
                throw new Exception("expected type of variable:  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
        }


        void expressionPar()
        {
            if (curToken.tokenCode == TokenCode.leftPar)
            {

                accept(TokenCode.leftPar);
                expressionPar();
                accept(TokenCode.rightPar);
                ExpressionOp();
            }
            else
                Expression();
        }

        void Expression()
        {
            switch (curToken.tokenCode)
            {
                case TokenCode.leftPar:
                    expressionPar();
                    break;

                case TokenCode.ident:
                    if (!identList.TryGetValue(curToken.l.value, out var listv)) 
                        throw new Exception("non-existent identifier: '" + curToken.l.value + "'  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);                  
                    if (identList[curToken.l.value] != identList[curIdent.l.value] && !(identList[curToken.l.value] == "integer" && identList[curIdent.l.value] == "real"))
                        throw new Exception("Incompatible data types: '" + identList[curIdent.l.value] + "' and '" + identList[curToken.l.value] + "'  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
                    if (usedList[curToken.l.value] == false)
                        throw new Exception("Empty variable: '" + curToken.l.value + "'  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
                    accept(TokenCode.ident);
                    ExpressionOp();
                    break;

                case TokenCode.int_const:
                    if (identList[curIdent.l.value] != "integer" && identList[curIdent.l.value] != "real")
                        throw new Exception("Incompatible data types: '" + identList[curIdent.l.value] + "' and 'integer'" + "  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
                    accept(TokenCode.int_const);
                    ExpressionOp();
                    break;

                case TokenCode.real_const:
                    if (identList[curIdent.l.value] != "real")
                        throw new Exception("Incompatible data types: '" + identList[curIdent.l.value] + "' and 'real'" + "  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
                    accept(TokenCode.real_const);
                    ExpressionOp();
                    break;

                case TokenCode.bool_const:
                    if (identList[curIdent.l.value] != "boolean")
                        throw new Exception("Incompatible data types: '" + identList[curIdent.l.value] + "' and 'boolean" + "  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
                    accept(TokenCode.bool_const);
                    break;

                case TokenCode.str_const:
                    if (identList[curIdent.l.value] != "string")
                        throw new Exception("Incompatible data types: '" + identList[curIdent.l.value] + "' and 'string'" + "  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
                    accept(TokenCode.str_const);
                    ExpressionOp();
                    break;

                case TokenCode.plus:
                case TokenCode.minus:
                    curToken = lA.GetLexemToken();
                    Expression();
                    break;
                default:
                    throw new Exception("expected another Expression:  in row " + curToken.l.rowPl + "in col " + curToken.l.colPl);
            }
        }

        void ExpressionOp()
        {
            switch (curToken.tokenCode)
            {
                case TokenCode.plus:
                case TokenCode.minus:
                case TokenCode.star:
                case TokenCode.slash:
                    curToken = lA.GetLexemToken();
                    Expression();
                    break;

                case TokenCode.semicolon:
                    accept(TokenCode.semicolon);
                    break;

                case TokenCode.rightPar:
                    break;

                default:
                    throw new Exception("expected another kw:  in row " + curToken.l.rowPl + " in col " + curToken.l.colPl);
            }
        }

    }
}
