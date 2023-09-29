class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        string[] my_str = Console.ReadLine().Split('=');
        if (my_str.Length > 80)
        {
            Console.WriteLine("Строка превосходит 80 символов");
            return;
        }
        int answer;
        try
        {
            answer = int.Parse(my_str[0].Trim(' '));
            if (answer > Math.Pow(2, 30))
            {
                Console.WriteLine("Требуемое число превосходит 2 в 30 степени");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Требуемое число превосходит 2 в 30 степени");
            return;
        }
        string expression = my_str[1].Trim(' ');
        int count_of_operations = expression.Count(x => x == ' ');
        HashSet<string[]> variants = new HashSet<string[]>();
        List<string> allPermutations = GeneratePermutations(count_of_operations);
        takeRightString(allPermutations, expression, answer);
    }

    static void takeRightString(List<string> variants, string expression, int answer)
    {
        List<int> positions_of_gaps = new List<int>();
        for (int i = 0; i < expression.Length; i++)
        {
            if (expression[i] == ' ')
            {
                positions_of_gaps.Add(i);
            }
        }
        for (int i = 0; i < variants.Count; i++)
        {
            char[] expr_var = expression.ToCharArray();
            for (int j = 0; j < positions_of_gaps.Count; j++)
            {
                expr_var[positions_of_gaps[j]] = variants[i][j];
            }
            string res_string = new string(expr_var);
            if (Calculate(res_string) == answer)
            {
                Console.WriteLine(res_string);
                return;
            }
        }
        Console.WriteLine("-1");
    }

    static int Calculate(string expression)
    {
        int bracket_start;
        string res_in_brackets;
        while (true)
        {
            bracket_start = 0;
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '(')
                {
                    bracket_start = i;
                } else if (expression[i] == ')')
                {
                    res_in_brackets = CalculateSimpleString(expression.Substring(bracket_start + 1, i - bracket_start-1));
                    expression = expression.Substring(0, bracket_start) + res_in_brackets + (i == expression.Length-1 ? "" : expression.Substring(i+1));
                    break;
                }
                if (bracket_start == 0 && i == expression.Length-1)
                {
                    expression = CalculateSimpleString(expression);
                    if (expression.Contains('-')) expression = expression.Substring(1);
                    return int.Parse(expression);
                }
            }
        }
    }

    static string CalculateSimpleString(string str)
    {
        str = str.Trim(' ');
        int counter;
        int right_counter;
        string first_number;
        string second_number;
        string res_of_two_numbers;
        bool is_first_have_unarn_minus;
        char[] operations = new char[] { '+', '-', '*' };
        while ( !(str.Count(x => x == '-') == 1 && str[0] == '-' && str.Count(x => x == '+') == 0 && str.Count(x => x == '*') == 0)
            && (str.Contains('+') || str.Contains('-') || str.Contains('*')) )
        {
            counter = 0;
            first_number = "";
            second_number = "";
            res_of_two_numbers = "";
            is_first_have_unarn_minus = false;
            if (str[0] == '-')
            {
                is_first_have_unarn_minus = true;
                str = str.Substring(1);
            }
            for (int i = 0; i < str.Length; i++)
            {
                if (!(i == 0 && str[0] == '-') && operations.Contains(str[i]))
                {
                    counter++;
                    for (int j = i+1; j < str.Length; j++) 
                    {
                        if (j == str.Length-1)
                        {
                            second_number = str.Substring(i + 1, str.Length-(i+1));
                            break;
                        }
                        if (operations.Contains(str[j])) break;
                        else
                        {
                            second_number += str[j];
                            counter++;
                        }
                    }
                    switch (str[i])
                    {
                        case '+':
                            res_of_two_numbers = ((is_first_have_unarn_minus ? int.Parse(first_number) * (-1) : int.Parse(first_number)) + int.Parse(second_number)).ToString();
                            break;
                        case '-':
                            res_of_two_numbers = ((is_first_have_unarn_minus ? int.Parse(first_number) * (-1) : int.Parse(first_number)) - int.Parse(second_number)).ToString();
                            break;
                        case '*':
                            res_of_two_numbers = ((is_first_have_unarn_minus ? int.Parse(first_number) * (-1) : int.Parse(first_number)) * int.Parse(second_number)).ToString();
                            break;
                    }
                    str = res_of_two_numbers + str.Substring(counter, str.Length-counter);
                    break;
                } else
                {
                    first_number += str[i];
                    counter++;
                }
            }
        }
        if(str[0] == '-')
        {
            return "0-" + str.Substring(1, str.Length-2);
        }
        return str.Substring(0, str.Length-1);
    }

    static List<string> GeneratePermutations(int length)
    {
        List<string> permutations = new List<string>();
        char[] operators = { '+', '-', '*' };
        char[] currentPermutation = new char[length];

        GeneratePermutationsRecursively(permutations, operators, currentPermutation, length, 0);

        return permutations;
    }

    static void GeneratePermutationsRecursively(List<string> permutations, char[] operators, char[] currentPermutation, int length, int index)
    {
        if (index == length)
        {
            permutations.Add(new string(currentPermutation));
            return;
        }

        foreach (char op in operators)
        {
            currentPermutation[index] = op;
            GeneratePermutationsRecursively(permutations, operators, currentPermutation, length, index + 1);
        }
    }
}