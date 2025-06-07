using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOIS_Lab1
{
    public class BinaryNumber
    {
        private int[] directBinaryNumber = new int[8];
        private readonly int[] backBinaryNumber = new int[8];
        private readonly int[] additionalBinaryNumber = new int[8];
        public int[] remainder = new int[5];
        public void ToDirectBinaryNumber(int x)
        {

            int temp = x;
            for (int i = 0; i < directBinaryNumber.Length; i++)
            {
                directBinaryNumber[directBinaryNumber.Length - 1 - i] = Math.Abs(temp % 2);
                temp /= 2;
            }
            if (x < 0) directBinaryNumber[0] = 1;
            else directBinaryNumber[0] = 0;
            ToBackBinaryNumber(x);
            ToAdditionalBinaryNumber(x);
        }
        private void ToBackBinaryNumber(int x)
        {
            if (x >= 0)
                for (int i = 0; i < directBinaryNumber.Length; i++)
                    backBinaryNumber[i] = directBinaryNumber[i];
            else
            {
                backBinaryNumber[0] = 1;
                for (int i = 1; i < backBinaryNumber.Length; i++)
                    backBinaryNumber[i] = directBinaryNumber[i] % 2 == 0 ? 1 : 0;
            }
        }
        private void ToAdditionalBinaryNumber(int x)
        {
            if (x >= 0)
                for (int i = 0; i < additionalBinaryNumber.Length; i++)
                    additionalBinaryNumber[i] = directBinaryNumber[i];
            else
            {
                additionalBinaryNumber[0] = 1;
                for (int i = 1; i < additionalBinaryNumber.Length; i++)
                {
                    additionalBinaryNumber[i] = directBinaryNumber[i] % 2 == 0 ? 1 : 0;
                }
                for (int i = additionalBinaryNumber.Length - 1; i >= 0; i--)
                {
                    additionalBinaryNumber[i]++;
                    if (additionalBinaryNumber[i] == 1) break;
                    if (additionalBinaryNumber[i] >= 2)
                    {
                        additionalBinaryNumber[i] = 0;
                    }
                }
            }
        }
        public int ToDecimalNumber()
        {
            int x = 0;
            for (int i = 1; i < directBinaryNumber.Length; i++)
            {
                x += directBinaryNumber[i] * (int)Math.Pow(2, directBinaryNumber.Length - 1 - i);
            }
            if (directBinaryNumber[0] == 1) x *= -1;
            return x;
        }
        private static void ToBinaryFromAdditional(BinaryNumber number)
        {
            for (int i = 0; i < number.directBinaryNumber.Length; i++)
            {
                number.directBinaryNumber[i] = number.additionalBinaryNumber[i];
            }
            if (number.directBinaryNumber[0] == 1)
            {
                for (int i = number.directBinaryNumber.Length - 1; i >= 0; i--)
                {
                    number.directBinaryNumber[i]--;
                    if (number.directBinaryNumber[i] == 0) break;
                    if (number.directBinaryNumber[i] >= -1)
                    {
                        number.directBinaryNumber[i] = 1;
                    }
                }
                for (int i = 1; i < number.directBinaryNumber.Length; i++)
                {
                    number.directBinaryNumber[i] = number.directBinaryNumber[i] % 2 == 1 ? 0 : 1;
                }
            }
        }
        public static BinaryNumber operator +(BinaryNumber firstNumber, BinaryNumber secondNumber)
        {
            BinaryNumber result = new();
            for (int i = 0; i < result.additionalBinaryNumber.Length; i++)
            {
                result.additionalBinaryNumber[i] = firstNumber.additionalBinaryNumber[i];
            }
            bool isNeedToAdd = false;
            for (int i = result.additionalBinaryNumber.Length - 1; i >= 0; i--)
            {
                if (isNeedToAdd)
                {
                    isNeedToAdd = false;
                    result.additionalBinaryNumber[i]++;
                }
                result.additionalBinaryNumber[i] += secondNumber.additionalBinaryNumber[i];
                if (result.additionalBinaryNumber[i] == 1) continue;
                if (result.additionalBinaryNumber[i] >= 2)
                {
                    isNeedToAdd = true;
                    result.additionalBinaryNumber[i] -= 2;
                }
            }
            ToBinaryFromAdditional(result);
            result.ToBackBinaryNumber(result.ToDecimalNumber());
            return result;
        }
        public static BinaryNumber operator -(BinaryNumber firstNumber, BinaryNumber secondNumber)
        {
            BinaryNumber result = new();
            result.ToDirectBinaryNumber(0 - secondNumber.ToDecimalNumber());
            result += firstNumber;
            return result;
        }
        public static BinaryNumber operator *(BinaryNumber firstNumber, BinaryNumber secondNumber)
        {
            BinaryNumber result = new();
            bool isNegative = firstNumber.directBinaryNumber[0] != secondNumber.directBinaryNumber[0];
            firstNumber.directBinaryNumber[0] = 0;
            secondNumber.directBinaryNumber[0] = 0;
            BinaryNumber shifted = new();
            for (int i = firstNumber.directBinaryNumber.Length - 1; i >= 1; i--)
            {
                if (secondNumber.directBinaryNumber[i] == 1)
                {
                    for (int j = 1; j < (firstNumber.directBinaryNumber.Length - (firstNumber.directBinaryNumber.Length - 1 - i)); j++)
                    {
                        shifted.directBinaryNumber[j] = firstNumber.directBinaryNumber[j + firstNumber.directBinaryNumber.Length - 1 - i];
                    }
                    shifted.ToAdditionalBinaryNumber(shifted.ToDecimalNumber());
                    result += shifted;
                }
                shifted = new BinaryNumber();
            }
            if (isNegative)
                result.directBinaryNumber[0] = 1;
            else
                result.directBinaryNumber[0] = 0;
            result.ToBackBinaryNumber(result.ToDecimalNumber());
            result.ToAdditionalBinaryNumber(result.ToDecimalNumber());
            return result;
        }
        public static BinaryNumber operator /(BinaryNumber firstNumber, BinaryNumber secondNumber)
        {
            return DivideWithFractionalRemainder(firstNumber, secondNumber);
        }
        private static int CompareBinary(int[] a, int[] b)
        {
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return a[i] - b[i];
            }
            return 0;
        }
        private static void ComputeFractionalPart(BinaryNumber remainder, BinaryNumber divisor, int[] fractionalResult)
        {
            if (remainder.ToDecimalNumber() == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    fractionalResult[i] = 0;
                }
                return;
            }

            BinaryNumber currentRemainder = new();
            for (int j = 0; j < remainder.directBinaryNumber.Length; j++)
            {
                currentRemainder.directBinaryNumber[j] = remainder.directBinaryNumber[j];
            }

            for (int i = 0; i < 5; i++)
            {
                currentRemainder.directBinaryNumber = ShiftLeft(currentRemainder.directBinaryNumber);

                if (CompareBinary(currentRemainder.directBinaryNumber, divisor.directBinaryNumber) >= 0)
                {
                    currentRemainder = SubtractBinary(currentRemainder, divisor);
                    fractionalResult[i] = 1;
                }
                else
                {
                    fractionalResult[i] = 0;
                }
            }
        }

        private static int[] ShiftLeft(int[] array)
        {
            int[] result = new int[array.Length];
            for (int i = 0; i < array.Length - 1; i++)
            {
                result[i] = array[i + 1];
            }
            result[array.Length - 1] = 0;
            return result;
        }

        private static BinaryNumber SubtractBinary(BinaryNumber a, BinaryNumber b)
        {
            BinaryNumber result = new();
            int borrow = 0;
            for (int i = 7; i >= 0; i--)
            {
                int diff = a.directBinaryNumber[i] - b.directBinaryNumber[i] - borrow;
                if (diff < 0)
                {
                    diff += 2;
                    borrow = 1;
                }
                else
                {
                    borrow = 0;
                }
                result.directBinaryNumber[i] = diff;
            }
            return result;
        }
        private static int[] OffsetArrayLeft(int[] array)
        {
            int[] result = new int[array.Length];
            for (int i = 1; i < array.Length; i++)
            {
                result[i - 1] = array[i];
            }
            result[array.Length - 1] = 0;
            return result;
        }
        public static BinaryNumber DivideWithFractionalRemainder(BinaryNumber dividend, BinaryNumber divisor)
        {
            if (divisor.ToDecimalNumber() == 0)
            {
                throw new DivideByZeroException("Деление на ноль невозможно.");
            }

            bool isNegative = dividend.directBinaryNumber[0] != divisor.directBinaryNumber[0];
            dividend.directBinaryNumber[0] = 0;
            divisor.directBinaryNumber[0] = 0;

            BinaryNumber quotient = new();
            BinaryNumber remainder = new();

            for (int i = 1; i < dividend.directBinaryNumber.Length; i++)
            {
                remainder.directBinaryNumber = OffsetArrayLeft(remainder.directBinaryNumber);
                remainder.directBinaryNumber[7] = dividend.directBinaryNumber[i];

                if (CompareBinary(remainder.directBinaryNumber, divisor.directBinaryNumber) >= 0)
                {
                    remainder = SubtractBinary(remainder, divisor);
                    quotient.directBinaryNumber[i] = 1;
                }
                else
                {
                    quotient.directBinaryNumber[i] = 0;
                }
            }

            ComputeFractionalPart(remainder, divisor, quotient.remainder);

            if (isNegative)
            {
                quotient.directBinaryNumber[0] = 1;
            }
            else
            {
                quotient.directBinaryNumber[0] = 0;
            }

            quotient.ToBackBinaryNumber(quotient.ToDecimalNumber());
            quotient.ToAdditionalBinaryNumber(quotient.ToDecimalNumber());

            return quotient;
        }
        public void Show()
        {
            Console.Write("Десятичное число: " + this.ToDecimalNumber() + "\n");
            Console.Write("Прямой код: ");
            for (int i = 0; i < directBinaryNumber.Length; i++)
            {
                Console.Write(directBinaryNumber[i] + " ");
            }
            Console.Write("\nОбратный код: ");
            for (int i = 0; i < backBinaryNumber.Length; i++)
            {
                Console.Write(backBinaryNumber[i] + " ");
            }
            Console.Write("\nДополнительный код: ");
            for (int i = 0; i < additionalBinaryNumber.Length; i++)
            {
                Console.Write(additionalBinaryNumber[i] + " ");
            }
            Console.WriteLine();
        }
    }
}