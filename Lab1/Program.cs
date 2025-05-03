namespace AOIS_Lab1
{
    public class Program
    {
        public static void Main()
        {
            int choice = 0;

            BinaryNumber firstNumber = new();
            BinaryNumber secondNumber = new();
            BinaryNumber result;

            int[] firstFloatNumber;
            int[] secondFloatNumber;
            int[] resultFloat;
            while (choice != 3)
            {
                Console.WriteLine("1. Операции с целыми числами");
                Console.WriteLine("2. Операция с числом с плавающей точкой");
                Console.WriteLine("3. Выход");
                Console.Write("Ваш выбор: ");
                try
                {
                    choice = Int32.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("\nПопробуйте ещё раз");
                    choice = 0;
                    continue;
                }
                switch (choice)
                {
                    case 1:
                        {
                            Console.Write("Введите первое число: ");
                            try
                            {
                                firstNumber.ToDirectBinaryNumber(Int32.Parse(Console.ReadLine()));
                            }
                            catch
                            {
                                Console.WriteLine("\nПопробуйте ещё раз");
                                continue;
                            }
                            firstNumber.Show();
                            Console.Write("Введите второе число: ");
                            try
                            {
                                secondNumber.ToDirectBinaryNumber(Int32.Parse(Console.ReadLine()));
                            }
                            catch
                            {
                                Console.WriteLine("\nПопробуйте ещё раз");
                                continue;
                            }
                            secondNumber.Show();
                            Console.WriteLine("Сложение: ");
                            result = firstNumber + secondNumber;
                            result.Show();
                            Console.WriteLine("Вычитание: ");
                            result = firstNumber - secondNumber;
                            result.Show();
                            Console.WriteLine("Умножение: ");
                            result = firstNumber * secondNumber;
                            result.Show();
                            Console.WriteLine("Деление: ");
                            result = firstNumber / secondNumber;
                            result.Show();
                            Console.Write("Остаток: ");
                            for (int i = 0; i < result.remainder.Length; i++)
                            {
                                Console.Write(result.remainder[i] + " ");
                            }
                            Console.WriteLine();
                            break;
                        }
                    case 2:
                        {
                            Console.Write("Введите первое число: ");
                            try
                            {
                                firstFloatNumber = FloatNumbers.ToFloatFromDecimal(float.Parse(Console.ReadLine()));
                            }
                            catch
                            {
                                Console.WriteLine("\nПопробуйте ещё раз");
                                continue;
                            }
                            Console.WriteLine(FloatNumbers.ToBinaryString(firstFloatNumber));
                            Console.Write("Введите второе число: ");
                            try
                            {
                                secondFloatNumber = FloatNumbers.ToFloatFromDecimal(float.Parse(Console.ReadLine()));
                            }
                            catch
                            {
                                Console.WriteLine("\nПопробуйте ещё раз");
                                continue;
                            }
                            Console.WriteLine(FloatNumbers.ToBinaryString(secondFloatNumber));
                            resultFloat = FloatNumbers.FloatSum(firstFloatNumber, secondFloatNumber);
                            Console.WriteLine("Результат сложения: " + FloatNumbers.ToDecimalFromFloat(resultFloat));
                            Console.WriteLine(FloatNumbers.ToBinaryString(resultFloat));
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("До встречи!");
                            break;
                        }
                }

            }
        }
    }
}