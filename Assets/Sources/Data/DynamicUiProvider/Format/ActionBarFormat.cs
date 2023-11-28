using UnityEngine;

namespace Core.DynamicUiProvider.Format
{
    public class ActionBarFormat
    {
        public virtual uint GetActionButtonFrame()
        {
            return 100;
        }
    }

    public class ActionButtonFrame
    {
        private uint _width;
        private uint _height;
        private Sprite _frame;

        public ActionButtonFrame(uint width, uint height, Sprite frame)
        {
            _width = width;
            _height = height;
            _frame = frame;
        }

        public uint Width => _width;
        public uint Height => _height;
        public Sprite Frame => _frame;
    }
}
