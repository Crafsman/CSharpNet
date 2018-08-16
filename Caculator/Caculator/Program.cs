using System;
using System.Collections.Generic;

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
                if (!expression.ToLower().Contains("x"))
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
        public static List<string> StandardizeExpression(List<string> expressions)
        {
            //string standardExpression = "";
            for (int i = 0; i < expressions.Count; i++)
            {
                if (expressions[i].ToLower().Contains("x"))
                {
                    if (expressions[i].Contains("*"))
                    {
                        expressions[i] = expressions[i].Replace("*", "");
                    }
                }
                else  // convert number???
                {
                    if (expressions[i].Contains("*"))
                    {
                        double newValue = Convert.ToDouble(expressions[i - 1]) * Convert.ToDouble(expressions[i + 1]);
                        expressions[i] = expressions[i].Replace("*", newValue.ToString());
                        expressions[i - 1] = expressions[i - 1].Replace(expressions[i - 1], "");
                        expressions[i + 1] = expressions[i + 1].Replace(expressions[i + 1], "");

                    } //Parse patern 5(2) + 5X = 15
                    else if(expressions[i].Contains("(") && expressions[i].Contains(")"))
                    {
                        int index = expressions[i].IndexOf('(');
                        double beforeNumber = Convert.ToDouble(expressions[i].Substring(index - 1, 1));
                        double behindNumber = Convert.ToDouble(expressions[i].Substring(index + 1, 1));
                        expressions[i] = (beforeNumber * behindNumber).ToString();
                    }
                }              
               
            }
            // handle '/'
            for (int i = 0; i < expressions.Count; i++)
            {
                if (expressions[i].Contains("/"))
                {
                    double Denominator = 1 / Convert.ToDouble(expressions[i + 1]);
                    expressions[i] = expressions[i].Replace(expressions[i], "");
                    expressions[i + 1] = expressions[i + 1].Replace(expressions[i + 1], "");
                    expressions[i - 1] = Denominator.ToString() + expressions[i - 1];
                }
            }



            List<string> standardExpression = new List<string>();
            for (int j = 0; j < expressions.Count; j++)
            {
                if (!String.IsNullOrEmpty(expressions[j]))
                {
                    standardExpression.Add(expressions[j]);
                }
            }

            return standardExpression;
        }

        private static double[] Caculate(List<string> expressionArray)
        {
            // caculate Ax + Bx + C
            double[] AB = new double[2];
            double A = 0;
            double B = 0;
            for (int i = 0; i < expressionArray.Count; i++)
            {
                //Caculate C
                if (!expressionArray[i].ToLower().Contains("x") &&
                    !expressionArray[i].Contains("+") &&
                    !expressionArray[i].Contains("-") &&
                    !expressionArray[i].Contains("*") &&
                    !expressionArray[i].Contains("/") &&
                    !expressionArray[i].Contains("%") &&
                    !expressionArray[i].Contains("^"))
                {
                    double number = Convert.ToDouble(expressionArray[i]);
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
                if (expressionArray[i].ToLower().Contains("x"))
                {
                    string ax = expressionArray[i];
                    if (ax.Contains("*"))
                    {
                        ax = expressionArray[i].Replace("*", "");
                    }
                    // 
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
                    }// ax not at the begining
                    else
                    {
                        //extract a,
                        string a = ax.Substring(0, ax.Length - 1);
                        double ia = Convert.ToDouble(a);
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

#if DEBUG
            Console.WriteLine("A {0}", A);
            Console.WriteLine("B {0}", B);
#endif
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
                // Left
                string[] leftExpressions = expressions[0].Trim().Split(' ');
                List<string> leftExpressionsList = new List<string>(leftExpressions);
                List<string> newLeftExpressions = StandardizeExpression(leftExpressionsList);
                double[] value1 = Caculate(newLeftExpressions);
                          
                //Right
                string[] rightExpressions = expressions[1].Trim().Split(' ');
                List<string> rightExpressionsList = new List<string>(rightExpressions);
                List<string> newRightExpressions = StandardizeExpression(rightExpressionsList);
                double[] value2 = Caculate(newRightExpressions);

                // X value
                double A = value1[0] - value2[0];
                double B = value1[1] - value2[1];
                if(A == 0)
                {
                    Console.WriteLine("Denominator cannot be 0");
                }
                Console.WriteLine("x =  {0}", -B/A);

            }





        }
    }
}
