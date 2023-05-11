namespace MauimonGottaCatchEmAll;

public class ExceptionEventArgs : EventArgs
{
	public ExceptionEventArgs(string source, Exception exception)
	{
		Source = source;
		Exception = exception;
	}

	public string Source { get; set; }
	public Exception Exception { get; set; }

	public string ExceptionString
		=> Exception.ToString();

	public bool Handled { get; set; }
}
