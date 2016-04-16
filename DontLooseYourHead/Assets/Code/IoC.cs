using System;
using System.Collections.Generic;

public static class IoC
{

    static IoC()
    {
        //register dependencies here
        RegisterSingleton(()=>new ShapeCreator());
    }

    private static Dictionary<Type, object> singletons = new Dictionary<Type, object>();

    public static TSingleton Resolve<TSingleton>()
    {
        var yes = false;

        if (singletons.ContainsKey(typeof(TSingleton)) && singletons[typeof(TSingleton)] is Func<TSingleton>)
        {
            singletons[typeof(TSingleton)] = ((Func<TSingleton>)singletons[typeof(TSingleton)])();
            yes = true;
        }
        if (yes || singletons.ContainsKey(typeof(TSingleton)))
            return (TSingleton)singletons[typeof(TSingleton)];
        throw new KeyNotFoundException("unknown interface: " + typeof(TSingleton).FullName);
    }

    public static void RegisterSingleton<TSingleton>(TSingleton singletonObject)
    {
        if (singletons.ContainsKey(typeof(TSingleton)))
        {
            singletons[typeof(TSingleton)] = singletonObject;
        }
        else
        {
            singletons.Add(typeof(TSingleton), singletonObject);
        }
    }

    public static void RegisterSingleton<TSingleton>(Func<TSingleton> singletonObject)
    {
        if (singletons.ContainsKey(typeof(TSingleton)))
        {
            singletons[typeof(TSingleton)] = singletonObject;
        }
        else
        {
            singletons.Add(typeof(TSingleton), singletonObject);
        }
    }
}
