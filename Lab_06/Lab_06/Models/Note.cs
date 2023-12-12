using System;
using System.Collections.Generic;

namespace Lab_06.Models;

public partial class Note
{
    public int NoteId { get; set; }

    public string? Title { get; set; }

    public string? NoteDescription { get; set; }

    public int UserId { get; set; }

    public int NoteTagId { get; set; }

    public int NoteStatusId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual NoteStatus? NoteStatus { get; set; } = null!;

    public virtual ICollection<SubNote> SubNotes { get; set; } = new List<SubNote>();

    public virtual NoteTag? NoteTag { get; set; } = null!;

    public virtual User? User { get; set; } = null!;
}
