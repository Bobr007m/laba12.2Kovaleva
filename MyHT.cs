using Geometryclass;
using laba12._2;
using System;
using System.Collections.Generic;
using System.Text;

namespace laba12._2
{
    public class MyHashTable <TKey, TValue>
    {
        // Массив записей Point<T>, представляющий собой хэш-таблицу
        public Point<TKey, TValue>[] table;

        // Счётчик текущего количества элементов в таблице
        private int count = 0;

        // Свойство, возвращающее количество элементов
        public int Count => count;

        // Коэффициент заполненности таблицы
        public double LoadFactor => (double)count / table.Length;

        public bool IsReadOnly => throw new NotImplementedException();

        // Конструктор таблицы
        public MyHashTable(int capacity = 10, double loadFactor = 0.72)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));
            if (loadFactor < 0 || loadFactor > 1)
                throw new ArgumentOutOfRangeException(nameof(loadFactor));

            // Создание массива заданной ёмкости
            table = new Point<TKey, TValue>[capacity];
        }

        // Проверка наличия ключа в таблице
        public bool Contains(TKey key)
        {
            if (key == null) return false;

            int index = Math.Abs(key.GetHashCode()) % table.Length;

            //  проход по таблице 
            for (int i = 0; i < table.Length; i++)
            {
                int currentIndex = (index + i) % table.Length;
                var entry = table[currentIndex];

                if (entry == null)
                    return false; // Если встретили null, значит такого ключа нет

                if (!entry.IsDeleted && entry.Key.Equals(key))
                    return true; // Нашли существующий  ключ
            }

            return false;
        }

        // Добавление пары ключ-значение в таблицу
        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            // Если таблица переполнена — увеличиваем её размер
            if (LoadFactor >= 0.72)
                Resize();

            int index = Math.Abs(key.GetHashCode()) % table.Length;

            // Поиск подходящей позиции для вставки
            for (int i = 0; i < table.Length; i++)
            {
                int currentIndex = (index + i) % table.Length;
                var entry = table[currentIndex];

                if (entry == null)
                {
                    // Вставка новой записи
                    table[currentIndex] = new Point<TKey, TValue>(key, value);
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
            Point<TKey, TValue>[] oldTable = table;
            table = new Point<TKey, TValue>[oldTable.Length * 2]; // Удваиваем размер
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
        public bool TryGetValue (TKey key, out TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            int index = Math.Abs(key.GetHashCode()) % table.Length;

            // Ищем значение по ключу 
            for (int i = 0; i < table.Length; i++)
            {
                int currentIndex = (index + i) % table.Length;
                var entry = table[currentIndex];

                if (entry == null)
                    throw new KeyNotFoundException("Элемент не найден.");

                if (!entry.IsDeleted && entry.Key.Equals(key))
                {
                    value = entry.Value;
                    return true;
                }    
            }
            value = default;
            return false;
            throw new KeyNotFoundException("Элемент не найден.");
        }

        // Удаление элемента по ключу
        public bool Remove(TKey key)
        {
            if (key == null || Count == 0)
                return false;

            int index = Math.Abs(key.GetHashCode()) % table.Length;

            // Поиск нужного элемента
            for (int i = 0; i < table.Length; i++)
            {
                int currentIndex = (index + i) % table.Length;
                var entry = table[currentIndex];

                if (entry == null)
                    return false;

                if (!entry.IsDeleted && entry.Key.Equals(key))
                {
                    entry.IsDeleted = true; // Просто помечаем как удалённый
                    count--;
                    return true;
                }
            }

            return false;
        }

        public string GetTableInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Хеш-таблица (Элементов: {Count}):");
            for (int i = 0; i < table.Length; i++)
            {
                var entry = table[i];
                sb.AppendLine(entry == null
                    ? $"[{i}]: Пусто"
                    : entry.IsDeleted
                        ? $"[{i}]: Удалено"
                        : $"[{i}]: {entry.Key} - {entry.Value}");
            }
            return sb.ToString();
        }
    }
}