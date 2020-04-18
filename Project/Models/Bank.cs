using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Models
{
    public class Bank
    {
        /// <summary>
        /// Идентификатор банка
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование банка
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// БИК Банка
        /// </summary>
        public string Bik { get; set; }

        /// <summary>
        /// Корреспондентский счёт
        /// </summary>
        public string CorrespondentAccount { get; set; }
    }
}
