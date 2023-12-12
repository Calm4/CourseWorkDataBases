using System;
using System.Collections.Generic;

namespace Lab_06.Models;

public partial class SubNote
{
    public int SubNoteId { get; set; }

    public int? NoteId { get; set; }

    public string? SubNoteDescription { get; set; }

    public bool? IsCompleted { get; set; }

    public virtual Note? Note { get; set; }
}
