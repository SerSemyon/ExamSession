using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSession.Таблицы
{
    public class StudyGroup
    {
        [Key]
        public string number_group { get; set; }
        public string specialization { get; set; }
        public Student elder { get; set; }
        public Teacher tutor { get; set; }
        public int number_of_students { get; set; }
    }
}
