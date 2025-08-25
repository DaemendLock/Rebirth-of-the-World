using Client.Combat.Domain.Units;

using DaeHitbox;

namespace Client.Combat.Presentation.Implementations.Units
{
    public class HurtboxOwner : BindableViewComponent<IUnit>, IHurtboxOwner<IUnit>
    {
        public IUnit Owner => Model;
    }
}
