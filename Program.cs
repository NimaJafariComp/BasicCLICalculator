// program, main loop, command handling
using System;

namespace BasicCalculatorPart1
{
    internal static class Program
    {
        // show menu, read choice, handle commands
        private static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                UI.Header();
                UI.Menu();

                string choice = Input.ReadNonEmpty("Choose an option (number or symbol): ").Trim().ToLowerInvariant();
                Console.WriteLine();

                if (choice is "0" or "q" or "quit" or "exit")
                    break;

                try
                {
                    if (!HandleChoice(choice))
                    {
                        UI.Error($"Unknown option: \"{choice}\". Please choose a listed option.");
                        UI.Pause();
                    }
                }
                catch (CalculatorException ex)
                {
                    UI.Error(ex.Message);
                    UI.Pause();
                }
                catch (OverflowException)
                {
                    UI.Error("Overflow: number too large for this calculation.");
                    UI.Pause();
                }
                catch (Exception ex)
                {
                    UI.Error($"Unexpected error: {ex.Message}");
                    UI.Pause();
                }
            }

            Console.WriteLine("Goodbye!");
        }

        // interpret choice and call the handler
        private static bool HandleChoice(string choice)
        {
            if (choice is "1" or "+") return DoBinary("+");
            if (choice is "2" or "-" or "−") return DoBinary("-");
            if (choice is "3" or "*" or "x" or "×") return DoBinary("*");
            if (choice is "4" or "/" or "÷") return DoBinary("/");
            if (choice is "5" or "^" or "pow" or "power") return DoPower();

            if (choice is "6" or "sqrt" or "√") return DoSqrt();
            if (choice is "7" or "1/x" or "inv" or "recip") return DoReciprocal();

            if (choice is "8" or "%" or "percent") return DoPercentMenu();

            if (choice is "h" or "help" or "?") return true;

            return false;
        }

        // take two numbers and apply the chosen operator
        private static bool DoBinary(string op)
        {
            decimal a = Input.ReadDecimal("Enter the first number: ");
            decimal b = Input.ReadDecimal("Enter the second number: ");

            decimal result = op switch
            {
                "+" => a + b,
                "-" => a - b,
                "*" => a * b,
                "/" => b == 0m ? throw new CalculatorException("Math error: division by zero is not allowed.") : a / b,
                _ => throw new CalculatorException("Unknown operation.")
            };

            UI.Success($"Result = {UI.Format(result)}");
            UI.Pause();
            return true;
        }

        // power: enter base and exponent
        private static bool DoPower()
        {
            decimal a = Input.ReadDecimal("Enter the base (x): ");
            decimal b = Input.ReadDecimal("Enter the exponent (y): ");

            decimal result = MathOps.PowerDecimal(a, b);
            UI.Success($"Result = {UI.Format(result)}");
            UI.Pause();
            return true;
        }

        // sqrt: compute the square root
        private static bool DoSqrt()
        {
            decimal x = Input.ReadDecimal("Enter the number (x): ");
            decimal result = MathOps.SqrtDecimal(x);

            UI.Success($"Result = {UI.Format(result)}");
            UI.Pause();
            return true;
        }

        // reciprocal: compute 1/x
        private static bool DoReciprocal()
        {
            decimal x = Input.ReadDecimal("Enter the number (x): ");
            if (x == 0m) throw new CalculatorException("Math error: 1/x is not allowed when x = 0.");

            decimal result = 1m / x;
            UI.Success($"Result = {UI.Format(result)}");
            UI.Pause();
            return true;
        }

        // percent: opens menu to choose between a op b% or a% of b
        private static bool DoPercentMenu()
        {
            Console.WriteLine("Percent Options:");
            Console.WriteLine("  [1] A op B%   (like a standard calculator: 200 + 10% = 220)");
            Console.WriteLine("  [2] A% of B   (e.g., 10% of 200 = 20)");
            Console.WriteLine();

            string sub = Input.ReadNonEmpty("Choose 1 or 2: ").Trim();

            if (sub == "1")
            {
                decimal a = Input.ReadDecimal("Enter A (base value): ");
                char op = Input.ReadOperator("Choose operator (+ - * /): ", new[] { '+', '-', '*', '/' });
                decimal bPercent = Input.ReadDecimal("Enter B (percent value, e.g., 10 for 10%): ");

                decimal result = MathOps.PercentAOpB(a, op, bPercent);
                UI.Success($"Result = {UI.Format(result)}");
                UI.Pause();
                return true;
            }

            if (sub == "2")
            {
                decimal aPercent = Input.ReadDecimal("Enter A (percent value, e.g., 10 for 10%): ");
                decimal b = Input.ReadDecimal("Enter B (whole value): ");

                decimal result = (aPercent / 100m) * b;
                UI.Success($"Result = {UI.Format(result)}");
                UI.Pause();
                return true;
            }

            UI.Error("Invalid percent option. Choose 1 or 2.");
            UI.Pause();
            return true;
        }
    }
}
