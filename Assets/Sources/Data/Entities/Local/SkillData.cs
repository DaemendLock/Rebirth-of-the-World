using Combat.Common.Flags;
using Combat.Common.ValueObjects;

using UnityEngine;

namespace Data.Entities
{
    public class SkillData : MonoBehaviour//, ISkillData
    {
        [SerializeField] private int _id;
        [field: SerializeField] public SkillFlags Flags { get; private set; }

        public SkillId Id => new(_id);

        //T ISkillData.GetComponent<T>() => gameObject.GetComponent<T>();

        //bool ISkillData.TryGetComponent<T>(out T result) => gameObject.TryGetComponent(out result);
    }

    //public interface ISkillData
    //{
    //    SkillId Id { get; }
    //    SkillFlags Flags { get; }
    //    IReadOnlyCollection<IActionData> AssociatedActions { get; }

    //    T GetComponent<T>() where T : class, ISkillComponent;
    //    bool TryGetComponent<T>(out T result) where T : class, ISkillComponent;
    //}

    public interface ISkillComponent
    {

    }
}
