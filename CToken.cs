using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompilyatory_V1
{
    class CToken
    {
        public string typeToken;
        public TokenCode tokenCode;
        public liter l;

        public CToken()
        {
            tokenCode = TokenCode.none;
        }

        public CToken(liter l)
        {
            this.l=l;
        }

    }

    /*class CConst : CToken
    {
        public CConst(liter l) : base ("Константа")
        {
            this.l=l;
        }

    }

    class COper : CToken
    {
        public COper(liter l) : base("Оператор")
        {
            this.l=l;
        }
    }

    class CKeyWord : CToken
    {
        public CKeyWord(liter l) : base("Ключевое слово")
        {
            this.l=l;
        }
    }

    class CIdent : CToken
    {
        public CIdent(liter l) : base("Идентификатор")
        {
            this.l=l;
        }
    }*/

        
}
