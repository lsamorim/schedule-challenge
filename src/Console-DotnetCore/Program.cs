using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Console_DotnetCore
{
    class Program
    {
        class Schedule
        {
            private readonly List<int?> _workHoursByDay = new List<int?>();

            private readonly string _pattern;

            public Schedule(string pattern)
            {
                _pattern = pattern;

                Array.ForEach(_pattern.ToCharArray(), x =>
                {
                    if (int.TryParse(x.ToString(), out var result))
                        _workHoursByDay.Add(result);
                    else
                        _workHoursByDay.Add(null);
                });
            }

            public List<string> FindAllPossibleSchedules(int workHours, int dayHours)
            {
                var totalFlexibleDays = _workHoursByDay.Where(x => x == null).Count();
                var totalFixedHours = _workHoursByDay.Where(x => x != null).Sum().Value;

                var remainderWorkHours = workHours - totalFixedHours;

                var _possibleSchedules = FindAllFlexibleHours(remainderWorkHours, dayHours, totalFlexibleDays);

                for (int i = 0; i < _possibleSchedules.Count; i++)
                {
                    var flexibleHourIndex = 0;
                    var tempSchedule = string.Empty;
                    _workHoursByDay.ForEach(workHour =>
                    {
                        if (workHour.HasValue)
                            tempSchedule += workHour.ToString();
                        else
                        {
                            tempSchedule += _possibleSchedules[i][flexibleHourIndex];
                            flexibleHourIndex++;
                        }
                    });

                    _possibleSchedules[i] = tempSchedule;
                }

                return _possibleSchedules;
            }

            public List<string> FindAllFlexibleHours(int targetHours, int maxDayHours, int days)
            {
                maxDayHours = Math.Min(targetHours, maxDayHours);

                if (targetHours / days > maxDayHours)
                    return null;

                if (days <= 1)
                    return new List<string> { maxDayHours.ToString() };

                var list = new List<string>();
                var currentDayHour = 0;

                while (currentDayHour <= maxDayHours)
                {
                    var tempSchedules = FindAllFlexibleHours(targetHours - currentDayHour, maxDayHours, days - 1);

                    if (tempSchedules != null)
                    {
                        for (int j = 0; j < tempSchedules.Count; j++)
                        {
                            list.Add($"{currentDayHour}{tempSchedules[j]}");
                        }
                    }

                    currentDayHour++;
                }

                return list;
            }
        }

        class Result
        {
            /*
             * Complete the 'findSchedules' function below.
             *
             * The function is expected to return a STRING_ARRAY.
             * The function accepts following parameters:
             *  1. INTEGER workHours
             *  2. INTEGER dayHours
             *  3. STRING pattern
             */
            public static List<string> findSchedules(int workHours, int dayHours, string pattern)
            {
                var schedule = new Schedule(pattern);
                return schedule.FindAllPossibleSchedules(workHours, dayHours);
            }
        }

        public static void Main(string[] args)
        {
            int workHours = Convert.ToInt32(Console.ReadLine().Trim());

            int dayHours = Convert.ToInt32(Console.ReadLine().Trim());

            string pattern = Console.ReadLine();

            List<string> result = Result.findSchedules(workHours, dayHours, pattern);

            Console.WriteLine(String.Join("\n", result));

            Console.ReadKey();
        }
    }
}
