using Microsoft.Extensions.Logging;

namespace MauimonGottaCatchEmAll;

public record ExceptionDetails(string Name, string Exception);

public static class MauiProgram
{
	public static event EventHandler<(string, Exception)> OnException;

	public static MauiApp CreateMauiApp()
	{

		AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
		AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

		TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

#if WINDOWS
		global::Microsoft.UI.Xaml.Application.Current.UnhandledException += Current_UnhandledException;
#elif ANDROID
		global::Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
#elif IOS || MACCATALYST
		ObjCRuntime.Runtime.MarshalManagedException += Runtime_MarshalManagedException;
#endif

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>();

		return builder.Build();
	}

	private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
	{
		OnException?.Invoke(sender, ("TaskScheduler.UnobservedTaskException", e.Exception));
		e.SetObserved();
	}

	private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
	{
		OnException?.Invoke(sender, ("AppDomain.CurrentDomain.FirstChanceException", e.Exception));
	}

	private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		OnException?.Invoke(sender, ("AppDomain.CurrentDomain.UnhandledException", e.ExceptionObject as Exception));
	}

#if WINDOWS
	private static void Current_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
	{
		OnException?.Invoke(sender, ("Microsoft.UI.Xaml.Application.Current.UnhandledException", e.Exception));
		e.Handled = true;
	}
#elif ANDROID
	private static void AndroidEnvironment_UnhandledExceptionRaiser(object sender, Android.Runtime.RaiseThrowableEventArgs e)
	{
		OnException?.Invoke(sender, ("Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser", e.Exception));
		e.Handled = true;
	}
#elif IOS || MACCATALYST
	private static void Runtime_MarshalManagedException(object sender, ObjCRuntime.MarshalManagedExceptionEventArgs e)
	{
		e.ExceptionMode = ObjCRuntime.MarshalManagedExceptionMode.UnwindNativeCode;
		OnException?.Invoke(sender, ("ObjCRuntime.Runtime.MarshalManagedException", e.Exception));
	}
#endif

}
