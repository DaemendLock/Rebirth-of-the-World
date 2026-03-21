using UnityEngine;

namespace Testing.Local.Temp.Factories
{
    public class ScaleOverTimeComponent : MonoBehaviour
    {
        [field: SerializeField] public float ScaleRate { get; set; }
        [field: SerializeField] public float Duration { get; set; }

        private void Awake()
        {
            enabled = false;
        }

        private void Update()
        {
            if (Duration <= 0)
            {
                enabled = false;
                return;
            }

            Duration -= Time.deltaTime;
            transform.localScale += Vector3.one * ((ScaleRate) / 100 * Time.deltaTime);

        }
    }
}
