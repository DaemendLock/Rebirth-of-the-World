using System;

namespace Syncronization.Core
{
    public class CoreEventProvider
    {
        public event Action<int, int> AbilityCasted;

        public void InformUnitCast()
        {

        }
    }
}
