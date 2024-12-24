using System;
using System.Collections.Generic;

namespace MyWebApi.Models;

public partial class Score
{
    public string? ClassId { get; set; }

    public string? StuId { get; set; }

    public string? StuName { get; set; }

    public string? StuGender { get; set; }

    public int? Math { get; set; }

    public int? English { get; set; }

    public int? Computer { get; set; }
}
