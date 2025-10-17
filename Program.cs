using Shaunebu.Analytics.AppInsights;
using Shaunebu.Analytics.AppInsights.Abstractions;
using Shaunebu.Analytics.AppInsights.Models;

Console.WriteLine("📊 Application Insights Manager Demo (MAUI-Free)");
Console.WriteLine("================================================\n");

try
{
    // Demo 1: Basic Initialization with Custom Platform Info
    await DemoBasicTracking();

    // Demo 2: Custom Platform Configuration
    await DemoCustomPlatformConfig();

    // Demo 3: Storage Management
    await DemoStorageManagement();

    Console.WriteLine("\n🎉 All AppInsights demos completed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();

static async Task DemoBasicTracking()
{
    Console.WriteLine("1. 🔧 Basic Initialization");
    Console.WriteLine("---------------------------");

    string instrumentationKey = "your-instrumentation-key-here";
    var consoleLogger = new ConsoleLogger();

    // Initialize with default platform info
    AppInsightsManager.Current.Logger = consoleLogger;
    AppInsightsManager.Current.Initialize(
        instrumentationKey: instrumentationKey,
        debugMode: true,
        batchIntervalSeconds: 10
    );

    // Set custom user context
    AppInsightsManager.Current.UserId = "console-user-123";

    // Track events
    AppInsightsManager.Current.TrackEvent("ConsoleAppStarted");
    AppInsightsManager.Current.TrackEvent("DataProcessed",
        new Dictionary<string, string> { { "Records", "150" } },
        metric: 45.7);

    Console.WriteLine($"Session ID: {AppInsightsManager.Current.SessionId}");
    Console.WriteLine($"User ID: {AppInsightsManager.Current.UserId}");

    await Task.Delay(1500);
    await AppInsightsManager.Current.FlushAsync();
}

static async Task DemoCustomPlatformConfig()
{
    Console.WriteLine("\n2. 🖥️ Custom Platform Configuration");
    Console.WriteLine("-----------------------------------");

    string instrumentationKey = "your-instrumentation-key-here";
    var consoleLogger = new ConsoleLogger();

    // Create custom platform information
    var platformInfo = new PlatformInfo
    {
        Platform = "Windows",
        OSVersion = "10.0.19045",
        AppVersion = "2.3.1",
        AppBuild = "2310"
    };

    // Re-initialize with custom platform info
    AppInsightsManager.Current.Logger = consoleLogger;
    AppInsightsManager.Current.Initialize(
        instrumentationKey: instrumentationKey,
        debugMode: true,
        batchIntervalSeconds: 10,
        platformInfo: platformInfo
    );

    // You can also set platform properties directly
    AppInsightsManager.Current.Platform = "Linux";
    AppInsightsManager.Current.OSVersion = "Ubuntu 22.04";
    AppInsightsManager.Current.AppVersion = "2.4.0-beta";

    // Add global properties
    AppInsightsManager.Current.AddGlobalProperty("Deployment", "Production");
    AppInsightsManager.Current.AddGlobalProperty("Region", "NorthEurope");

    // Track events with custom platform context
    AppInsightsManager.Current.TrackEvent("CustomPlatformEvent",
        new Dictionary<string, string> { { "Feature", "AdvancedAnalytics" } });

    Console.WriteLine($"Platform: {AppInsightsManager.Current.Platform}");
    Console.WriteLine($"OS Version: {AppInsightsManager.Current.OSVersion}");
    Console.WriteLine($"App Version: {AppInsightsManager.Current.AppVersion}");

    await Task.Delay(1500);
    await AppInsightsManager.Current.FlushAsync();
}

static async Task DemoStorageManagement()
{
    Console.WriteLine("\n3. 💾 Storage Management");
    Console.WriteLine("------------------------");

    string instrumentationKey = "your-instrumentation-key-here";
    var consoleLogger = new ConsoleLogger();

    AppInsightsManager.Current.Logger = consoleLogger;
    AppInsightsManager.Current.Initialize(
        instrumentationKey: instrumentationKey,
        debugMode: true
    );

    // Demonstrate user ID persistence in memory
    var originalUserId = AppInsightsManager.Current.UserId;
    Console.WriteLine($"Original User ID: {originalUserId}");

    // Change user context
    AppInsightsManager.Current.UserId = "new-user-456";
    Console.WriteLine($"New User ID: {AppInsightsManager.Current.UserId}");

    // Track user-specific events
    AppInsightsManager.Current.TrackEvent("UserProfileUpdated");

    // Clear storage demo
    Console.WriteLine("Clearing storage...");
    AppInsightsManager.Current.ClearStorage();

    // User ID should be regenerated after clear
    Console.WriteLine($"User ID after clear: {AppInsightsManager.Current.UserId}");

    await Task.Delay(1000);
    await AppInsightsManager.Current.FlushAsync();
}

public class ConsoleLogger : IAppLogger
{
    public void Debug(string message) => Console.WriteLine($"[DEBUG] {message}");
    public void Info(string message) => Console.WriteLine($"[INFO] {message}");
    public void Warn(string message) => Console.WriteLine($"[WARN] {message}");
    public void Error(string message, Exception? ex = null)
    {
        Console.WriteLine($"[ERROR] {message}");
        if (ex != null) Console.WriteLine($"[EXCEPTION] {ex.Message}");
    }
}