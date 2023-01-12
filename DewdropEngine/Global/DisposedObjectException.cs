using System;

public class DisposedObjectException : Exception
{
    public DisposedObjectException(string name) : base($"Tried to access object '{name}' after it had been disposed!")
    {
    }
}
