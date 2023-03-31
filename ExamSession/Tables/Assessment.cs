using System;
using System.Collections.Generic;

namespace ExamSession.Tables;

public partial class Assessment
{
    public int AssessmentId { get; set; }

    public int? ItemId { get; set; }

    public int? StudentId { get; set; }

    public int? Score { get; set; }

    public DateOnly? ExamDate { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Student? Student { get; set; }
}
