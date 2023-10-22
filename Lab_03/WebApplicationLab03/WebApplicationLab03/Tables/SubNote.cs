using System;
using System.Collections.Generic;

namespace WebApplicationLab03.Tables;

public partial class SubNote
{
    public int SubNoteId { get; set; }

    public int? NoteId { get; set; }

    public string? SubNoteDescription { get; set; }

    public bool? IsCompleted { get; set; }

    public virtual Note? Note { get; set; }
}
