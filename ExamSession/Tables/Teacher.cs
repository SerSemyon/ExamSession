using System;
using System.Collections.Generic;

namespace ExamSession.Tables;

public partial class Teacher
{
    public int TeacherId { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? Patronymic { get; set; }

    public int? Age { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<StudyGroup> StudyGroups { get; } = new List<StudyGroup>();

    public virtual ICollection<TeacherItem> TeacherItems { get; } = new List<TeacherItem>();

    //public Teacher(string input)
    //{
    //    string[] data = input.Split(',');
    //    Name = data[0];
    //    LastName = data[1];
    //    Patronymic = data[2];
    //    Age = int.Parse(data[3]);
    //    PhoneNumber = data[4];
    //    Email = data[5];
    //}
}
