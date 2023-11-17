using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Editor.SpellMaker
{
    public class FileIniter : MonoBehaviour
    {
        private void Start()
        {
            File.OpenWrite(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\spells.datamap");
            File.Create(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\combatspells.data");

            

            File.Create(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\viewspells.data");
        }
    }
}
