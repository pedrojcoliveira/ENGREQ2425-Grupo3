﻿namespace AMAPP.API.Utils;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}