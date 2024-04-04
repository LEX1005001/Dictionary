﻿/*using System;
using System.Threading;
using System.Xml.Serialization;

namespace Timelon.Data
{
    /// <summary>
    /// Идентифицируемый вид
    /// </summary>
    public interface IUniqueIdentifiable
    {
        // PASS
    }

    /// <summary>
    /// Контейнер данных идентифицируемого объекта для сериализации
    /// </summary>
    public abstract class UniqueData : DataContainer
    {
        [XmlAttribute]
        public int Id;
        public string Word;
        public string Translation;
    }

    /// <summary>
    /// Абстракция идентифицируемого объекта
    /// Служит для хранения и генерации идентификаторов и названий
    /// Только для идентифицируемого вида
    /// </summary>
    public abstract class Unique<T> where T : IUniqueIdentifiable
    {
        /// <summary>
        /// Счетчик
        /// </summary>
        private static int _incrementor = 0;

        /// <summary>
        /// Зарегистрировать уникальный идентификатор в текущей сессии
        /// </summary>
        /// <param name="id">Идентификатор</param>
        private static void Register(int id)
        {
            if (id <= _incrementor)
            {
                return;
            }

            Interlocked.Exchange(ref _incrementor, id + 1);
        }

        /// <summary>
        /// Получить следующий уникальный идентификатор
        /// </summary>
        /// <returns>Уникальный идентификатор</returns>
        private static int UniqueId()
        {
            int result = _incrementor;

            Interlocked.Increment(ref _incrementor);
            return result;
        }

        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int _id;

        /// <summary>
        /// Темма сущности
        /// </summary>
        public string _theme;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="name">Название</param>
        public Unique(int id,string theme )
        {
            Id = id;
            Theme= theme;
        }

        /// <summary>
        /// Конструктор с автоматическим определением идентификатора
        /// </summary>
        /// <param name="name">Название</param>
        public Unique(string theme) : this(UniqueId(),theme)
        {
            // PASS
        }

        /// <summary>
        /// Доступ к уникальному идентификатору
        /// </summary>
        public int Id
        {
            get => _id;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("id не может быть отрицательным числом");
                }

                _id = value;

                // Регистрируем идентификатор в текущей сессии
                // во избежание дублирования
                Register(value);
            }
        }

        public string Theme
        {
            get => _theme;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentOutOfRangeException("theme не может быть пустой строкой");
                }

                _theme = value.Trim();
            }

        }

        public bool Equals(Unique<T> unique)
        {
            return unique.Id == Id && unique.Theme ==Theme;
        }
    }
}
*/