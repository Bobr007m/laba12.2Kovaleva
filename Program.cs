using Geometryclass;
using laba12._2;
using System;

class Program
{
    static void Main()
    {
        var table = new MyHashTable<string, Geometryfigure1>();
        var rand = new Random();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("1. Добавить элемент");
            Console.WriteLine("2. Найти элемент");
            Console.WriteLine("3. Удалить элемент");
            Console.WriteLine("4. Вывести таблицу");
            Console.WriteLine("5. Заполнить случайными фигурами");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddElementManually(table);
                        break;
                    case "2":
                        FindElement(table);
                        break;
                    case "3":
                        RemoveElement(table);
                        break;
                    case "4":
                        Console.WriteLine(table.GetTableInfo());
                        break;
                    case "5":
                        FillRandomly(table, rand);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.WriteLine("Нажмите Enter для продолжения...");
            Console.ReadLine();
        }
    }
    static int GetRandomSize() => new Random().Next(1, 10);
    static void AddElementManually(MyHashTable<string, Geometryfigure1> table)
    {
        Console.Write("Введите ключ: ");
        var key = Console.ReadLine();

        Console.WriteLine("Выберите тип фигуры (1-Круг, 2-Прямоугольник, 3-Параллелепипед): ");
        var type = Console.ReadLine();

        Geometryfigure1 figure = null;

        if (type == "1")
        {
            figure = new Circle1(GetRandomSize());
        }
        else if (type == "2")
        {
            figure = new Rectangle1(GetRandomSize(), GetRandomSize());
        }
        else if (type == "3")
        {
            figure = new Parallelepiped1(GetRandomSize(), GetRandomSize(), GetRandomSize());
        }
        else
        {
            throw new ArgumentException("Неверный тип фигуры");
        }

        table.Add(key, figure);
        Console.WriteLine("Элемент добавлен");
    }

    static void FindElement(MyHashTable<string, Geometryfigure1> table)
    {
        if (table.Count == 0)
        {
            Console.WriteLine("Таблица пуста");
            return;
        }

        Console.Write("Введите ключ: ");
        var key = Console.ReadLine();

        if (table.TryGetValue(key, out var value))
            Console.WriteLine($"Найдено: {value}");
        else
            Console.WriteLine("Элемент не найден");
    }

    static void RemoveElement(MyHashTable<string, Geometryfigure1> table)
    {
        if (table.Count == 0)
        {
            Console.WriteLine("Таблица пуста");
            return;
        }

        Console.Write("Введите ключ: ");
        var key = Console.ReadLine();

        if (table.Remove(key))
            Console.WriteLine("Элемент удален");
        else
            Console.WriteLine("Элемент не найден");
    }

    static void FillRandomly(MyHashTable<string, Geometryfigure1> table, Random rand)
    {
        Console.Write("Сколько элементов добавить? ");
        if (!int.TryParse(Console.ReadLine(), out var count) || count <= 0)
        {
            Console.WriteLine("Некорректное число");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            // Генерируем уникальный ключ 
            string key = $"figure_{i}";

            // Создаем случайную фигуру
            int figureType = rand.Next(1, 4);
            Geometryfigure1 figure;

            if (figureType == 1)
            {
                figure = new Circle1(rand.Next(1, 10));
            }
            else if (figureType == 2)
            {
                figure = new Rectangle1(rand.Next(1, 10), rand.Next(1, 10));
            }
            else if (figureType == 3)
            {
                figure = new Parallelepiped1(rand.Next(1, 10), rand.Next(1, 10), rand.Next(1, 10));
            }
            else
            {
                throw new InvalidOperationException("Неизвестный тип фигуры");
            }

            // Добавляем элемент в таблицу
            table.Add(key, figure);
        }

        Console.WriteLine($"Добавлено {count} элементов");
            // Создаем маленькую хеш-таблицу с низким порогом заполнения для демонстрации
        var table1 = new MyHashTable<string, Geometryfigure1>(capacity: 3, loadFactor: 0.7);
        Console.WriteLine("Создана хеш-таблица с начальной емкостью 3 и порогом заполнения 0.7");

        // Добавляем элементы до достижения порога
        Console.WriteLine("\n1. Добавляем элементы до достижения порога заполнения:");
        table.Add("circle1", new Circle1(5));
        Console.WriteLine($"Добавлен circle1. Текущий коэффициент заполнения: {table1.LoadFactor:P0}");
        table.Add("rect1", new Rectangle1(3, 4));
        Console.WriteLine($"Добавлен rect1. Текущий коэффициент заполнения: {table1.LoadFactor:P0}");

        // Выводим текущее состояние таблицы
        Console.WriteLine("\nТекущее состояние таблицы:");
        Console.WriteLine(table.GetTableInfo());

        // Пытаемся добавить элемент, который вызовет расширение таблицы
        Console.WriteLine("\n2. Пытаемся добавить третий элемент (должен вызвать Resize):");
        try
        {
            table.Add("paral1", new Parallelepiped1(2, 3, 4));
            Console.WriteLine($"Добавлен paral1. Текущий коэффициент заполнения: {table.LoadFactor:P0}");
            Console.WriteLine("\nСостояние таблицы после расширения:");
            Console.WriteLine(table.GetTableInfo());
            Console.WriteLine("Размер таблицы увеличился автоматически!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        // Теперь попробуем заполнить таблицу до предела
        Console.WriteLine("\n3. Пытаемся заполнить таблицу до предела:");
        try
        {
            // Добавляем элементы, пока не возникнет ошибка
            for (int i = 0; i < 10; i++)
            {
                string key = $"item{i}";
                table.Add(key, new Circle1(i + 1));
                Console.WriteLine($"Добавлен {key}. Коэффициент: {table.LoadFactor:P0}");
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\nОшибка при добавлении: {ex.Message}");
            Console.WriteLine("Достигнут максимальный размер таблицы!");
        }

        // Выводим финальное состояние таблицы
        Console.WriteLine("\nФинальное состояние таблицы:");
        Console.WriteLine(table.GetTableInfo());
    }
}

