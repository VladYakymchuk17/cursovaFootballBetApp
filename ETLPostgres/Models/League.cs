using System;
using System.Collections.Generic;

namespace ETLPostgres.Models;

public partial class League
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Season { get; set; }

    public string Round { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
