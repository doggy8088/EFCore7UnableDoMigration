#nullable disable
using System;
using System.Collections.Generic;

namespace EFCore7UnableDoMigration.Domain.Entities;

public partial class OfficeAssignment
{
    public int InstructorId { get; set; }

    public string Location { get; set; }

    public virtual Person Instructor { get; set; }
}