using System;
using System.Collections.Generic;

namespace ConsoleEF7.NotePlannerDB;

public partial class NoteTag
{
    public int TagId { get; set; }

    public string? TagName { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
