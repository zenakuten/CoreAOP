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
    public virtual void OnEnter(MethodInfo mi) { }
    public virtual void OnException(MethodInfo mi, Exception ex) { }
    public virtual void OnExit(MethodInfo mi) { }
}
```

Examples

```csharp
public abstract class LogAttribute : AttributeAttribute
{
    public override void OnEnter(MethodInfo mi) 
    { 
        Console.WriteLine($"Calling {mi.Name}");
    }
}
```

```csharp
public abstract class ProfileAttribute : AttributeAttribute
{
    private DateTime timestamp;
    public override void OnEnter(MethodInfo mi) 
    { 
        timestamp = DateTime.Now;
    }
    public override void OnExit(MethodInfo mi) 
    { 
        Console.WriteLine($"Calling {mi.Name} took {DateTime.Now - _timestamp}");
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

AddAspects() __must__ be called __after__ your services with aspect attributes have been added to the collection.

----

You can also add aspects by implementing the IAspect interface.

```csharp
using Microsoft.Extensions.DependencyInjection;

public class MyAspect : IAspect
{
    public void OnCreate(Type createdType) { }
    public void OnEnter(MethodInfo mi) 
    { 
        Console.WriteLine("MyAspect"); 
    }
    public void OnException(MethodInfo mi, Exception ex) { }
    public void OnExit(MethodInfo mi) { }
}
```
```csharp

 var services = new ServiceCollection();
 services.AddTransient<IMyService, MyService>();
 services.AddAspect<IMyService, MyAspect>();

```

The implementation is achieved using [DispatchProxy](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.dispatchproxy)