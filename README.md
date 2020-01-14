# CoreAOP

### Aspect oriented programming using .NET Core and Microsoft's Dependency Injection.


This library allows you to extend interface functionality by defining and applying attributes to interfaces.


```csharp
[Profile]
public interface IMyService 
{
    [Log]
    bool MyMethod();
}
```

Attributes are defined by implementing a base class

```csharp
public abstract class AspectAttribute : Attribute, IAspect
{
    public virtual void OnCreate(Type createdType) { }
    public virtual object[] OnEnter(MethodInfo mi, object[] args) { }
    public virtual void OnException(MethodInfo mi, Exception ex) { }
    public virtual object OnExit(MethodInfo mi, object[] args, object retval) { }
}
```

Examples

```csharp
public class LogAttribute : AspectAttribute
{
    public override void OnEnter(MethodInfo mi, object[] args) 
    { 
        Console.WriteLine($"Calling {mi.Name}");
        return args;
    }
}
```

```csharp
public class ProfileAttribute : AspectAttribute
{
    private DateTime timestamp;
    public override void OnEnter(MethodInfo mi, object[] args) 
    { 
        timestamp = DateTime.Now;
        return args;
    }
    public override object OnExit(MethodInfo mi, object[] args, object retval) 
    { 
        Console.WriteLine($"Calling {mi.Name} took {DateTime.Now - timestamp}");
        return retval;
    }
}
```

The aspects are applied by extending IServiceCollection.


```csharp
using Microsoft.Extensions.DependencyInjection;

 var services = new ServiceCollection();
 services.AddTransient<IMyService, MyService>();
 services.AddAspects();
```

<sup>AddAspects() __must__ be called __after__ your services with aspect attributes have been added to the collection.</sup>

----

You can also add aspects by implementing the IAspect interface.

```csharp
using Microsoft.Extensions.DependencyInjection;

public class MyAspect : IAspect
{
    public void OnCreate(Type createdType) { }
    public object[] OnEnter(MethodInfo mi, object[] args) 
    { 
        Console.WriteLine("MyAspect"); 
        return args;
    }
    public void OnException(MethodInfo mi, Exception ex) { }
    public object OnExit(MethodInfo mi, object[] args, object retval) 
	{  
        return retval; 
	}
}
```
```csharp

 var services = new ServiceCollection();
 services.AddTransient<IMyService, MyService>();
 services.AddAspect<IMyService, MyAspect>();

```

The implementation is achieved using [DispatchProxy](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.dispatchproxy)