using System;
using System.IO;

namespace Kompilyatory_V1 {

    struct liter {
        public string value;
        public int rowPl;
        public int colPl;
    }

    class Start {

        public static void Main()
        {
            CCompiler cc = new CCompiler();
            cc.programBlock();

            Console.WriteLine();
            foreach (var ident in cc.identList)
                Console.WriteLine(ident);

            Console.WriteLine();
            for (int i = 0; i<cc.errorList.Count; i++)
                Console.WriteLine(cc.errorList[i]);
        }
    }
}
