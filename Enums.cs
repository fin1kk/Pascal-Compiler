using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kompilyatory_V1
{
    public enum TokenCode
    {
        none,

        programkw,
        varkw,
        beginkw,
        endkw,      
        realkw,
        booleankw,
        integerkw,
        stringkw,

        later,              // <
        laterOrEqual,       // <=
        greater,            // >
        greaterOrEqual,     // >=
        equal,              // =
        notEqual,           // <>
        assign,             // :=
        semicolon,          // ;
        colon,              // :
        point,              // .
        comma,              // ,
        plus,               // +
        minus,              // -
        star,               // *
        slash,              // /
        leftPar,            // (
        rightPar,           // )


        ident,


        int_const,
        str_const,
        bool_const,
        real_const

    }

}
