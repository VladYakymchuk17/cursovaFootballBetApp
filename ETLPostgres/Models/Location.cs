using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class Location
{
    public int Id { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
