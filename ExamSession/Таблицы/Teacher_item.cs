using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSession.Таблицы
{
    internal class Teacher_item
    {
        public int Id { get; set; }
        public Item item { get; set; }
        public Teacher teacher { get; set; }
    }
}
