using UnityEngine;

public class PrecacheScript : MonoBehaviour
{
    


    private class PrecacheObject {
        public string name;
        public string resource;
        public ResourceType type;
        public PrecacheObject(string name, GameObject gameObject, ResourceType type) {
            RotW.Precache(name, resource, type);
        }
    }
}
