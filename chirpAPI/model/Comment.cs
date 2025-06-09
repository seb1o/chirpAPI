using System;
using System.Collections.Generic;

namespace chirpAPI.model;

public partial class Comment
{
    public int Id { get; set; }

    public int IdChirp { get; set; }

    public int? IdParent { get; set; }

    public string? Text { get; set; }

    public DateTime CreationTime { get; set; }

    public virtual Chirp IdChirpNavigation { get; set; } = null!;

    public virtual Comment? IdParentNavigation { get; set; }

    public virtual ICollection<Comment> InverseIdParentNavigation { get; set; } = new List<Comment>();
}
