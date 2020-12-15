# UtilityLib

The *UtilityLib* libraries provides a set of helper classes to facilitate the implementation of applications by providing standardized implementations in various contexts.
An application class is derived from the corresponding base class using the default implementation (constructor, properties, methods).
Other helper classes are for default exit codes, data status, property validation, json converters, and custom attributes.

The following interfaces and classes are provided:
##

##### Interfaces
- IHttpClientSettings
- IPingSettings
- IUdpClientSettings

##### Base Classes
- BaseClass
- BaseCommand
- BaseController
- BaseOption
- BaseRootCommand

##### Helper Classes
- NumberConverter
- IPAddressConverter
- IPEndPointConverter
- TimeSpanConverter
- SpecialDoubleConverter

##### Status and Health Checks 
- DataStatus
- ExitCodes

##### Extensions
- ArgumentExtensions
- CommandExtensions
- HostExtensions
- JsonExtensions
- OptionExtensions
- PropertyExtensions
- ValidationExtensions

##### Settings
- HttpClientSettings
- PingSettings
- UdpClientSettings

##### Attributes
- UriAttribute
- IPAddressAttribute
- IPEndPointAttribute
- OptionValidationAttribute

## Base Classes

### BaseClass
The *BaseClass* class provides a standard logger data member using dependency injection.

~~~CSharp
protected readonly ILogger<BaseClass> _logger;
~~~

### BaseCommand
The *BaseCommand* class for console applications provides logger and default json serialization data members and is derived from the **Command** class.

### BaseRootCommand
The *BaseRootCommand* class for console applications provides logger and default json serialization data members and is derived from the **RootCommand** class.
The basic options *verbose*, *settings*, and *configuration* are provided. Note that the verbose option is a global command option.

### BaseOptions
The *BaseOptions* class for console applications provides base options data members used by the **BaseRootCommand** class.

### BaseController
The *BaseController* class for an API controller provides logger and configuration data members.
A helper method maps a data status to corresponding *ObjectResult*. 

## Helper Classes

### NumberConverter
This converter is used to convert strings containing numbers.

### TimeSpanConverter
This converter is used to convert TimeSpans (note: should use ISO8601).

### SpecialDoubleConverter
This converter allows special values (NaN, Infinity, -Infinity).

## Status and HealthChecks

### DataStatus
Class to provide OPC UA compatible result codes.

~~~CSharp
public long Timestamp { get; set; }
public uint Code { get; set; }
public string Name { get; set; }
public string Explanation { get; set; }

public bool IsGood { get; }
public bool IsNotGood { get; }
public bool IsUncertain { get; }
public bool IsNotUncertain { get; }
public bool IsBad { get; }
public bool IsNotBad { get; }
~~~

### ExitCodes
The *ExitCodes* class provides standard exit codes (enum) for a console application.

|     Exit Code Name    | Code |
|:----------------------|:----:|
| SuccessfullyCompleted |   0  |
| IncorrectFunction     |  -1  |
| FileNotFound          |   2  |
| PathNotFound          |   3  |
| CantOpenFile          |   4  |
| AccessDenied          |   5  |
| InvalidData           |  13  |
| ProgramNotRecognized  | 9009 |

##### On Windows OS
|     Exit Code Name    |    Code    |
|:----------------------|:----------:|
| OperationCanceled     | 0xC000013A |
| UnhandledException    | 0xE0434F4D |

##### On Non Windows OS
|     Exit Code Name    |      Code     |
|:----------------------|:-------------:|
| OperationCanceled     | 130 - SIGINT  |
| UnhandledException    | 134 - SIGABRT |

The last two exit codes are OS platform dependent.

## Extensions

### JsonExtensions
The *JsonExtensions* class provides helper extension methods dealing with JSON serializer options.

~~~CSharp
public static JsonSerializerOptions DefaultSerializerOptions
public static void AddDefaultOptions(this JsonSerializerOptions options)
~~~

### PropertyExtensions
Extension class to provide property helper methods for an object.

~~~CSharp
public static bool IsProperty(this Type type, string name)
public static object? GetPropertyValue(this object obj, string name)
public static void SetPropertyValue(this object obj, string name, object value)
~~~

### ValidationExtensions
Extension class to provide a validate an object based on its DataAnnotations, possibly throwing an exception if the object is not valid.

~~~CSharp
public static ICollection<ValidationResult> Validate<T>(this T obj)
public static T ValidateAndThrow<T>(this T obj)
~~~

### ArgumentExtensions

### OptionsExtensions

### CommandExtensions

### ParserExtensions

### Host Extensions

## Settings

### HttpClientSettings
The *HttpClientSettings* are used to configure *HttpClients*.

~~~CSharp
    public class HttpClientSettings : IHttpClientSettings
    {
        [AbsoluteUri]
        public string BaseAddress { get; set; } = "http://localhost";

        [Range(0, Int32.MaxValue)]
        public int Timeout { get; set; } = 100;
    }
~~~

### PingSettings
The *PingSettings* are used to configure the execution parameter of the *Ping* health check.
The **Host** property is used to define the remote host for the Ping.
The **Roundtrip** property is used to limit the allowed Ping roundtrip time (degrading) to the specified number of milliseconds.
The **DontFragment** and **Ttl** are ping options used to control how Ping data packets are transmitted.

~~~CSharp
    public class PingSettings : IPingSettings
    {
        public string Host { get; set; } = "localhost";

        [Range(0, Int32.MaxValue)]
        public int Timeout { get; set; } = 100;

        public bool DontFragment { get; set; }

        [Range(1, Int32.MaxValue)]
        public int Ttl { get; set; } = 128;

        [Range(1, Int32.MaxValue)]
        public int Roundtrip { get; set; } = 100;
    }
~~~

### UdpClientSettings
The *UdpClientSettings* are used to configure *UdpClients*.

~~~CSharp
    public class UdpClientSettings : IUdpClientSettings
    {
        [IPEndPoint]
        public string EndPoint { get; set; } = string.Empty;

        [Range(0, 65535)]
        public int Port { get; set; }

        [Range(0, double.MaxValue)]
        public double Timeout { get; set; } = 1.0;
    }
~~~

## Attributes

- Guid: Validates using *Guid.TryParse()*.
- IPAddress: Validates using *IPAddress.TryParse()*.
- IPEndPoint: Validates using *IPEndPoint.TryParse()*.
- Uri: Validates using *Uri.TryCreate()*.

