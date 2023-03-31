using System;
using System.Collections.Generic;

namespace ExamSession.Tables;

public partial class Student
{
    public int StudentId { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public int? Age { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? NumberGroup { get; set; }

    public virtual ICollection<Assessment> Assessments { get; } = new List<Assessment>();

    public virtual StudyGroup? NumberGroupNavigation { get; set; }

    public virtual ICollection<StudyGroup> StudyGroups { get; } = new List<StudyGroup>();
}
