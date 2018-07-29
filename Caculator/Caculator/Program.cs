using System;

namespace Caculator
{
    class MainClass
    {
        string[] opers = new string[] { "+", "-", "/", "*", "%", "^" };  
        private enum Operation
        {
            Unknown,

            Addition,

            Subtraction,

            Multiplication,

            Division,

            Exponentiation,

            Modulo
        };

        public static void Linear()
        {
            //x=-b/a
        }

        //Validate expression, if the expression is correct, remove "cal ", and
        //return the right expression
        public static string ValidateExpression()
        {
            int index = -1;
            string expression = "";
            while (true)
            {
                expression = Console.ReadLine();

                index = expression.IndexOf("calc ", StringComparison.CurrentCulture);

                if (index != 0)
                {
                    Console.WriteLine("Please follow this pattern: \"calc aX + b = c\"");
                    continue;
                }
                if (!expression.Contains("X"))
                {
                    Console.WriteLine("Please input X as equation variable");
                    continue;
                }

                if (index == 0)
                {
                    expression = expression.Remove(0, 5);
                    break;
                }
            }

            return expression;
        }

        public static void Main(string[] args)
        {
            string expression = ValidateExpression();
            Console.WriteLine("Pattern right: {0}", expression);
            Console.ReadLine();



                
        }
    }
}
