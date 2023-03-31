using System;
using System.Collections.Generic;

namespace ExamSession.Tables;

public partial class TeacherItem
{
    public int Id { get; set; }

    public int? ItemId { get; set; }

    public int? TeacherId { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Teacher? Teacher { get; set; }
}
