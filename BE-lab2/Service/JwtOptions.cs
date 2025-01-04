﻿namespace BE_lab2.Service;

public class JwtOptions
{
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public string? Key { get; set; }
    public string? TokenValidityMins { get; set; }
}
