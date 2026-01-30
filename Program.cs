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
                UI.ModeSelectionMenu();

                string modeChoice = Input.ReadNonEmpty("Choose mode (1 or 2): ").Trim().ToLowerInvariant();
                Console.WriteLine();

                if (modeChoice is "0" or "q" or "quit" or "exit")
                    break;

                if (modeChoice == "1")
                {
                    RunNoMemoryMode();
                }
                else if (modeChoice == "2")
                {
                    RunMemoryMode();
                }
                else
                {
                    UI.Error($"Invalid option: \"{modeChoice}\". Please choose 1 or 2.");
                    UI.Pause();
                }
            }

            Console.WriteLine("Goodbye!");
        }

        // Original mode: no memory, resets after every operation
        private static void RunNoMemoryMode()
        {
            while (true)
            {
                UI.Header();
                UI.Menu();

                string choice = Input.ReadNonEmpty("Choose an option (number or symbol): ").Trim().ToLowerInvariant();
                Console.WriteLine();

                if (choice is "0" or "q" or "quit" or "exit" or "back")
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
        }

        // memory mode with continuous operations
        private static void RunMemoryMode()
        {
            decimal memory = 0m;

            while (true)
            {
                UI.Header();
                UI.MemoryMenu(memory);

                string choice = Input.ReadNonEmpty("Choose an option (number or symbol): ").Trim().ToLowerInvariant();
                Console.WriteLine();

                if (choice is "0" or "q" or "quit" or "exit" or "back")
                    break;

                try
                {
                    if (!HandleMemoryChoice(choice, ref memory))
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

        // choice for memory mode
        private static bool HandleMemoryChoice(string choice, ref decimal memory)
        {
            if (choice is "1" or "+") return DoMemoryBinary("+", ref memory);
            if (choice is "2" or "-" or "−") return DoMemoryBinary("-", ref memory);
            if (choice is "3" or "*" or "x" or "×") return DoMemoryBinary("*", ref memory);
            if (choice is "4" or "/" or "÷") return DoMemoryBinary("/", ref memory);
            if (choice is "5" or "^" or "pow" or "power") return DoMemoryPower(ref memory);

            if (choice is "6" or "sqrt" or "√") return DoMemorySqrt(ref memory);
            if (choice is "7" or "1/x" or "inv" or "recip") return DoMemoryReciprocal(ref memory);

            if (choice is "8" or "%" or "percent") return DoMemoryPercentMenu(ref memory);

            if (choice is "9" or "set") return DoMemorySet(ref memory);
            if (choice is "c" or "clear" or "reset") return DoMemoryClear(ref memory);

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


        // binary ops in memory mode
        private static bool DoMemoryBinary(string op, ref decimal memory)
        {
            decimal b = Input.ReadDecimal("Enter number: ");

            decimal result = op switch
            {
                "+" => memory + b,
                "-" => memory - b,
                "*" => memory * b,
                "/" => b == 0m ? throw new CalculatorException("Math error: division by zero is not allowed.") : memory / b,
                _ => throw new CalculatorException("Unknown operation.")
            };

            memory = result;
            UI.Success($"Result = {UI.Format(result)}");
            UI.Info($"Memory updated to: {UI.Format(memory)}");
            UI.Pause();
            return true;
        }

        // power in memory mode
        private static bool DoMemoryPower(ref decimal memory)
        {
            decimal exponent = Input.ReadDecimal("Enter the exponent (y): ");

            decimal result = MathOps.PowerDecimal(memory, exponent);
            memory = result;
            UI.Success($"Result = {UI.Format(result)}");
            UI.Info($"Memory updated to: {UI.Format(memory)}");
            UI.Pause();
            return true;
        }

        // square root in memory mode
        private static bool DoMemorySqrt(ref decimal memory)
        {
            decimal result = MathOps.SqrtDecimal(memory);
            memory = result;
            UI.Success($"Result = {UI.Format(result)}");
            UI.Info($"Memory updated to: {UI.Format(memory)}");
            UI.Pause();
            return true;
        }

        // reciprocal in memory mode
        private static bool DoMemoryReciprocal(ref decimal memory)
        {
            if (memory == 0m) throw new CalculatorException("Math error: 1/x is not allowed when x = 0.");

            decimal result = 1m / memory;
            memory = result;
            UI.Success($"Result = {UI.Format(result)}");
            UI.Info($"Memory updated to: {UI.Format(memory)}");
            UI.Pause();
            return true;
        }

        // percent in memory mode
        private static bool DoMemoryPercentMenu(ref decimal memory)
        {
            Console.WriteLine("Percent Options:");
            Console.WriteLine("  [1] Memory op B%   (like a standard calculator: memory + 10% = memory + 10% of memory)");
            Console.WriteLine("  [2] Memory% of B   (e.g., if memory=10, then 10% of 200 = 20)");
            Console.WriteLine();

            string sub = Input.ReadNonEmpty("Choose 1 or 2: ").Trim();

            if (sub == "1")
            {
                char op = Input.ReadOperator("Choose operator (+ - * /): ", new[] { '+', '-', '*', '/' });
                decimal bPercent = Input.ReadDecimal("Enter B (percent value, e.g., 10 for 10%): ");

                decimal result = MathOps.PercentAOpB(memory, op, bPercent);
                memory = result;
                UI.Success($"Result = {UI.Format(result)}");
                UI.Info($"Memory updated to: {UI.Format(memory)}");
                UI.Pause();
                return true;
            }

            if (sub == "2")
            {
                decimal b = Input.ReadDecimal("Enter B (whole value): ");

                decimal result = (memory / 100m) * b;
                memory = result;
                UI.Success($"Result = {UI.Format(result)}");
                UI.Info($"Memory updated to: {UI.Format(memory)}");
                UI.Pause();
                return true;
            }

            UI.Error("Invalid percent option. Choose 1 or 2.");
            UI.Pause();
            return true;
        }

        // set memory value
        private static bool DoMemorySet(ref decimal memory)
        {
            decimal newValue = Input.ReadDecimal("Enter new memory value: ");
            memory = newValue;
            UI.Success($"Memory set to: {UI.Format(memory)}");
            UI.Pause();
            return true;
        }

        // clear/reset memory
        private static bool DoMemoryClear(ref decimal memory)
        {
            memory = 0m;
            UI.Success("Memory cleared (reset to 0)");
            UI.Pause();
            return true;
        }
    }
}
