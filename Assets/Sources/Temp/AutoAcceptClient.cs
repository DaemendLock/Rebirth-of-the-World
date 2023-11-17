using Networking.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Sources.Temp
{
    public class AutoAcceptClient : Client
    {
        public override void Dispose()
        {
            //throw new NotImplementedException();
        }

        public override void SendRequest(byte[] data)
        {
            Syncronization.CombatSyncroniaztion.Put(data);
        }
    }
}
