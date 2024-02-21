using System;
using System.Collections.Generic;

namespace JaoudaMS_API.Models;

public partial class Authentification
{
    public string Login { get; set; } = null!;

    public string? Password { get; set; }
}
