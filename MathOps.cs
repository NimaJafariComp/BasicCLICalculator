// math helpers
using System;

namespace BasicCalculatorPart1
{
    public static class MathOps
    {
        // basic operations
        public static decimal Add(decimal a, decimal b) => a + b;
        public static decimal Subtract(decimal a, decimal b) => a - b;
        public static decimal Multiply(decimal a, decimal b) => a * b;

        public static decimal Divide(decimal a, decimal b)
        {
            if (b == 0m) throw new CalculatorException("Math error: division by zero is not allowed.");
            return a / b;
        }

        public static decimal Reciprocal(decimal x)
        {
            if (x == 0m) throw new CalculatorException("Math error: 1/x is not allowed when x = 0.");
            return 1m / x;
        }

        // square root: uses double internally
        public static decimal SqrtDecimal(decimal x)
        {
            if (x < 0m) throw new CalculatorException("Math error: square root of a negative number is not allowed.");

            double xd = (double)x;
            double rd = Math.Sqrt(xd);

            if (double.IsNaN(rd) || double.IsInfinity(rd))
                throw new CalculatorException("Math error: invalid square root result.");

            return (decimal)rd;
        }

        // power: integer exponents use decimal math; fractional exponents fall back to double
        public static decimal PowerDecimal(decimal baseValue, decimal exponent)
        {
            if (baseValue == 0m && exponent == 0m)
                throw new CalculatorException("Math error: 0^0 is undefined.");

            if (IsInteger(exponent))
            {
                int e;
                try
                {
                    e = checked((int)exponent);
                }
                catch
                {
                    throw new CalculatorException("Math error: integer exponent is too large.");
                }

                return PowInt(baseValue, e);
            }

            if (baseValue < 0m)
                throw new CalculatorException("Math error: negative base with non-integer exponent is not supported (would require complex numbers).");

            double b = (double)baseValue;
            double e2 = (double)exponent;
            double r = Math.Pow(b, e2);

            if (double.IsNaN(r) || double.IsInfinity(r))
                throw new CalculatorException("Math error: invalid power result.");

            return (decimal)r;
        }


        // percent: a op b% 
        public static decimal PercentAOpB(decimal a, char op, decimal bPercent)
        {
            decimal bFrac = bPercent / 100m;

            return op switch
            {
                '+' => a + (a * bFrac),
                '-' => a - (a * bFrac),
                '*' => a * bFrac,
                '/' => bFrac == 0m
                    ? throw new CalculatorException("Math error: division by zero (B% resulted in 0).")
                    : a / bFrac,
                _ => throw new CalculatorException("Invalid operator for percent mode.")
            };
        }

        // percent: a% of b
        public static decimal PercentAOfB(decimal aPercent, decimal b)
        {
            return (aPercent / 100m) * b;
        }

        // percent: memory% of b
        public static decimal PercentMemoryOfB(decimal memory, decimal b)
        {
            return (memory / 100m) * b;
        }

        // is value an integer (no fractional part)?
        private static bool IsInteger(decimal value) => value == decimal.Truncate(value);

        // integer power using binary exponentiation (keeps decimal precision)
        private static decimal PowInt(decimal b, int e)
        {
            if (e == 0) return 1m;

            bool negExp = e < 0;
            long exp = negExp ? -(long)e : e;

            decimal result = 1m;
            decimal factor = b;

            while (exp > 0)
            {
                if ((exp & 1) == 1)
                    result *= factor;

                exp >>= 1;
                if (exp > 0)
                    factor *= factor;
            }

            if (!negExp) return result;

            if (result == 0m)
                throw new CalculatorException("Math error: division by zero (negative exponent on 0).");

            return 1m / result;
        }
    }
}
