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

	bool TryToHandle = false;

	public ObservableCollection<ExceptionEventArgs> Exceptions = new ();

	static object lockObj = new object();

	private void MauiProgram_OnException(object sender, ExceptionEventArgs e)
	{
		System.Diagnostics.Debug.WriteLine($"{e.Source}: {e.ExceptionString}");

		lock (lockObj)
		{
			Exceptions.Add(e);
		}

		if (TryToHandle)
			e.Handled = true;
	}

	private void OnCounterClicked(object sender, EventArgs e)
	{
		TryToHandle = false;
		throw new Exception("void");
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		TryToHandle = true;
		throw new Exception("void");
	}
}

