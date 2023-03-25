using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamSession.Таблицы
{
    internal class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AudienceNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int NumberOfEmployees { get; set; }
        public Teacher HeadOfDepartment { get; set; }
    }
}
