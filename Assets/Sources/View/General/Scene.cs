using Unity.Jobs;
using UnityEngine.SceneManagement;

namespace View.General
{
    public struct Scene : IJob
    {
        public readonly int BuildIndex;

        public Scene(int buildIndex)
        {
            BuildIndex = buildIndex;
            IsLoading = false;
        }
        public bool IsLoading { get; private set; }

        public void Load()
        {
            JobHandle loading = this.Schedule();
        }

        public void Execute()
        {
            IsLoading = true;
            SceneManager.LoadScene(BuildIndex);
            IsLoading = false;
        }
    }
}
