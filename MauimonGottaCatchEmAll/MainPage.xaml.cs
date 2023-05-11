using System.Collections.ObjectModel;

namespace MauimonGottaCatchEmAll;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		cv.ItemsSource = Exceptions;

		MauiProgram.OnException += MauiProgram_OnException;

		cv.ItemsSource = Exceptions;
	}


	public ObservableCollection<ExceptionDetails> Exceptions = new ();

	private void MauiProgram_OnException(object sender, (string, Exception) e)
	{
		System.Diagnostics.Debug.WriteLine(e.Item1 + ": " + e.Item2);

		Exceptions.Add(new ExceptionDetails(e.Item1, e.Item2.ToString()));
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		throw new Exception("void");
	}
}

