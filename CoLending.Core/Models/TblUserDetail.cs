using System;
using System.Collections.Generic;

namespace CoLending.Core.Models;

public partial class TblUserDetail
{
    public long Id { get; set; }

    public string? UserId { get; set; }

    public string? Password { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? UpdatedOn { get; set; }
}
