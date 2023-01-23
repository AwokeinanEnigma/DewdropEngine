using System;

/// <summary>
/// The DisposedObjectException class represents an exception that is thrown when an attempt is made to access an object after it has been disposed.
/// </summary>
public class DisposedObjectException : Exception
{
    /// <summary>
    /// Initializes a new instance of the DisposedObjectException class with a specified error message that includes the object name
    /// </summary>
    /// <param name="name">The name of the object that was accessed after disposal</param>
    public DisposedObjectException(string name) : base($"Tried to access object '{name}' after it had been disposed!")
    {
    }
}
