
using System;

public static class MenuEventManager
{

    public static event Action ResourceLoaded;

    public static void InformResurceLoaded() => ResourceLoaded?.Invoke();

}
