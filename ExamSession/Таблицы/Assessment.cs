using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace ExamSession.Таблицы
{
    internal class Assessment
    {
        public int Id { get; set; }
        public Item item { get; set; }
        public Student student { get; set; }
        public int Score { get; set; }
    }
}
