﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
                    Console.WriteLine("Please begin with \"calc \"");
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

        private static string LinearOrQuatratic(string expression)
        {
            string linearOrQuatratic = "linear";

            if (Regex.IsMatch(expression, @"((\s)*\((.*(([0-9]*)x).*)+\)(\s)*){2}|(\^2)", RegexOptions.IgnoreCase))
            {
                 linearOrQuatratic = "quatratic";                
            }
#if DEBUG
            Console.WriteLine("{0}", linearOrQuatratic);
#endif
            return linearOrQuatratic;
        }

        private static double MatchCoefficent(string mathString)
        {
            string pattern = @"(\d)+(\s)*\(";
            double coefficent = 1;
            Match currentMath = Regex.Match(mathString, pattern, RegexOptions.IgnoreCase);
            if (currentMath.Success)
            {
                Match currentCoefficent = Regex.Match(currentMath.Value, @"(\d)+", RegexOptions.IgnoreCase);
                if (currentCoefficent.Success)
                {
                    coefficent *= Convert.ToDouble(currentCoefficent.Value);
                }
            }
#if DEBUG
            Console.WriteLine(coefficent);
#endif
            return coefficent;
        }

        //this pattern (X-2) (X-3) = 0
        private static string ProcessBrackets(string expression)
        {
            string currentExpression = expression;

            #region match  (X-2) (X-3) = 0
            #endregion
            if (Regex.IsMatch(expression, @"((\s)*\((.*(([0-9]*)x).*)+\)(\s)*){2}=(\s)*0", RegexOptions.IgnoreCase))
            {
            }

            #region match  2(X-1) + 8 = = 0            Match matchA = Regex.Match(expression, @"((\d)*(\s)*\(((\d*)x)+(\s)*(-|\+)(\s)*\d*\)+)", RegexOptions.IgnoreCase);
            if (matchA.Success)
            {
                string temp = matchA.Value;
                Console.WriteLine(temp);

                double coefficent = MatchCoefficent(temp);

                //????????????????????????????????????????????? (x-1)
                Match matchInnerBracketExpression = Regex.Match(temp, @"(\(((\d*)x)+(\s)*(-|\+)(\s)*\d*\))", RegexOptions.IgnoreCase);
                if (matchInnerBracketExpression.Success)
                {
                    Console.WriteLine(matchInnerBracketExpression.Value);
                }
            }
            matchA = matchA.NextMatch();
            while (matchA.Success)
            {
                // same as up
                string temp = matchA.Value;
                Console.WriteLine(temp);

                MatchCoefficent(temp);

                matchA = matchA.NextMatch();
            }
            #endregion

            #region match  4(4X) + 2 (X) = 72
            Match match = Regex.Match(expression, @"((\d)*(\s)*\(((\d*)x)+\))", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string temp = match.Value;
                Console.WriteLine(temp);

                double coefficent = 1;
                Match currentMath = Regex.Match(temp, @"(\d)+", RegexOptions.IgnoreCase);
                if (currentMath.Success)
                {
                    Console.WriteLine(currentMath.Value);
                    coefficent *= Convert.ToDouble(currentMath.Value);
                }
                currentMath = currentMath.NextMatch();
                while (currentMath.Success)
                {
                    Console.WriteLine(currentMath.Value);
                    coefficent *= Convert.ToDouble(currentMath.Value);
                    currentMath = currentMath.NextMatch();
                }
                Console.WriteLine("current coefficent is: {0}", coefficent);
                currentExpression = currentExpression.Replace(temp, coefficent.ToString() + "x");
                Console.WriteLine("currentExpression change to: {0} ", currentExpression);

            }

            match = match.NextMatch();
            while (match.Success)
            {
                string temp = match.Value;
                Console.WriteLine(temp);

                double coefficent = 1;
                Match currentMath = Regex.Match(temp, @"(\d)+", RegexOptions.IgnoreCase);
                if (currentMath.Success)
                {
                    Console.WriteLine(currentMath.Value);
                    coefficent *= Convert.ToDouble(currentMath.Value);
                }
                currentMath = currentMath.NextMatch();
                while (currentMath.Success)
                {
                    Console.WriteLine(currentMath.Value);
                    coefficent *= Convert.ToDouble(currentMath.Value);
                    currentMath = currentMath.NextMatch();
                }
                Console.WriteLine("current coefficent is: {0}", coefficent);
                currentExpression = currentExpression.Replace(temp, coefficent.ToString() + "x");
                Console.WriteLine("currentExpression change to: {0} ", currentExpression);

                match = match.NextMatch();

            }
            #endregion




            return currentExpression;

        }
         
        // Change A*X => AX; change 5 * 2 =>10;      AX + b
        public static List<string> StandardizeExpression(List<string> expressions)
        {
            //string standardExpression = "";
            for (int i = 0; i < expressions.Count; i++)
            {
                if (!expressions[i].ToLower().Contains("x^2"))
                {
                    if (expressions[i].ToLower().Contains("x"))
                    {
                        if (expressions[i].Contains("*"))
                        {
                            expressions[i] = expressions[i].Replace("*", "");
                        }
                    }
                    else  // convert number
                    {
                        // Parse 6 * 2 => 12
                        if (expressions[i].Contains("*") && !expressions[i + 1].ToLower().Contains("x"))
                        {
                            double newValue = Convert.ToDouble(expressions[i - 1]) * Convert.ToDouble(expressions[i + 1]);
                            expressions[i] = expressions[i].Replace("*", newValue.ToString());
                            expressions[i - 1] = expressions[i - 1].Replace(expressions[i - 1], "");
                            expressions[i + 1] = expressions[i + 1].Replace(expressions[i + 1], "");

                        }
                        //Parse patern 7 * x => 7x "" ""
                        else if (expressions[i].Contains("*") && expressions[i + 1].ToLower().Contains("x"))
                        {
                            expressions[i - 1] = expressions[i - 1] + "x";
                            expressions[i] = expressions[i].Replace("*", "");
                            expressions[i+ 1] = expressions[i + 1].Replace(expressions[i + 1], "");
                        }
                        //Parse patern 5(2) + 5X = 15
                        else if (expressions[i].Contains("(") && expressions[i].Contains(")"))
                        {
                            int index = expressions[i].IndexOf('(');
                            double beforeNumber = Convert.ToDouble(expressions[i].Substring(index - 1, 1));
                            double behindNumber = Convert.ToDouble(expressions[i].Substring(index + 1, 1));
                            expressions[i] = (beforeNumber * behindNumber).ToString();
                        }
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


            //get rid of "" in expressions
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

        public static List<string> StandardizeQuatraticExpression(List<string> expressions)
        {
            for (int i = 0; i < expressions.Count; i++)
            {
                if (!expressions[i].ToLower().Contains("x^2"))
                {
                    if (expressions[i].Contains("*"))
                    {
                        expressions[i] = expressions[i].Replace("*", "");
                    }
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

        private static double[] CaculateQuatratic(List<string> expressionArray)
        {
            //ax^2 + bx + c = 0
            double[] ABC = new double[3];
            double a = 0;
            double b = 0;
            double c = 0;
            for (int i = 0; i < expressionArray.Count; i++)
            {
                // Caculate a
                if(expressionArray[i].ToLower().Contains("x^2"))
                {
                    double ia = 0;
                    int index = expressionArray[i].IndexOf("x^2");
                    if(index == 0 )
                    {
                        ia = 1;
                    }
                    else
                    {
                        ia = Convert.ToDouble(expressionArray[i].Substring(0, index));
                    }
                    
                    if((i >= 1) && expressionArray[i-1].Contains("-"))
                    {
                        a -= ia;
                    }
                    else
                    {
                        a += ia;
                    }
                    
                }
                // Calc b, parse b1X + b2X - b3X ; b = (b1 + b2 - b3)
                if (expressionArray[i].ToLower().Contains("x") && !expressionArray[i].ToLower().Contains("x^2"))
                {
                    double ib = 0;
                    int index = expressionArray[i].IndexOf("x");
                    if (index == 0)
                    {
                        ib = 1;
                    }
                    else
                    {
                        ib = Convert.ToDouble(expressionArray[i].Substring(0, index));
                    }

                    if ((i >= 1) && expressionArray[i - 1].Contains("-"))
                    {
                        b -= ib;
                    }
                    else
                    {
                        b += ib;
                    }

                }
                //Caculate C
                if (!expressionArray[i].ToLower().Contains("x") &&
                    !expressionArray[i].Contains("+") &&
                    !expressionArray[i].Contains("-") &&
                    !expressionArray[i].Contains("*") &&
                    !expressionArray[i].Contains("/") &&
                    !expressionArray[i].Contains("%") &&
                    !expressionArray[i].Contains("^"))
                {
                    double ic = Convert.ToDouble(expressionArray[i]);
                    if (i == 0)
                    {
                        c += ic;
                    }
                    else
                    {
                        string opera = expressionArray[i - 1];
                        if (opera == "-")
                        {
                            c -= ic;
                        }
                        else if (opera == "+")
                        {
                            c += ic;
                        }
                    }
                }


            }
            ABC[0] = a;
            ABC[1] = b;
            ABC[2] = c;
            return ABC;

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
                    if (ax.Contains("*")) //This will be deleted later, because it has been process in standarlized function
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
                expression = ProcessBrackets(expression);


                string linearOrQuatratic = LinearOrQuatratic(expression);
                // split expression by =
                string[] expressions = expression.Split('=');

                // Left
                string[] leftExpressions = expressions[0].Trim().Split(' ');
                List<string> leftExpressionsList = new List<string>(leftExpressions);
                List<string> newLeftExpressions = StandardizeExpression(leftExpressionsList);

                //Right
                string[] rightExpressions = expressions[1].Trim().Split(' ');
                List<string> rightExpressionsList = new List<string>(rightExpressions);
                List<string> newRightExpressions = StandardizeExpression(rightExpressionsList);

                if (linearOrQuatratic == "linear")
                {
                    double[] value1 = Caculate(newLeftExpressions);
                    double[] value2 = Caculate(newRightExpressions);

                    // X value
                    double A = value1[0] - value2[0];
                    double B = value1[1] - value2[1];
                    if (A == 0)
                    {
                        Console.WriteLine("Denominator cannot be 0");
                    }
                    Console.WriteLine("x =  {0}", -B / A);
                }
                else //ax ^ 2 + bx + c = 0
                {
                    double[] value1 = CaculateQuatratic(newLeftExpressions);
                    double[] value2 = CaculateQuatratic(newRightExpressions);

                    // X value
                    double A = value1[0] - value2[0];
                    double B = value1[1] - value2[1];
                    double C = value1[2] - value2[2];
                    if (A == 0)
                    {
                        Console.WriteLine("Denominator cannot be 0");
                    }

                    double d = B * B - (4 * A * C);
                    double x1 = ((B * -1) + Math.Sqrt(d)) / (2 * A);
                    double x2 = ((B * -1) - Math.Sqrt(d)) / (2 * A);
                    Console.WriteLine("x1 =  {0}", x1);
                    Console.WriteLine("x2 =  {0}", x2);

                }

                  
                

            }


            /*
             ax^2 + bx + c = 0
             int d = b * b - ( 4 * a * c)
             int x1 = ((b * -1) + sqrt(d)) / (2 * a);
             int x2 = ((b * -1) - sqrt(d)) /  (2 * a);
             */


        }
    }
}
