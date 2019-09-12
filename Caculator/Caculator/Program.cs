using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/*
steps:
1. validate expressions
2. splict the equation by '='
3. handle left expression, get the left coefficients
4. handle right expression, get the right coefficients
5. put the right coefficients to left
6. use formula x = -b / a         (ax + b = 0)
*/
namespace Caculator
{
    class MainClass
    {
        static char globalVariable;
        static string[] operators = new string[]{"+", "-", "*", "/"};

        public static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Please input linear equation:");

                try
                {
                    string expression = ValidateExpression();
                    expression = RemoveBrackets(expression);

                    // split expression by =
                    string[] expressions = expression.Split('=');

                    // Left expression
                    string[] leftExpressions = expressions[0].Trim().Split(' ');
                    List<string> leftExpressionsList = new List<string>(leftExpressions);
                    List<string> standardLeftExpression = StandardizeExpression(leftExpressionsList);

                    // Right expression
                    string[] rightExpressions = expressions[1].Trim().Split(' ');
                    List<string> rightExpressionsList = new List<string>(rightExpressions);
                    List<string> standardRightExpression = StandardizeExpression(rightExpressionsList);

                    double[] leftCoefficients = CaculateStandardExpressionCoefficent(standardLeftExpression);
                    double[] rightCoefficients = CaculateStandardExpressionCoefficent(standardRightExpression);

                    double aCoefficients = leftCoefficients[0] - rightCoefficients[0];
                    double bCoefficients = leftCoefficients[1] - rightCoefficients[1];

                    if (Math.Abs(aCoefficients) < Double.Epsilon)
                    {
                        throw new System.DivideByZeroException();
                    }

                    Console.WriteLine("{0} = {1}", globalVariable, (-bCoefficients / aCoefficients));

                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Argument Out Of Range, please input again: ");
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Index out of range, please input again: ");
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("Denominator cannot be 0, please input again: ");
                }
                catch (FormatException)
                {
                    Console.WriteLine("You shuold use \"withe space\" to split operators and numbers. \n For example: calc X + 2 = 6 ");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

        }
        //Validate expression input pattern, and remove "cal "
        public static string ValidateExpression()
        {
          
            string expression = "";
            while (true)
            {
                expression = Console.ReadLine();

                if (Regex.IsMatch(expression, @"^calc "))
                {
                    expression = expression.Remove(0, 5);
                }
                else
                {
                    Console.WriteLine("Please begin with \"calc \"");
                    continue;
                }
                // Check if there is x variable
                if(!Regex.IsMatch(expression, @"[A-Za-z]+"))
                {
                    Console.WriteLine("Please input a valid variable, please input again: ");
                    continue;
                }
                // no '='
                if (!Regex.IsMatch(expression, @"="))
                {
                    Console.WriteLine("not a equation, please input again: ");
                    continue;
                }
                break;
            }

            // Change any variable to x, for example 'z + 4 = 0' => 'x + 4 = 0' 
            foreach (var character in expression)
            {
                if (Char.IsLetter(character))
                {
                    globalVariable = character;
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
            return coefficent;
        }

       
        private static string RemoveBrackets(string expression)
        {
            string currentExpression = expression;

            #region match  2(X-1) + 8 = 0            Match matchA = Regex.Match(expression, @"((\d)*(\s)*\(((\d*)x)+(\s)*(-|\+)(\s)*\d*\)+)", RegexOptions.IgnoreCase);
            if (matchA.Success)
            {
                string temp = matchA.Value;
                string replaceString = "";
                double coefficent = MatchCoefficent(temp);
                double xEfficent = 1;
                double number = 0;

                //????????????????????????????????????????????? (x-1)
                Match matchInnerBracketExpression = Regex.Match(temp, @"((\d*)x)+(\s)*(-|\+)(\s)*\d*", RegexOptions.IgnoreCase);
                if (matchInnerBracketExpression.Success)//x-1
                {               
                    string innerBracketExpression = matchInnerBracketExpression.Value;
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
                }

                currentExpression = currentExpression.Replace(temp, replaceString);

            }

            matchA = matchA.NextMatch();
            while (matchA.Success)
            {
                // same as above
                string temp = matchA.Value;

                string replaceString = "";
                double coefficent = MatchCoefficent(temp);
                double xEfficent = 1;
                double number = 0;

                Match matchInnerBracketExpression = Regex.Match(temp, @"((\d*)x)+(\s)*(-|\+)(\s)*\d*", RegexOptions.IgnoreCase);
                if (matchInnerBracketExpression.Success)//x-1
                {
                    string innerBracketExpression = matchInnerBracketExpression.Value;
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
               
                }
                currentExpression = currentExpression.Replace(temp, replaceString);
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
                coefficent *= Convert.ToDouble(currentMath.Value);
            }
            currentMath = currentMath.NextMatch();
            while (currentMath.Success)
            {
                coefficent *= Convert.ToDouble(currentMath.Value);
                currentMath = currentMath.NextMatch();
            }

            return coefficent;
        }

        public static bool IsOperator(string expression)
        {
            foreach(var operate in operators)
            {
                if (expression.Equals(operate))
                    return true;
            }
            return false;
        }

        // Recursive function. Resolving multiple concurrent operators. e.g.X = 3 - - - 8X - 4
        public static void ResolveMultipleConcurrentOperators(ref List<string> expressions)
        {
            for (int i = 0; i < expressions.Count; i++)
            {
                if (IsOperator(expressions[i]) && IsOperator(expressions[i + 1]))
                {
                    expressions.Remove(expressions[i]);
                    ResolveMultipleConcurrentOperators(ref expressions);
                }
            }
        }

        // Remove '*' symble, for example: Change "A*X" to "AX" and calc "5 * 2" to 10
        public static List<string> StandardizeExpression(List<string> expressions)
        {
            ResolveMultipleConcurrentOperators(ref expressions);

            // Handle '*'
            for (int i = 0; i < expressions.Count; i++)
            {
                //Change "A*X" to "AX"
                if (expressions[i].ToLower().Contains("x"))
                {
                    if (expressions[i].Contains("*"))
                    {
                        expressions[i] = expressions[i].Replace("*", "");
                    }
                }
                else  // convert "5 * 2" to 10
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

            // handle '/'
            for (int i = 0; i < expressions.Count; i++)
            {
                if (expressions[i].Contains("/"))
                {
                    // 1. there is 'x' before '/', such as  9x / 3 = 9
                    if(expressions[i - 1].ToLower().Contains("x"))
                    {
                        double coefficient = withdrawCoefficent(expressions[i - 1]);
                        double denominator = Convert.ToDouble(expressions[i + 1]);

                        if (Math.Abs(denominator) < Double.Epsilon)
                        {
                            throw new System.DivideByZeroException();
                        }
                        double newCoefficient = coefficient / denominator;
                        expressions[i] = expressions[i].Replace(expressions[i], "");
                        expressions[i + 1] = expressions[i + 1].Replace(expressions[i + 1], "");
                        expressions[i - 1] = newCoefficient.ToString() + "x";
                    }// 2. no 'x' before '/', such as 6 / 3 ; 10 marks
                    else if(Regex.IsMatch(expressions[i - 1], @"^\d+$") && Regex.IsMatch(expressions[i + 1], @"^\d+$"))
                    {                  
                        double newValue = Convert.ToDouble(expressions[i - 1]) / Convert.ToDouble(expressions[i + 1]);
                        expressions[i] = expressions[i].Replace(expressions[i], "");
                        expressions[i + 1] = expressions[i + 1].Replace(expressions[i + 1], "");
                        expressions[i - 1] = newValue.ToString();

                    }
                }
            }

            //get rid of "" in expressions
            List<string> outputStandardExpression = new List<string>();
            for (int j = 0; j < expressions.Count; j++)
            {
                if (!String.IsNullOrEmpty(expressions[j]))
                {
                    outputStandardExpression.Add(expressions[j]);
                }
            }

            return outputStandardExpression;
        }

        private static double withdrawCoefficent(string expression)
        {
            double coefficient = 1;
            if(expression.ToLower().Contains("x"))
            {
                if (expression.Length == 1)
                {
                    coefficient = 1;
                }
                else
                {
                    string aCoefficient = expression.Substring(0, expression.Length - 1);
                    coefficient = Convert.ToDouble(aCoefficient);
                }
            }
            return coefficient;
        }

        private static double[] CaculateStandardExpressionCoefficent(List<string> expressionArray)
        {
            // caculate Ax + Bx + C
            double[] AB = new double[2];
            double A = 0;
            double B = 0;
            for (int i = 0; i < expressionArray.Count; i++)
            {
                //Caculate C
                if (!expressionArray[i].ToLower().Contains("x") && !IsOperator(expressionArray[i]))
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
                            }else if(opera == "+")
                            {
                                A += 1;
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
            AB[0] = A;
            AB[1] = B; 
            return AB;
        }




    }
}
