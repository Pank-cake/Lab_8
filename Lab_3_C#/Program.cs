using lab8;
using System;
using System.Collections.Generic;

namespace Lab8
{
    class Program
    {
        private static List<BusSchedule> _schedules;

        static void Main(string[] args)
        {
            Console.Title = "Расписание автостанции";

            _schedules = DatabaseHelper.LoadDatabase();

            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                DrawMainMenu();

                int choice = InputValidator.ReadIntInRange("\nВаш выбор: ", 0, 7);

                Console.Clear();

                try
                {
                    switch (choice)
                    {
                        case 0:
                            exit = true;
                            Console.WriteLine("Сохранение данных...");
                            DatabaseHelper.SaveDatabase(_schedules);
                            Console.WriteLine("До свидания!");
                            break;
                        case 1: ViewAllRecords(); break;
                        case 2: AddNewRecord(); break;
                        case 3: DeleteRecord(); break;
                        case 4: QueryByDestination(); break;
                        case 5: QueryByAvailableSeats(); break;
                        case 6: QueryAveragePrice(); break;
                        case 7: QueryTotalAvailableSeats(); break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                if (!exit)
                {
                    Console.Write("\nНажмите любую клавишу...");
                    Console.ReadKey();
                }
            }
        }

        private static void DrawMainMenu()
        {
            Console.WriteLine("╔════════════════════════════════════════════════════════╗");
            Console.WriteLine("║        РАСПИСАНИЕ АВТОСТАНЦИИ (Лаб. раб. №8)           ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════╣");
            Console.WriteLine("║                                                        ║");
            Console.WriteLine("║  БАЗА ДАННЫХ:                                          ║");
            Console.WriteLine("║    1. Просмотр всех рейсов                             ║");
            Console.WriteLine("║    2. Добавить рейс                                    ║");
            Console.WriteLine("║    3. Удалить рейс                                     ║");
            Console.WriteLine("║                                                        ║");
            Console.WriteLine("║  LINQ-ЗАПРОСЫ (перечни):                               ║");
            Console.WriteLine("║    4. Рейсы по пункту назначения (orderby)             ║");
            Console.WriteLine("║    5. Рейсы по своб. местам (group by)                 ║");
            Console.WriteLine("║                                                        ║");
            Console.WriteLine("║  LINQ-ЗАПРОСЫ (одно значение):                         ║");
            Console.WriteLine("║    6. Средняя стоимость билета                         ║");
            Console.WriteLine("║    7. Всего свободных мест на дату                     ║");
            Console.WriteLine("║                                                        ║");
            Console.WriteLine("║    0. Выход                                            ║");
            Console.WriteLine("║                                                        ║");
            Console.WriteLine($"║  Записей в базе: {_schedules.Count,-37} ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════╝");
        }

        private static void ViewAllRecords()
        {
            Console.WriteLine("=== ПРОСМОТР ВСЕХ РЕЙСОВ ===\n");
            DatabaseHelper.ViewAll(_schedules);
        }

        private static void AddNewRecord()
        {
            Console.WriteLine("=== ДОБАВЛЕНИЕ РЕЙСА ===\n");

            int routeNumber = InputValidator.ReadInt("Номер маршрута: ");
            string destination = InputValidator.ReadNonEmptyString("Пункт назначения: ");
            DateTime departureTime = InputValidator.ReadDateTime("Дата и время (ГГГГ-ММ-ДД ЧЧ:ММ): ");
            int platform = InputValidator.ReadIntPositive("Номер платформы: ");
            decimal ticketPrice = InputValidator.ReadDecimalPositive("Стоимость билета (руб.): ");
            int totalSeats = InputValidator.ReadIntPositive("Общее количество мест: ");

            int availableSeats = InputValidator.ReadIntNonNegative("Свободных мест: ");
            while (availableSeats > totalSeats)
            {
                Console.WriteLine($"Ошибка: свободных мест не может быть больше {totalSeats}.");
                availableSeats = InputValidator.ReadIntNonNegative("Свободных мест: ");
            }

            string busType = InputValidator.ReadBusType("Тип автобуса (Эконом/Обычный/Комфорт/Люкс): ");

            BusSchedule newRecord = new BusSchedule(routeNumber, destination, departureTime, platform, ticketPrice, totalSeats, availableSeats, busType);

            DatabaseHelper.AddRecord(_schedules, newRecord);
        }

        private static void DeleteRecord()
        {
            Console.WriteLine("=== УДАЛЕНИЕ РЕЙСА ===\n");
            DatabaseHelper.ViewAll(_schedules);

            if (_schedules.Count == 0) return;

            int routeNumber = InputValidator.ReadInt("\nНомер маршрута: ");
            DateTime time = InputValidator.ReadDateTime("Время отправления (ГГГГ-ММ-ДД ЧЧ:ММ): ");

            DatabaseHelper.DeleteByKey(_schedules, routeNumber, time);
        }

        private static void QueryByDestination()
        {
            Console.WriteLine("=== РЕЙСЫ ПО ПУНКТУ НАЗНАЧЕНИЯ ===\n");
            string destination = InputValidator.ReadNonEmptyString("Пункт назначения: ");

            List<BusSchedule> result = DatabaseHelper.QueryByDestination(_schedules, destination);

            if (result.Count == 0)
            {
                Console.WriteLine($"\nРейсов в направлении '{destination}' не найдено.");
            }
            else
            {
                Console.WriteLine($"\nНайдено {result.Count} рейсов:");
                string divider = new string('-', 90);
                Console.WriteLine(divider);
                for (int i = 0; i < result.Count; i++)
                {
                    Console.WriteLine(result[i]);
                }
                Console.WriteLine(divider);
            }
        }

        private static void QueryByAvailableSeats()
        {
            Console.WriteLine("=== РЕЙСЫ ПО СВОБОДНЫМ МЕСТАМ ===\n");
            int minSeats = InputValidator.ReadIntNonNegative("Минимальное количество свободных мест: ");
            DatabaseHelper.QueryByAvailableSeats(_schedules, minSeats);
        }

        private static void QueryAveragePrice()
        {
            Console.WriteLine("=== СРЕДНЯЯ СТОИМОСТЬ БИЛЕТА ===\n");
            decimal average = DatabaseHelper.QueryAverageTicketPrice(_schedules);
            Console.WriteLine($"Средняя стоимость: {average:F2} руб.");
            Console.WriteLine($"Всего рейсов: {_schedules.Count}");
        }

        private static void QueryTotalAvailableSeats()
        {
            Console.WriteLine("=== СВОБОДНЫЕ МЕСТА НА ДАТУ ===\n");
            DateTime date = InputValidator.ReadDate("Дата (ГГГГ-ММ-ДД): ");
            int totalSeats = DatabaseHelper.QueryTotalAvailableSeatsByDate(_schedules, date);
            Console.WriteLine($"\nДата: {date:dd.MM.yyyy}");
            Console.WriteLine($"Свободных мест: {totalSeats}");
        }
    }
}
