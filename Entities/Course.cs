#nullable disable
using System;
using System.Collections.Generic;

namespace EFCore7UnableDoMigration.Domain.Entities;

public partial class Course
{
    public int CourseId { get; set; }

    public string Title { get; set; }

    public int Credits { get; set; }

    public int DepartmentId { get; set; }

    public virtual Department Department { get; set; }

    public virtual ICollection<Enrollment> Enrollment { get; set; } = new List<Enrollment>();

    public virtual ICollection<Person> Instructor { get; set; } = new List<Person>();
}