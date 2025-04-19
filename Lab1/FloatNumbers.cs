using System;
using System.Text;

public class FloatNumbers
{
    private struct FloatNumber
    {
        public int Sign;
        public int[] Exponent;
        public int[] Mantissa;
    }
    public static int[] ToFloatFromDecimal(float number)
    {
        int[] result = new int[32];

        if (number == 0)
            return result;

        FloatNumber bits;
        bits.Sign = number < 0 ? 1 : 0;
        number = Math.Abs(number);

        bits.Exponent = new int[8];
        bits.Mantissa = new int[23];

        int exponent = 0;
        while (number >= 2.0f)
        {
            number /= 2.0f;
            exponent++;
        }
        while (number < 1.0f && number != 0)
        {
            number *= 2.0f;
            exponent--;
        }

        number -= 1.0f;
        for (int i = 0; i < 23 && number > 0; i++)
        {
            number *= 2;
            bits.Mantissa[i] = number >= 1 ? 1 : 0;
            if (number >= 1) number -= 1;
        }

        exponent += 127;
        if (exponent >= 255)
        {
            result[0] = bits.Sign;
            for (int i = 0; i < 8; i++) result[i + 1] = 1;
            return result;
        }
        if (exponent <= 0)
        {
            result[0] = bits.Sign;
            return result;
        }

        for (int i = 7; i >= 0; i--)
        {
            bits.Exponent[i] = exponent & 1;
            exponent >>= 1;
        }

        result[0] = bits.Sign;
        for (int i = 0; i < 8; i++)
            result[i + 1] = bits.Exponent[i];
        for (int i = 0; i < 23; i++)
            result[i + 9] = bits.Mantissa[i];

        return result;
    }

    public static float ToDecimalFromFloat(int[] bits)
    {
        if (bits.Length != 32)
            throw new ArgumentException("Array must contain exactly 32 bits");

        bool isZero = true;
        for (int i = 0; i < 32; i++)
        {
            if (bits[i] != 0)
            {
                isZero = false;
                break;
            }
        }
        if (isZero) return 0;

        int sign = bits[0];
        int exponent = 0;
        for (int i = 0; i < 8; i++)
            exponent = (exponent << 1) | bits[i + 1];

        int[] mantissa = new int[23];
        for (int i = 0; i < 23; i++)
            mantissa[i] = bits[i + 9];

        float result;
        if (exponent == 0xFF)
        {
            bool isNaN = false;
            for (int i = 0; i < 23; i++)
                if (mantissa[i] != 0)
                {
                    isNaN = true;
                    break;
                }
            return isNaN ? float.NaN : (sign == 1 ? float.NegativeInfinity : float.PositiveInfinity);
        }

        if (exponent == 0)
        {
            float mantissaValue = 0;
            for (int i = 0; i < 23; i++)
                mantissaValue += mantissa[i] * (float)Math.Pow(2, -(i + 1));
            result = mantissaValue * (float)Math.Pow(2, -126);
        }
        else
        {
            float mantissaValue = 1;
            for (int i = 0; i < 23; i++)
                mantissaValue += mantissa[i] * (float)Math.Pow(2, -(i + 1));
            result = mantissaValue * (float)Math.Pow(2, exponent - 127);
        }

        return sign == 1 ? -result : result;
    }
    public static int[] FloatSum(int[] firstNumber, int[] secondNumber)
    {
        if (firstNumber.Length != 32 || secondNumber.Length != 32)
            throw new ArgumentException("Arrays must contain exactly 32 bits");

        int[] result = new int[32];

        int signFirst = firstNumber[0];
        int signSecond = secondNumber[0];
        int expFirst = 0, expSecond = 0;
        for (int i = 0; i < 8; i++)
        {
            expFirst = (expFirst << 1) | firstNumber[i + 1];
            expSecond = (expSecond << 1) | secondNumber[i + 1];
        }
        int[] mantFirst = new int[24];
        int[] mantSecond = new int[24];
        for (int i = 0; i < 23; i++)
        {
            mantFirst[i + 1] = firstNumber[i + 9];
            mantSecond[i + 1] = secondNumber[i + 9];
        }
        if (expFirst != 0) mantFirst[0] = 1;
        if (expSecond != 0) mantSecond[0] = 1;

        if (expFirst == 0xFF || expSecond == 0xFF)
        {
            if (expFirst == 0xFF && expSecond == 0xFF)
            {
                bool isNaNFirst = false, isNaNSecond = false;
                for (int i = 0; i < 23; i++)
                {
                    if (firstNumber[i + 9] != 0) isNaNFirst = true;
                    if (secondNumber[i + 9] != 0) isNaNSecond = true;
                }
                if (isNaNFirst || isNaNSecond)
                {
                    result[0] = 0;
                    result[1] = 1;
                    for (int i = 2; i < 9; i++) result[i] = 1;
                    result[9] = 1;
                    return result;
                }
                return firstNumber;
            }
            return expFirst == 0xFF ? firstNumber : secondNumber;
        }

        int maxExp = Math.Max(expFirst, expSecond);
        int shiftFirst = maxExp - expFirst;
        int shiftSecond = maxExp - expSecond;

        if (shiftFirst > 0)
        {
            for (int i = 23; i >= shiftFirst; i--)
                mantFirst[i] = mantFirst[i - shiftFirst];
            for (int i = 0; i < shiftFirst; i++)
                mantFirst[i] = 0;
            expFirst = maxExp;
        }
        if (shiftSecond > 0)
        {
            for (int i = 23; i >= shiftSecond; i--)
                mantSecond[i] = mantSecond[i - shiftSecond];
            for (int i = 0; i < shiftSecond; i++)
                mantSecond[i] = 0;
            expSecond = maxExp;
        }

        int[] resultMant = new int[24];
        int temp = 0;
        if (signFirst == signSecond)
        {
            for (int i = 23; i >= 0; i--)
            {
                int sum = mantFirst[i] + mantSecond[i] + temp;
                resultMant[i] = sum & 1;
                temp = sum >> 1;
            }
        }
        else
        {
            int[] larger = mantFirst, smaller = mantSecond;
            int signResult = signFirst;
            bool swap = false;
            for (int i = 0; i < 24; i++)
            {
                if (mantFirst[i] > mantSecond[i]) break;
                if (mantSecond[i] > mantFirst[i])
                {
                    larger = mantSecond;
                    smaller = mantFirst;
                    signResult = signSecond;
                    swap = true;
                    break;
                }
            }
            for (int i = 23; i >= 0; i--)
            {
                int diff = larger[i] - smaller[i] - temp;
                if (diff < 0)
                {
                    diff += 2;
                    temp = 1;
                }
                else
                    temp = 0;
                resultMant[i] = diff;
            }
            signFirst = signResult;
        }

        int finalExp = maxExp;
        int shift = 0;
        bool isZero = true;
        for (int i = 0; i < 24; i++)
            if (resultMant[i] != 0)
            {
                isZero = false;
                break;
            }
        if (isZero)
            return new int[32];

        if (temp == 1 || resultMant[0] == 1)
        {
            for (int i = 23; i > 0; i--)
                resultMant[i] = resultMant[i - 1];
            resultMant[0] = temp;
            finalExp++;
        }
        else
        {
            while (resultMant[0] == 0 && finalExp > 0)
            {
                for (int i = 0; i < 23; i++)
                    resultMant[i] = resultMant[i + 1];
                resultMant[23] = 0;
                finalExp--;
                shift--;
            }
        }

        if (finalExp >= 0xFF)
        {
            result[0] = signFirst;
            for (int i = 1; i < 9; i++) result[i] = 1;
            return result;
        }
        if (finalExp <= 0)
        {
            result[0] = signFirst;
            return result;
        }

        result[0] = signFirst;
        int tempExp = finalExp;
        for (int i = 8; i >= 1; i--)
        {
            result[i] = tempExp & 1;
            tempExp >>= 1;
        }
        for (int i = 0; i < 23; i++)
            result[i + 9] = resultMant[i + 1];

        return result;
    }

    public static string ToBinaryString(int[] bits)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < bits.Length; i++)
        {
            sb.Append(bits[i]);
            if (i == 0 || i == 8) sb.Append(" ");
        }
        return sb.ToString();
    }
}
