using UnityEngine;

public class LazyLoader : MonoBehaviour
{
    private static bool _loaded = false;

    // Start is called before the first frame update
    private void Start()
    {
        if (_loaded)
        {
            return;
        }

        _loaded = true;
        //RotW.Precache("Florence", "Resources/Prefabs/Units/Florence", ResourceType.PREFAB);
        //RotW.Precache("FlorenceCard", "Resources/Prefabs/Units/Florence", ResourceType.SPRITE);
    }
}
