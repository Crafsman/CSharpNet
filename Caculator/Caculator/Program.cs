using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Caculator
{
    class MainClass
    {
        static char variable;
        //Validate expression and remove "cal "
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

                if (index == 0)
                {
                    expression = expression.Remove(0, 5);
                    break;
                }

            }

            foreach (var character in expression)
            {
                if (Char.IsLetter(character))
                {
                    variable = character;
                    expression = expression.Replace(character, 'x');
                }
            }

            return expression;
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

                string replaceString = "";
                double coefficent = MatchCoefficent(temp);
                double xEfficent = 1;
                double number = 0;

                //????????????????????????????????????????????? (x-1)
                Match matchInnerBracketExpression = Regex.Match(temp, @"((\d*)x)+(\s)*(-|\+)(\s)*\d*", RegexOptions.IgnoreCase);
                if (matchInnerBracketExpression.Success)//x-1
                {               
                    string innerBracketExpression = matchInnerBracketExpression.Value;
                    Console.WriteLine(innerBracketExpression);
                    int index = innerBracketExpression.IndexOf("x", StringComparison.CurrentCultureIgnoreCase);
                    if(index == 0)
                    {
                        xEfficent = 1;
                    }else{
                        xEfficent = Convert.ToDouble(innerBracketExpression.Substring(0, index));

                    }
                    xEfficent *= coefficent;

                    replaceString += xEfficent.ToString() + "x";

                    Match matchNumberExpression = Regex.Match(innerBracketExpression, @"(-|\+)(\s)*\d+", RegexOptions.IgnoreCase);
                    if(matchNumberExpression.Success)
                    {
                        Match matchNumber = Regex.Match(matchNumberExpression.Value, @"\d+", RegexOptions.IgnoreCase);
                        if (matchNumber.Success)
                        {
                            number = Convert.ToDouble(matchNumber.Value);
                             
                            Console.WriteLine(matchNumber.Value);
                        }
                    }
                    number *= coefficent;

                    if(innerBracketExpression.Contains("-"))
                    {
                        replaceString += " - " + number.ToString();
                    }
                    if (innerBracketExpression.Contains("+"))
                    {
                        replaceString += " + " + number.ToString();
                    }

                    Console.WriteLine(replaceString);
                }

                currentExpression = currentExpression.Replace(temp, replaceString);
#if DEBUG
                Console.WriteLine("currentExpression change to: {0} ", currentExpression);
#endif


            }
            matchA = matchA.NextMatch();
            while (matchA.Success)
            {
                // same as up
                string temp = matchA.Value;
                Console.WriteLine(temp);

                string replaceString = "";
                double coefficent = MatchCoefficent(temp);
                double xEfficent = 1;
                double number = 0;

                //????????????????????????????????????????????? (x-1)
                Match matchInnerBracketExpression = Regex.Match(temp, @"((\d*)x)+(\s)*(-|\+)(\s)*\d*", RegexOptions.IgnoreCase);
                if (matchInnerBracketExpression.Success)//x-1
                {
                    string innerBracketExpression = matchInnerBracketExpression.Value;
                    Console.WriteLine(innerBracketExpression);
                    int index = innerBracketExpression.IndexOf("x", StringComparison.CurrentCultureIgnoreCase);
                    if (index == 0)
                    {
                        xEfficent = 1;
                    }
                    else
                    {
                        xEfficent = Convert.ToDouble(innerBracketExpression.Substring(0, index));

                    }
                    xEfficent *= coefficent;

                    replaceString += xEfficent.ToString() + "x";

                    Match matchNumberExpression = Regex.Match(innerBracketExpression, @"(-|\+)(\s)*\d+", RegexOptions.IgnoreCase);
                    if (matchNumberExpression.Success)
                    {
                        Match matchNumber = Regex.Match(matchNumberExpression.Value, @"\d+", RegexOptions.IgnoreCase);
                        if (matchNumber.Success)
                        {
                            number = Convert.ToDouble(matchNumber.Value);

                            Console.WriteLine(matchNumber.Value);
                        }
                    }
                    number *= coefficent;

                    if (innerBracketExpression.Contains("-"))
                    {
                        replaceString += " - " + number.ToString();
                    }
                    if (innerBracketExpression.Contains("+"))
                    {
                        replaceString += " + " + number.ToString();
                    }

                    Console.WriteLine(replaceString);
                }
                currentExpression = currentExpression.Replace(temp, replaceString);
#if DEBUG
                Console.WriteLine("currentExpression change to: {0} ", currentExpression);
#endif
                matchA = matchA.NextMatch();
            }
            #endregion

            #region match  4(4X) + 2 (X) = 72
            Match match = Regex.Match(expression, @"((\d)*(\s)*\(((\d*)x)+\))", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                string temp = match.Value;
                double coefficent = CaculateCoefficent(temp);
                currentExpression = currentExpression.Replace(temp, coefficent.ToString() + "x");

            }

            match = match.NextMatch();
            while (match.Success)
            {
                string temp = match.Value;
                double coefficent = CaculateCoefficent(temp);
                currentExpression = currentExpression.Replace(temp, coefficent.ToString() + "x");

                match = match.NextMatch();

            }
            #endregion

            return currentExpression;

        }

        private static double CaculateCoefficent(string expresion)
        {
            double coefficent = 1;
            Match currentMath = Regex.Match(expresion, @"(\d)+", RegexOptions.IgnoreCase);
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

            return coefficent;
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
                    !expressionArray[i].Contains("%"))
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
                try
                {
                    string expression = ValidateExpression();
                    expression = ProcessBrackets(expression);

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


                    double[] value1 = Caculate(newLeftExpressions);
                    double[] value2 = Caculate(newRightExpressions);

                    // X value
                    double A = value1[0] - value2[0];
                    double B = value1[1] - value2[1];

                    if (Math.Abs(A) < Double.Epsilon)
                    {
                        Console.WriteLine("Denominator cannot be 0");
                        continue;
                    }
                    Console.WriteLine("{0} = {1}", variable, -B / A);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
  


            }


        }
    }
}
