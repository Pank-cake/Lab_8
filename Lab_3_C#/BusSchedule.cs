using System;

namespace Lab8
{
    // Класс, представляющий запись в расписании автостанции
    public class BusSchedule
    {
        // Приватные поля: camelCase с префиксом _ (стандарт Microsoft)
        private int _routeNumber;
        private string _destination;
        private DateTime _departureTime;
        private int _platform;
        private decimal _ticketPrice;
        private int _totalSeats;
        private int _availableSeats;
        private string _busType;

        // Свойства: PascalCase
        public int RouteNumber
        {
            get
            {
                return _routeNumber;
            }
            set
            {
                _routeNumber = value;
            }
        }

        public string Destination
        {
            get
            {
                return _destination;
            }
            set
            {
                _destination = value;
            }
        }

        public DateTime DepartureTime
        {
            get
            {
                return _departureTime;
            }
            set
            {
                _departureTime = value;
            }
        }

        public int Platform
        {
            get
            {
                return _platform;
            }
            set
            {
                _platform = value;
            }
        }

        public decimal TicketPrice
        {
            get
            {
                return _ticketPrice;
            }
            set
            {
                _ticketPrice = value;
            }
        }

        public int TotalSeats
        {
            get
            {
                return _totalSeats;
            }
            set
            {
                _totalSeats = value;
            }
        }

        public int AvailableSeats
        {
            get
            {
                return _availableSeats;
            }
            set
            {
                _availableSeats = value;
            }
        }

        public string BusType
        {
            get
            {
                return _busType;
            }
            set
            {
                _busType = value;
            }
        }

        // Конструктор по умолчанию
        public BusSchedule()
        {
            _routeNumber = 0;
            _destination = string.Empty;
            _departureTime = DateTime.Now;
            _platform = 1;
            _ticketPrice = 0m;
            _totalSeats = 40;
            _availableSeats = 40;
            _busType = "Обычный";
        }

        // Конструктор с параметрами
        public BusSchedule(
            int routeNumber,
            string destination,
            DateTime departureTime,
            int platform,
            decimal ticketPrice,
            int totalSeats,
            int availableSeats,
            string busType)
        {
            _routeNumber = routeNumber;
            _destination = destination;
            _departureTime = departureTime;
            _platform = platform;
            _ticketPrice = ticketPrice;
            _totalSeats = totalSeats;
            _availableSeats = availableSeats;
            _busType = busType;
        }

        // Перегруженный метод ToString()
        public override string ToString()
        {
            string time = _departureTime.ToString("HH:mm");
            string date = _departureTime.ToString("dd.MM.yyyy");

            string result = $"Маршрут №{_routeNumber} | {_destination} | {date} {time} | " +
                         $"Платф.{_platform} | {_ticketPrice} руб. | " +
                         $"Мест: {_availableSeats}/{_totalSeats} | Тип: {_busType}";

            if (_availableSeats == 0)
            {
                result += " [МЕСТ НЕТ]";
            }
            else if (_availableSeats <= 5)
            {
                result += " [МАЛО МЕСТ]";
            }

            return result;
        }
    }
}