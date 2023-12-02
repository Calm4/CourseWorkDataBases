using System;
using System.Collections.Generic;

namespace Lab_05.Models;

public partial class NoteTag
{
    public int NoteTagId { get; set; }

    public string? TagName { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
