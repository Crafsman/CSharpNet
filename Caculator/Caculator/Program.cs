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

        //reduce * 
        public static void ReduceOperations(ref string[] expressions)
        {
            //string standardExpression = "";
            for (int i = 0; i < expressions.Length; i++)
            {
                if (expressions[i].Contains("X"))
                {
                    if (expressions[i].Contains("*"))
                    {
                        expressions[i] = expressions[i].Replace("*", "");
                    }
                }
                // convert number???
            }

            
            //return standardExpression;
        }

        private static int[] Caculate(string[] expressionArray)
        {
            // caculate Ax + Bx + C
            int[] AB = new int[2];
            int A = 0;
            int B = 0;
            for (int i = 0; i < expressionArray.Length; i++)
            {
                //Caculate C
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

                //Caculate the substring that contains X
                if (expressionArray[i].Contains("X"))
                {
                    string ax = expressionArray[i];
                    if (ax.Contains("*"))
                    {
                        ax = expressionArray[i].Replace("*", "");
                    }

                    if (ax.Length == 1)
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
                        string a = ax.Substring(0, ax.Length - 1);
                        int ia = Convert.ToInt32(a);
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
            AB[0] = A;
            AB[1] = B; 
            return AB;
        }
        public static void Main(string[] args)
        {
            while(true)
            {
                string expression = ValidateExpression();

                // split expression by =
                string[] expressions = expression.Split('=');
                
                //AX + B = CX +D => 
                string[] leftExpressions = expressions[0].Trim().Split(' ');
                //Convert2StandardEquation();
                int[] value1 = Caculate(leftExpressions);
                             
                string[] rightExpressions = expressions[1].Trim().Split(' ');
                int[] value2 = Caculate(rightExpressions);

                int A = value1[0] - value2[0];
                int B = value1[1] - value2[1];
                Console.WriteLine("x =  {0}", -B/A);


            }





        }
    }
}
