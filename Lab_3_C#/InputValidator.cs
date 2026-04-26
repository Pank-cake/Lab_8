using System;

namespace lab8
{
    public static class InputValidator
    {
        public static int ReadInt(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Ошибка: введите число.");
                    continue;
                }

                int result;
                if (int.TryParse(input, out result))
                {
                    return result;
                }

                Console.WriteLine("Ошибка: введите целое число.");
            }
        }

        public static int ReadIntInRange(string prompt, int min, int max)
        {
            while (true)
            {
                int result = ReadInt(prompt);

                if (result >= min && result <= max)
                {
                    return result;
                }

                Console.WriteLine($"Ошибка: число должно быть от {min} до {max}.");
            }
        }

        public static int ReadIntPositive(string prompt)
        {
            while (true)
            {
                int result = ReadInt(prompt);

                if (result > 0)
                {
                    return result;
                }

                Console.WriteLine("Ошибка: число должно быть положительным.");
            }
        }

        public static int ReadIntNonNegative(string prompt)
        {
            while (true)
            {
                int result = ReadInt(prompt);

                if (result >= 0)
                {
                    return result;
                }

                Console.WriteLine("Ошибка: число не должно быть отрицательным.");
            }
        }

        public static decimal ReadDecimal(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Ошибка: введите число.");
                    continue;
                }

                decimal result;
                if (decimal.TryParse(input, out result))
                {
                    return result;
                }

                Console.WriteLine("Ошибка: введите десятичное число.");
            }
        }

        public static decimal ReadDecimalPositive(string prompt)
        {
            while (true)
            {
                decimal result = ReadDecimal(prompt);

                if (result > 0)
                {
                    return result;
                }

                Console.WriteLine("Ошибка: число должно быть положительным.");
            }
        }

        public static string ReadNonEmptyString(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                Console.WriteLine("Ошибка: строка не должна быть пустой.");
            }
        }

        public static string ReadString(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public static DateTime ReadDateTime(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Ошибка: введите дату и время.");
                    continue;
                }

                DateTime result;
                if (DateTime.TryParse(input, out result))
                {
                    return result;
                }

                Console.WriteLine("Ошибка: неверный формат. Используйте ГГГГ-ММ-ДД ЧЧ:ММ.");
            }
        }

        public static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Ошибка: введите дату.");
                    continue;
                }

                DateTime result;
                if (DateTime.TryParse(input, out result))
                {
                    return result;
                }

                Console.WriteLine("Ошибка: неверный формат. Используйте ГГГГ-ММ-ДД.");
            }
        }

        public static string ReadBusType(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Ошибка: введите тип автобуса.");
                    continue;
                }

                string lower = input.Trim().ToLower();
                if (lower == "эконом" || lower == "обычный" || lower == "комфорт" || lower == "люкс")
                {
                    return input.Trim();
                }

                Console.WriteLine("Ошибка: допустимые типы: Эконом, Обычный, Комфорт, Люкс.");
            }
        }
    }
}