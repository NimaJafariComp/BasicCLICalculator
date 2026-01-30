// ui helpers
using System;
using System.Globalization;

namespace BasicCalculatorPart1
{
    internal static class UI
    {
        // mode selection menu
        public static void ModeSelectionMenu()
        {
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║       Select Calculator Mode:                ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  [1] No Memory Mode                          ║");
            Console.WriteLine("║      (resets after each operation)           ║");
            Console.WriteLine("║                                              ║");
            Console.WriteLine("║  [2] Memory Mode                             ║");
            Console.WriteLine("║      (continuous operations with memory)     ║");
            Console.WriteLine("║                                              ║");
            Console.WriteLine("║  [0] Exit (q / quit)                         ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        // app header
        public static void Header()
        {
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║      Project 1 - Basic Calculator            ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
        }

        // menu for no-memory mode
        public static void Menu()
        {
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║  Type number OR symbol/keyword:              ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  [1] Add                    (+)              ║");
            Console.WriteLine("║  [2] Subtract               (-)              ║");
            Console.WriteLine("║  [3] Multiply               (* / x)          ║");
            Console.WriteLine("║  [4] Divide                 (/)              ║");
            Console.WriteLine("║  [5] Power                  (^ / pow)        ║");
            Console.WriteLine("║  [6] Square Root            (sqrt / √)       ║");
            Console.WriteLine("║  [7] Reciprocal (1/x)       (1/x / inv)      ║");
            Console.WriteLine("║  [8] Percent (%)            (% / percent)    ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  [0] Back to Mode Menu      (q / quit)       ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        // menu for memory mode
        public static void MemoryMenu(decimal currentMemory)
        {
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine($"║  Current Memory: {Format(currentMemory),-28} ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  Memory Mode (operations use/update memory)  ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  [1] Add                    (+)              ║");
            Console.WriteLine("║  [2] Subtract               (-)              ║");
            Console.WriteLine("║  [3] Multiply               (* / x)          ║");
            Console.WriteLine("║  [4] Divide                 (/)              ║");
            Console.WriteLine("║  [5] Power                  (^ / pow)        ║");
            Console.WriteLine("║  [6] Square Root            (sqrt / √)       ║");
            Console.WriteLine("║  [7] Reciprocal (1/x)       (1/x / inv)      ║");
            Console.WriteLine("║  [8] Percent (%)            (% / percent)    ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  [9] Set Value              (set)            ║");
            Console.WriteLine("║  [C] Clear/Reset            (c / clear)      ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  [0] Back to Mode Menu      (q / quit)       ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        // trim trailing zeros
        public static string Format(decimal value)
        {
            string s = value.ToString(CultureInfo.InvariantCulture);

            if (s.Contains('.'))
            {
                s = s.TrimEnd('0');
                if (s.EndsWith(".")) s = s.TrimEnd('.');
            }

            return s;
        }

        // show info/success/error messages in one place
        public static void Info(string message) => Console.WriteLine($"[INFO]  {message}");
        public static void Success(string message) => Console.WriteLine($"[OK]    {message}");
        public static void Error(string message) => Console.WriteLine($"[ERROR] {message}");


        public static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
