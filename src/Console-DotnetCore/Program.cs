using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Console_DotnetCore
{
    public static class StringExtensions
    {
        public static List<string> Permute(this string element)
        {
            return element.ToCharArray().Permute(0, element.Length - 1);
        }

        public static List<string> Permute(this char[] elements, int recursionDepth, int maxDepth)
        {
            var list = new List<string>();

            if (recursionDepth == maxDepth)
            {
                list.Add(new string(elements));
                return list;
            }

            for (int i = recursionDepth; i <= maxDepth; i++)
            {
                Swap(ref elements[recursionDepth], ref elements[i]);
                list.AddRange(elements.Permute(recursionDepth + 1, maxDepth));
                // backtrack
                Swap(ref elements[recursionDepth], ref elements[i]);
            }

            return list;
        }

        private static void Swap(ref char a, ref char b)
        {
            char tmp = a;
            a = b;
            b = tmp;
        }
    }

    class Program
    {
        class Schedule
        {
            private readonly List<int?> _workHoursByWeekDay = new List<int?>();

            private string _pattern;

            public Schedule(string pattern)
            {
                _pattern = pattern;

                pattern.ToList().ForEach(x =>
                {
                    if (int.TryParse(x.ToString(), out var result))
                        _workHoursByWeekDay.Add(result);
                    else
                        _workHoursByWeekDay.Add(null);
                });
            }

            public List<string> FindSchedules(int workHours, int dayHours)
            {
                var emptySpacesCount = _workHoursByWeekDay.Where(x => x == null).Count();
                var totalFixedHours = _workHoursByWeekDay.Where(x => x != null).Sum().Value;

                var remainderWorkHours = workHours - totalFixedHours;

                var _possibleSchedules = FindInnerSchedules(remainderWorkHours, dayHours, emptySpacesCount);

                //for (int i = 0; i < _possibleSchedules.Count; i++)
                //{
                //    var index = 0;
                //    var newX = string.Empty;
                //    _workHoursByWeekDay.ForEach(y =>
                //    {
                //        if (y.HasValue)
                //            newX += y.ToString();
                //        else
                //        {
                //            newX += _possibleSchedules[i][index];
                //            index++;
                //        }
                //    });

                //    _possibleSchedules[i] = newX;
                //}

                var arranjes = new List<string>();
                foreach (var l in _possibleSchedules)
                {
                    arranjes.AddRange(l.Permute().Distinct());
                }

                for (int i = 0; i < arranjes.Count; i++)
                {
                    var index = 0;
                    var newX = string.Empty;
                    _workHoursByWeekDay.ForEach(y =>
                    {
                        if (y.HasValue)
                            newX += y.ToString();
                        else
                        {
                            newX += arranjes[i][index];
                            index++;
                        }
                    });

                    arranjes[i] = newX;
                }

                return arranjes.OrderBy(x => x).ToList();
                //return _possibleSchedules.ToList();//.OrderBy(x => x).ToList();
            }

            public List<string> FindInnerSchedules(int total, int reference, int spaces)
            {
                reference = Math.Min(total, reference);

                var list = new List<string>();
                var currentReference = reference;
                
                //if (total / spaces > reference)
                //    return null;

                if (spaces <= 1)
                    return new List<string> { reference.ToString() };

                while  (currentReference * spaces >= total) //(currentReference >= 0)
                {

                    var tempSchedules = FindInnerSchedules(total - currentReference, reference, spaces - 1);

                    if (tempSchedules != null)
                    {
                        for (int j = 0; j < tempSchedules.Count; j++)
                        {
                            list.Add($"{currentReference}{tempSchedules[j]}");
                        }
                    }


                    currentReference--;
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
                return schedule.FindSchedules(workHours, dayHours);
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
