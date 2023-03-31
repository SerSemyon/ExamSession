using System;
using System.Collections.Generic;

namespace ExamSession.Tables;

public partial class Item
{
    public int ItemId { get; set; }

    public string? Name { get; set; }

    public int? HoursNum { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Assessment> Assessments { get; } = new List<Assessment>();

    public virtual ICollection<TeacherItem> TeacherItems { get; } = new List<TeacherItem>();
}
