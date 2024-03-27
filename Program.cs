namespace ConsoleFifthTask
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Переменные
            //Словарь со списком работников, где Keys - Идентификатор работника, Values - ФИО
            Dictionary<int, string> dictionaryOfWorkers = new Dictionary<int, string>()
            {
                [1] = "Иванова Ивана Ивановича",
                [2] = "Петрова Петра Петровича",
                [3] = "Юлиной Юлии Юлиановны",
                [4] = "Сидорова Сидора Сидоровича",
                [5] = "Павлова Павела Павловича",
                [6] = "Георгиева Георга Георгиевича"
            };

            //Словарь со списком начальных дат отпусков, где Keys - Идентификатор даты начала отпуска, Values - Дата начала отпуска
            Dictionary<int, DateTime> vacationStartDate = new Dictionary<int, DateTime>();

            //Словарь со списком конечных дат отпусков, где Keys - Идентификатор даты конца отпуска, Values - Дата конца отпуска
            Dictionary<int, DateTime> vacationEndDate = new Dictionary<int, DateTime>();

            //Список оставшихся дней отпуска у сотрудников
            List<int> countOfAbvVacationDays = new List<int>() {28,28,28,28,28,28};

            Random rnd = new Random();

            int countVacations = 1; //Общее кол-во отпуском для расчета индекса словарей
            #endregion

            // Цикл вычисления дат отпусков и вывода результата на консоль
            for (int index = 0; index < dictionaryOfWorkers.Count; index++)
            {
                int adIndex = 1; //Индекс кол-ва отпусков у сотрудника
                Console.WriteLine("\nОтпуска " + dictionaryOfWorkers[index + 1] + "\n");
                while(countOfAbvVacationDays[index] != 0) //Пока остаток отпусков у сотрудника(countOfAbvVacationDays) не равен 0 не выходим из цикла
                {
                    GetStartDate();
                    countOfAbvVacationDays[index] = GetEndDate(countOfAbvVacationDays[index],countVacations); // После вызова метода изменяем кол-во оставшихся дней отпуска у сотрудника
                    Console.WriteLine(adIndex + " : c " + vacationStartDate[countVacations].ToString("d") + " по " + 
                        vacationEndDate[countVacations].ToString("d") + " " +
                        (vacationEndDate[countVacations] - vacationStartDate[countVacations]).Days + " дней"); //Форматированный вывод на консоль
                    adIndex++;
                    countVacations++;
                }
            }

            Console.WriteLine("\nДля выхода из программы нажмите любую кнопку");
            Console.ReadKey();

            #region Методы
            /// Локальный метод получения даты начала отпуска
            void GetStartDate()
            {
                bool canBeChoose = false;
                DateTime beginingOfVacation;
                tryGetRightDayStartGlobal:
                if (DateTime.IsLeapYear(DateTime.Now.Year) == true) //Проверяем год, если високосный, то увеличиваем кол-во дней на 1
                {
                tryGetRightDayStart:
                    DateTime tempData = new DateTime(DateTime.Now.Year, 1, 1);
                    tempData = tempData.AddDays(rnd.Next(1, 335)); //Получаем случайный день, т.к. необходим отпуск только за один год(без переливов) убираем декабрь месяц
                    if (tempData.DayOfWeek == DayOfWeek.Sunday || tempData.DayOfWeek == DayOfWeek.Saturday) //Проверка на Субботу и Воскресенье, если выпало на них, подбираем день заново
                    {
                        goto tryGetRightDayStart;
                    }
                    beginingOfVacation = tempData;
                }
                else
                {
                tryGetRightDayStart:
                    DateTime tempData = new DateTime(DateTime.Now.Year, 1, 1);
                    tempData = tempData.AddDays(rnd.Next(1, 334)); //Получаем случайный день, т.к. необходим отпуск только за один год(без переливов) убираем декабрь месяц
                    if (tempData.DayOfWeek == DayOfWeek.Sunday || tempData.DayOfWeek == DayOfWeek.Saturday) //Проверка на Субботу и Воскресенье, если выпало на них, подбираем день заново
                    {
                        goto tryGetRightDayStart;
                    }
                    beginingOfVacation = tempData;
                }

                if (vacationStartDate.Count == 0) //Если это первый отпуск впринципе, добавляем в словарь первую запись, от которой можем начать отсчёт
                {
                    vacationStartDate.Add(1, beginingOfVacation);
                }
                else vacationStartDate.Add(vacationStartDate.Keys.Count + 1, beginingOfVacation);

                foreach (var item in vacationStartDate) //Проверка на совпадения отпусков
                {
                    if (canBeChoose == true)
                    {
                        goto tryGetRightDayStartGlobal;
                    }
                    else if (vacationStartDate.ContainsValue(beginingOfVacation) == false &&
                        beginingOfVacation > item.Value.AddDays(7) == true &&
                        beginingOfVacation > item.Value.AddDays(14) == true &&
                        beginingOfVacation < item.Value.AddDays(-7) == true &&
                        beginingOfVacation < item.Value.AddDays(-14) == true)
                    {
                        canBeChoose = true;
                    }
                    else continue;
                }
            }

            /// Локальный метод получения даты окончания отпуска
            /// Метод принимает кол-во оставшихся дней у сотрудника(_maxDaysCount) и кол-во текущих отпусков(countDates)
            /// Метод возвращает оставшееся у сотрудника кол-во дней отпуска
            int GetEndDate(int _maxDaysCount, int countDates)
            {
                        if (rnd.Next(0, 2) == 0 || _maxDaysCount == 7) //Рандомно выбираем кол-во дней отпуска, если у сотрудника осталось 7 дней, также обрабатываем тут
                        {
                            if (vacationEndDate.Count == 0) //Обработка первого окончания отпуска
                            {
                                vacationEndDate.Add(1, vacationStartDate[countDates].AddDays(7));
                        _maxDaysCount -= 7;
                                return _maxDaysCount;
                            }
                            else
                            {
                                vacationEndDate.Add(vacationStartDate.Keys.Count, vacationStartDate[countDates].AddDays(7));
                        _maxDaysCount -= 7;
                                return _maxDaysCount;
                            }
                        }
                        else
                        {
                            if (vacationEndDate.Count == 0) //Обработка первого окончания отпуска
                    {
                                vacationEndDate.Add(1, vacationStartDate[countDates].AddDays(14));
                                _maxDaysCount -= 14;
                                return _maxDaysCount;
                            }
                            else
                            {
                                vacationEndDate.Add(vacationStartDate.Keys.Count, vacationStartDate[countDates].AddDays(14));
                                _maxDaysCount -= 14;
                                return _maxDaysCount;
                            }
                        }
            }
            #endregion
        }
    }
}