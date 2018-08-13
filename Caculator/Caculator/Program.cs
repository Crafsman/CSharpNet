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

        private static void Caculate(string[] expressionArray)
        {

            // caculate Ax + Bx
            int A = 0;
            int B = 0;
            for (int i = 0; i < expressionArray.Length; i++)
            {
                if (!expressionArray[i].Contains("X") &&
                    !expressionArray[i].Contains("+") &&
                    !expressionArray[i].Contains("-") &&
                    !expressionArray[i].Contains("*") &&
                    !expressionArray[i].Contains("/") &&
                    !expressionArray[i].Contains("%") &&
                    !expressionArray[i].Contains("^"))
                {
                    int number = Convert.ToInt32(expressionArray[i]);
                    if (i == 0)
                    {
                        B += number;
                    }
                    else
                    {
                        string opera = expressionArray[i - 1];
                        if (opera == "-")
                        {
                            B -= number;
                        }
                        else if (opera == "+")
                        {
                            B += number;
                        }
                    }
                }

                if (expressionArray[i].Contains("X"))
                {
                    string ax = expressionArray[i];
                    if (expressionArray[i].Length == 1)
                    {
                        //a=1
                        if (i == 0)
                        {
                            A += 1;
                        }
                        else
                        {
                            string opera = expressionArray[i - 1];
                            if (opera == "-")
                            {
                                A -= 1;
                            }
                        }

                    }
                    else
                    {
                        //a != 1
                        string a = expressionArray[i].Substring(0, expressionArray[i].Length - 1);
                        int ia = Convert.ToInt32(a);
                        //A += ia;
                        if (i == 0)
                        {
                            A += ia;
                        }
                        else
                        {
                            string opera = expressionArray[i - 1];
                            if (opera == "-")
                            {
                                A -= ia;
                            }
                            else if (opera == "+")
                            {
                                A += ia;
                            }
                        }
                    }
                }

            }

            Console.WriteLine("A {0}", A);
            Console.WriteLine("B {0}", B);
            Console.WriteLine("x = {0}", -B / A);
        }
        public static void Main(string[] args)
        {
            while(true)
            {
                string expression = ValidateExpression();
                // split by =, and put right expressions to left
                string[] expressions = expression.Split('=');
                if (expressions[1].Trim() == "0")
                {
                    //patern AX + B = 0; X = -B/A
                    //splict left expression by white space 
                    string[] expressionArray = expressions[0].Trim().Split(' ');
                    Caculate(expressionArray);


                }
                else // patern ax + b = c
                {
                    string rightExpressions = expressions[1].Trim();
                }

                if (expressions.Length == 1)
                {
                    //
                    string leftExpression = expressions[0];
                    Console.WriteLine("Pattern right: {0}", leftExpression);
                }
                else if (expression.Length == 2)
                {
                    string rightExpression = expressions[1];
                }
                else
                {
                    //worng expression
                }
                //Console.WriteLine("Pattern right: {0}", expression);
                //Console.ReadLine();
            }
           



                
        }
    }
}
