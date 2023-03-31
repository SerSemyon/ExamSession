using System;
using System.Collections.Generic;

namespace ExamSession.Tables;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string? Name { get; set; }

    public int? AudienceNumber { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public int? NumberOfEmployees { get; set; }

    public int? HeadOfTheDepartment { get; set; }

    public virtual Teacher? HeadOfTheDepartmentNavigation { get; set; }
}
