# Shaunebu.Analytics.AppInsights 📗


**A lightweight, cross-platform application lifecycle manager for .NET MAUI.**
This library provides **foreground/background lifecycle events** across **Android, iOS, and Windows**, allowing developers to react to app state changes in a consistent way. It abstracts platform differences and exposes a simple **event-driven API**.

![NuGet Version](https://img.shields.io/nuget/v/Shaunebu.Analytics.AppInsights?color=blue&label=NuGet)

![NET Support](https://img.shields.io/badge/.NET%20-%3E%3D8.0-blueviolet) ![NET Support](https://img.shields.io/badge/.NET%20CORE-%3E%3D3.1-blueviolet) ![NET Support](https://img.shields.io/badge/.NET%20MAUI-%3E%3D%208.0-blueviolet) [![Support](https://img.shields.io/badge/support-buy%20me%20a%20coffee-FFDD00)](https://buymeacoffee.com/jcz65te)

* * *

Features
--------

*   Singleton-based **AppInsights telemetry manager** for MAUI + .NET Standard.
    
*   Tracks **custom events, page views, and exceptions**.
    
*   Supports **offline buffering and batch flushing**.
    
*   Automatically enriches telemetry with **platform, OS, app version, session, and user info**.
    
*   Lifecycle-aware: automatically flushes on **app resume or sleep**.
    
*   Global default properties for all telemetry.
    
*   Fully compatible with **Microsoft.ApplicationInsights TelemetryClient**.
    
*   Optional **debug mode** for local logging.
    

* * *

Installation
------------

Install via NuGet:

`Install-Package Shaunebu.Analytics.AppInsights`

Add namespace references in your project:

`using Shaunebu.Analytics.AppInsights;`

* * *

Initialization
--------------

```csharp
// Initialize AppInsightsManager in App.xaml.cs
AppInsightsManager.Current.Initialize(
    connectionString: "<YOUR-CONNECTION-STRING>",
    debugMode: true,           // Optional: enables debug logging
    batchIntervalSeconds: 30   // Optional: flush interval in seconds
);
```

Optional: provide a **custom TelemetryClient**:

```csharp
var customClient = new TelemetryClient(new TelemetryConfiguration("<YOUR-CONNECTION-STRING>"));
AppInsightsManager.Current.Initialize("<YOUR-CONNECTION-STRING>", customClient: customClient);
```

* * *

Usage
-----

### Track Custom Event

```csharp
AppInsightsManager.Current.TrackEvent(
    "ButtonClicked",
    new Dictionary<string, string>
    {
        { "ButtonName", "Login" }
    },
    metric: 1
);
```

### Track Page View

```csharp
AppInsightsManager.Current.TrackPageView("MainPage");
```

### Track Exception

```csharp
try
{
    // Some code that throws
}
catch (Exception ex)
{
    AppInsightsManager.Current.TrackException(ex, new Dictionary<string, string>
    {
        { "Context", "LoginButtonClicked" }
    });
}
```

### Add Global Properties

```csharp
AppInsightsManager.Current.AddGlobalProperty("AppFlavor", "Beta");
```

### Flush Telemetry Immediately

```csharp
await AppInsightsManager.Current.FlushAsync();
```

* * *

Properties
----------

| Property | Type | Description |
| --- | --- | --- |
| `Current` | `AppInsightsManager` | Singleton instance of the manager. |
| `connectionString` | `string?` | AppInsights connection string. |
| `Logger` | `IAppLogger?` | Optional logger for debug/info/error messages. |
| `EnableDebugMode` | `bool` | Enable or disable debug logging. |
| `SessionId` | `string` | Current session identifier (auto-generated if not set). |
| `UserId` | `string` | Current user identifier (persisted via Preferences). |

* * *

Methods
-------

| Method | Parameters | Description |
| --- | --- | --- |
| `Initialize(string connectionString, bool debugMode = false, int batchIntervalSeconds = 30, TelemetryClient? customClient = null)` | `instrumentationKey`: AppInsights key  <br>`debugMode`: enable debug logging  <br>`batchIntervalSeconds`: flush interval  <br>`customClient`: optional custom TelemetryClient | Initializes the telemetry manager and hooks into MAUI lifecycle events. |
| `TrackEvent(string name, Dictionary<string, string>? properties = null, double? metric = null)` | `name`: event name  <br>`properties`: optional properties  <br>`metric`: optional numeric value | Tracks a custom event. |
| `TrackPageView(string pageName, Dictionary<string, string>? properties = null)` | `pageName`: page identifier  <br>`properties`: optional properties | Tracks a page view. |
| `TrackException(Exception ex, Dictionary<string, string>? properties = null)` | `ex`: Exception object  <br>`properties`: optional properties | Tracks an exception. |
| `AddGlobalProperty(string key, string value)` | `key`: property name  <br>`value`: property value | Adds a property to all future telemetry items. |
| `FlushAsync()` | None | Flushes all buffered telemetry immediately. |

* * *

Example: Full App.xaml.cs
-------------------------

```csharp
public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        // Initialize AppInsights
        AppInsightsManager.Current.Logger = new ConsoleLogger();
        AppInsightsManager.Current.Initialize("<YOUR-CONNECTION-STRING>", debugMode: true);

        // Track app start
        AppInsightsManager.Current.TrackEvent("App_Started");
    }
}
```

* * *

Logger Interface
----------------

Implement your own logger if needed:

```csharp
public class ConsoleLogger : AppInsightsManager.IAppLogger
{
    public void Info(string message) => Console.WriteLine("[INFO] " + message);
    public void Warn(string message) => Console.WriteLine("[WARN] " + message);
    public void Error(string message, Exception? ex = null)
    {
        Console.WriteLine("[ERROR] " + message + (ex != null ? " " + ex.Message : ""));
    }
}
```

* * *

Lifecycle & Platform Awareness
------------------------------

*   Hooks into **Application.Current.Resumed** and **Application.Current.Slept** to automatically flush telemetry.
    
*   Automatically enriches all telemetry with:
    *   `Platform` (Android/iOS/Windows)
        
    *   `OSVersion`
        
    *   `AppVersion`
        
    *   `AppBuild`
        
    *   `SessionId`
        
    *   `UserId`
        

* * *

Notes & Best Practices
----------------------

*   **Enable DebugMode** for development to log telemetry locally.
    
*   **AddGlobalProperty** for app-wide telemetry metadata.
    
*   **FlushAsync** should be called during app shutdown if needed.
    
*   Works with **.NET MAUI** and **.NET Standard 2.1+** libraries.