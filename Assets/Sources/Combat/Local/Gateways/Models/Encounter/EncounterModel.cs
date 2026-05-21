using Combat.Local.Domain.Entities;

using UnityEngine.SceneManagement;

namespace Combat.Local.Gateways.Models.Encounter
{
    public sealed class EncounterModel
    {
        public EncounterModel(Domain.Entities.Encounter encounter, Scene scene, Spawnpoint[] spawnpoints)
        {
            Scene = scene;
        }

        public Scene Scene { get; }

        public Spawnpoint[] Spawnpoints { get; set; }
    }
}
