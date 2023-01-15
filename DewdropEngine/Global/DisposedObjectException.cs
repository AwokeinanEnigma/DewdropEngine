using System;

/// <summary>
/// This exception is thrown when a disposed object is accessed.
/// </summary>
public class DisposedObjectException : Exception
{
    public DisposedObjectException(string name) : base($"Tried to access object '{name}' after it had been disposed!")
    {
    }
}
