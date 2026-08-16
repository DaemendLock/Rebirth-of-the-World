using Combat.Common.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Combat.Local.Domain.Entities.Characters
{
    public readonly ref struct ItemOwner
    {
        public readonly UnitId Id { get; }

        public readonly int ItemId { get; }
    }
}
