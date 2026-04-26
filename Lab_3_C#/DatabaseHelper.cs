using lab8;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab8
{
    public static class DatabaseHelper
    {
        // Стандарт: приватные поля начинаются с _ и используют camelCase
        private static string _filePath = "schedule.dat";

        public static List<BusSchedule> LoadDatabase()
        {
            var schedules = new List<BusSchedule>();

            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("Файл базы данных не найден. Создаётся новая база...");
                    schedules = CreateTestData();
                    SaveDatabase(schedules);
                    return schedules;
                }

                using var stream = File.OpenRead(_filePath);
                using var reader = new BinaryReader(stream);

                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    // Использование var и PascalCase для свойств объекта
                    var schedule = new BusSchedule
                    {
                        RouteNumber = reader.ReadInt32(),
                        Destination = reader.ReadString(),
                        DepartureTime = new DateTime(reader.ReadInt64()),
                        Platform = reader.ReadInt32(),
                        TicketPrice = reader.ReadDecimal(),
                        TotalSeats = reader.ReadInt32(),
                        AvailableSeats = reader.ReadInt32(),
                        BusType = reader.ReadString()
                    };
                    schedules.Add(schedule);
                }

                Console.WriteLine($"Загружено {schedules.Count} записей.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла: {ex.Message}");
                schedules = CreateTestData();
            }

            return schedules;
        }

        public static void SaveDatabase(List<BusSchedule> schedules)
        {
            try
            {
                using var stream = File.OpenWrite(_filePath);
                using var writer = new BinaryWriter(stream);

                foreach (var schedule in schedules)
                {
                    writer.Write(schedule.RouteNumber);
                    writer.Write(schedule.Destination);
                    writer.Write(schedule.DepartureTime.Ticks);
                    writer.Write(schedule.Platform);
                    writer.Write(schedule.TicketPrice);
                    writer.Write(schedule.TotalSeats);
                    writer.Write(schedule.AvailableSeats);
                    writer.Write(schedule.BusType);
                }

                Console.WriteLine($"База данных сохранена ({schedules.Count} записей).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
            }
        }

        public static void ViewAll(List<BusSchedule> schedules)
        {
            if (schedules.Count == 0)
            {
                Console.WriteLine("База данных пуста.");
                return;
            }

            var divider = new string('-', 90);
            Console.WriteLine(divider);
            for (var i = 0; i < schedules.Count; i++)
            {
                Console.WriteLine($"[{i}] {schedules[i]}");
            }
            Console.WriteLine(divider);
            Console.WriteLine($"Всего рейсов: {schedules.Count}");
        }

        public static void DeleteByKey(List<BusSchedule> schedules, int routeNumber, DateTime time)
        {
            // Используем LINQ для поиска, чтобы избежать громоздких циклов
            var scheduleToRemove = schedules.FirstOrDefault(s =>
                s.RouteNumber == routeNumber && s.DepartureTime == time);

            if (scheduleToRemove != null)
            {
                Console.WriteLine($"Удалён: {scheduleToRemove}");
                schedules.Remove(scheduleToRemove);
                SaveDatabase(schedules);
            }
            else
            {
                Console.WriteLine("Рейс не найден.");
            }
        }

        public static void AddRecord(List<BusSchedule> schedules, BusSchedule newRecord)
        {
            schedules.Add(newRecord);
            SaveDatabase(schedules);
            Console.WriteLine("Запись добавлена.");
        }

        public static List<BusSchedule> QueryByDestination(List<BusSchedule> schedules, string destination)
        {
            // Оптимизация LINQ: возвращаем результат сразу через .ToList()
            return (from schedule in schedules
                    where schedule.Destination.Contains(destination, StringComparison.OrdinalIgnoreCase)
                    orderby schedule.DepartureTime
                    select schedule).ToList();
        }

        public static void QueryByAvailableSeats(List<BusSchedule> schedules, int minSeats)
        {
            var groupedResults = from schedule in schedules
                                 where schedule.AvailableSeats >= minSeats
                                 group schedule by schedule.BusType;

            if (!groupedResults.Any())
            {
                Console.WriteLine($"Нет рейсов с местами >= {minSeats}.");
                return;
            }

            Console.WriteLine($"\nРейсы с местами >= {minSeats}:");
            foreach (var group in groupedResults)
            {
                Console.WriteLine($"\n  Тип: {group.Key}");
                foreach (var s in group)
                {
                    Console.WriteLine($"    {s}");
                }
            }
        }

        public static decimal QueryAverageTicketPrice(List<BusSchedule> schedules)
        {
            if (schedules.Count == 0)
            {
                return 0m;
            }

            return schedules.Average(s => s.TicketPrice);
        }

        public static int QueryTotalAvailableSeatsByDate(List<BusSchedule> schedules, DateTime date)
        {
            return schedules
                .Where(s => s.DepartureTime.Date == date.Date)
                .Sum(s => s.AvailableSeats);
        }

        private static List<BusSchedule> CreateTestData()
        {
            return new List<BusSchedule>
            {
                new(801, "Москва", new DateTime(2026, 4, 26, 10, 0, 0), 1, 3500.00m, 55, 12, "Люкс"),
                new(802, "Казань", new DateTime(2026, 4, 26, 20, 0, 0), 2, 3000.00m, 50, 25, "Комфорт"),
                new(803, "Ижевск", new DateTime(2026, 4, 26, 7, 45, 0), 3, 1900.00m, 45, 8, "Комфорт"),
                new(804, "Челябинск", new DateTime(2026, 4, 26, 7, 0, 0), 4, 2700.00m, 48, 15, "Люкс"),
                new(805, "Уфа", new DateTime(2026, 4, 26, 12, 0, 0), 5, 2400.00m, 45, 10, "Комфорт"),
                new(806, "Самара", new DateTime(2026, 4, 26, 14, 50, 0), 6, 2800.00m, 50, 5, "Комфорт"),
                new(807, "Нижний Тагил", new DateTime(2026, 4, 26, 12, 25, 0), 7, 1600.00m, 40, 18, "Обычный"),
                new(808, "Красноуфимск", new DateTime(2026, 4, 26, 17, 0, 0), 8, 1300.00m, 35, 12, "Обычный"),
                new(809, "Чайковский", new DateTime(2026, 4, 26, 10, 30, 0), 1, 950.00m, 38, 15, "Обычный"),
                new(810, "Лысьва", new DateTime(2026, 4, 26, 10, 35, 0), 2, 800.00m, 35, 10, "Эконом"),
                new(811, "Соликамск", new DateTime(2026, 4, 26, 9, 0, 0), 3, 1200.00m, 42, 8, "Комфорт"),
                new(812, "Березники", new DateTime(2026, 4, 26, 11, 55, 0), 4, 900.00m, 40, 12, "Обычный"),
                new(813, "Кудымкар", new DateTime(2026, 4, 26, 9, 30, 0), 5, 1000.00m, 38, 15, "Обычный"),
                new(814, "Оса", new DateTime(2026, 4, 26, 9, 5, 0), 6, 750.00m, 35, 10, "Эконом"),
                new(815, "Кизел", new DateTime(2026, 4, 26, 7, 50, 0), 4, 850.00m, 34, 16, "Эконом"),
                new(301, "Краснокамск", new DateTime(2026, 4, 26, 10, 55, 0), 2, 180.00m, 45, 15, "Обычный"),
                new(302, "Добрянка", new DateTime(2026, 4, 26, 16, 45, 0), 3, 200.00m, 42, 0, "Обычный"),
                new(303, "Полазна", new DateTime(2026, 4, 26, 8, 0, 0), 5, 170.00m, 38, 14, "Эконом"),
                new(304, "Усть-Качка", new DateTime(2026, 4, 26, 12, 50, 0), 6, 190.00m, 40, 25, "Комфорт"),
                new(305, "Гамово", new DateTime(2026, 4, 26, 17, 25, 0), 8, 110.00m, 30, 20, "Эконом")
            };
        }
    }
}