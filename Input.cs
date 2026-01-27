// input helpers
using System;
using System.Globalization;

namespace BasicCalculatorPart1
{
    public static class Input
    {
        // read a decimal from the user, keeps asking until it gets a good value
        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                string raw = ReadNonEmpty(prompt).Trim();

                if (TryValidateAndParse(raw, out decimal value))
                {
                    return value;
                }
            }
        }

        // validate raw text and try to parse it to decimal
        private static bool TryValidateAndParse(string raw, out decimal value)
        {
            value = 0m;

            foreach (char ch in raw)
            {
                if (char.IsLetter(ch))
                {
                    UI.Error("Invalid input: alphabet letters are not allowed in numbers.");
                    return false;
                }
            }

            if (!IsValidNumericToken(raw))
            {
                UI.Error("Invalid number format. Use digits with optional leading '-' and a single '.' (example: -12.34).");
                return false;
            }

            if (!decimal.TryParse(raw, NumberStyles.Number, CultureInfo.InvariantCulture, out value))
            {
                UI.Error("Invalid number: could not parse. Try again.");
                return false;
            }

            return true;
        }

        // read an operator char (like + - * /). enforces single-character input
        public static char ReadOperator(string prompt, char[] allowed)
        {
            while (true)
            {
                string raw = ReadNonEmpty(prompt).Trim();
                if (raw.Length != 1)
                {
                    UI.Error("Enter exactly one operator character.");
                    continue;
                }

                char c = raw[0];
                foreach (char a in allowed)
                {
                    if (c == a) return c;
                }

                UI.Error($"Invalid operator '{c}'. Allowed: {string.Join(" ", allowed)}");
            }
        }

        // prompt and return the typed string, never empty
        public static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? s = Console.ReadLine();

                if (s is null)
                {
                    UI.Error("Input error: no input received.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(s))
                {
                    UI.Error("Input cannot be empty.");
                    continue;
                }

                return s;
            }
        }

        // check the token for digits, one optional dot, and optional leading '-'
        private static bool IsValidNumericToken(string s)
        {
            int dotCount = 0;
            int minusCount = 0;

            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];

                if (c == '.')
                {
                    dotCount++;
                    if (dotCount > 1) return false;
                    continue;
                }

                if (c == '-')
                {
                    minusCount++;
                    if (minusCount > 1) return false;
                    if (i != 0) return false;
                    continue;
                }

                if (char.IsDigit(c))
                    continue;

                return false;
            }

            if (s == "-" || s == "." || s == "-.") return false;

            return true;
        }
    }
}
