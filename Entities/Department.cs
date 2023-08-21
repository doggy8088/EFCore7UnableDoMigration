#nullable disable
using System;
using System.Collections.Generic;

namespace EFCore7UnableDoMigration.Domain.Entities;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string Name { get; set; }

    public decimal Budget { get; set; }

    public DateTime StartDate { get; set; }

    public int? InstructorId { get; set; }

    public byte[] RowVersion { get; set; }

    public virtual ICollection<Course> Course { get; set; } = new List<Course>();

    public virtual Person Instructor { get; set; }
}