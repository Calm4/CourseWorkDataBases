using System;
using System.Collections.Generic;

namespace WebApplicationLab03.Tables;

public partial class NoteStatus
{
    public int NoteStatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}
