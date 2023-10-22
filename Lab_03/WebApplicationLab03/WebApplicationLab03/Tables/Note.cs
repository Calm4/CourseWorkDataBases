﻿using System;
using System.Collections.Generic;

namespace WebApplicationLab03.Tables;

public partial class Note
{
    public int NoteId { get; set; }

    public string? Title { get; set; }

    public string? NoteDescription { get; set; }

    public int UserId { get; set; }

    public int TagId { get; set; }

    public int NoteStatusId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual NoteStatus NoteStatus { get; set; } = null!;

    public virtual ICollection<SubNote> SubNotes { get; set; } = new List<SubNote>();

    public virtual NoteTag Tag { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
