using Geometryclass;
using laba12._2;
using System;
using System.Collections.Generic;

namespace laba12._2
{
    public class MyHashTable<T> where T : Geometryfigure1
    {
        // Массив записей Point<T>, представляющий собой хэш-таблицу
        public Point<T>[] MyHashtable;

        // Счётчик текущего количества элементов в таблице
        private int count = 0;

        // Свойство, возвращающее количество элементов
        public int Count => count;

        // Коэффициент заполненности таблицы
        private double LoadFactor => (double)count / MyHashtable.Length;

        public bool IsReadOnly => throw new NotImplementedException();
        public string Key { get; set; }
        public T Value { get; set; }
        public bool IsDeleted;

        // Конструктор таблицы
        public MyHashTable(int capacity = 10, double loadFactor = 0.72)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (loadFactor < 0 || loadFactor > 1)
                throw new ArgumentOutOfRangeException(nameof(loadFactor));

            // Создание массива заданной ёмкости
            MyHashtable = new Point<T>[capacity];
        }

        // Проверка наличия ключа в таблице
        public bool Contains(string key)
        {
            if (key == null) return false;

            int index = Math.Abs(key.GetHashCode()) % MyHashtable.Length;

            //  проход по таблице 
            for (int i = 0; i < MyHashtable.Length; i++)
            {
                int currentIndex = (index + i) % MyHashtable.Length;
                var entry = MyHashtable[currentIndex];

                if (entry == null)
                    return false; // Если встретили null, значит такого ключа нет

                if (!entry.IsDeleted && entry.Key == key)
                    return true; // Нашли существующий  ключ
            }

            return false;
        }

        // Добавление пары ключ-значение в таблицу
        public void Add(string key, T value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            // Если таблица переполнена — увеличиваем её размер
            if (LoadFactor >= 0.72)
                Resize();

            int index = Math.Abs(key.GetHashCode()) % MyHashtable.Length;

            // Поиск подходящей позиции для вставки
            for (int i = 0; i < MyHashtable.Length; i++)
            {
                int currentIndex = (index + i) % MyHashtable.Length;
                var entry = MyHashtable[currentIndex];

                if (entry == null)
                {
                    // Вставка новой записи
                    MyHashtable[currentIndex] = new Point<T>(key, value);
                    count++;
                    return;
                }
                else if (entry.IsDeleted || !entry.Key.Equals(key)) continue;

                // Если такой ключ уже есть —  исключение
                throw new ArgumentException("Элемент с таким ключом уже существует.");
            }

            // Если цикл завершился, а место не найдено — таблица переполнена
            throw new InvalidOperationException("Таблица переполнена.");
        }

        // Увеличение размера таблицы при превышении коэффициента заполнения
        public void Resize()
        {
            Point<T>[] oldTable = MyHashtable;
            MyHashtable = new Point<T>[oldTable.Length * 2]; // Удваиваем размер
            count = 0; // Обнуляем счётчик перед повторным добавлением

            foreach (var item in oldTable)
            {
                if (item != null && !item.IsDeleted)
                {
                    Add(item.Key, item.Value); // Перезаписываем все элементы в новую таблицу
                }
            }
        }

        // Получение значения по ключу
        public T Find(string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int index = Math.Abs(key.GetHashCode()) % MyHashtable.Length;

            // Ищем значение по ключу 
            for (int i = 0; i < MyHashtable.Length; i++)
            {
                int currentIndex = (index + i) % MyHashtable.Length;
                var entry = MyHashtable[currentIndex];

                if (entry == null)
                    throw new KeyNotFoundException("Элемент не найден.");

                if (!entry.IsDeleted && entry.Key == key)
                    return entry.Value;
            }

            throw new KeyNotFoundException("Элемент не найден.");
        }

        // Удаление элемента по ключу
        public bool Remove(string key)
        {
            if (key == null)
                return false;

            int index = Math.Abs(key.GetHashCode()) % MyHashtable.Length;

            // Поиск нужного элемента
            for (int i = 0; i < MyHashtable.Length; i++)
            {
                int currentIndex = (index + i) % MyHashtable.Length;
                var entry = MyHashtable[currentIndex];

                if (entry == null)
                    return false;

                if (!entry.IsDeleted && entry.Key == key)
                {
                    entry.IsDeleted = true; // Просто помечаем как удалённый
                    count--;
                    return true;
                }
            }

            return false;
        }

        // Метод вывода содержимого таблицы 
        public void PrintTable()
        {
            Console.WriteLine($"Хеш-таблица ( Элементов: {Count}):");
            for (int i = 0; i < MyHashtable.Length; i++)
            {
                var entry = MyHashtable[i];
                if (entry != null)
                {
                    if (entry.IsDeleted)
                        Console.WriteLine($"[{i}]: Удалено");
                    else
                        Console.WriteLine($"[{i}]: {entry.Key} - {entry.Value}");
                }
                else
                {
                    Console.WriteLine($"[{i}]: Пусто");
                }
            }
        }

        // Вспомогательный метод добавления с выводом информации о состоянии таблицы
        public void AddWithLoadFactorCheck(string key, T value)
        {
            try
            {
                Add(key, value);
                Console.WriteLine($"Добавлен {key}, LoadFactor = {LoadFactor:P2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Не удалось добавить {key}: {ex.Message}");
            }
        }
    }
}