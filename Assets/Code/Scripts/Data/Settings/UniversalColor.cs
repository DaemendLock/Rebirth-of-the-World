#if UNITY_2017_1_OR_NEWER
using UnityEditor;
using UnityEngine;
#endif

namespace UnityReplacement {
    public class Color {
#if UNITY_2017_1_OR_NEWER
        private UnityEngine.Color _representingColor;

        public Color(UnityEngine.Color color) {
            _representingColor = color;
            r = (byte) (color.r * 255);
            g = (byte) (color.g * 255);
            b = (byte) (color.b * 255);
        }

        public readonly byte r;
        public readonly byte g;
        public readonly byte b;

        public Color(byte r, byte g, byte b) {
            _representingColor = new UnityEngine.Color(r, g, b);
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public static implicit operator UnityEngine.Color(Color color) => color._representingColor;

        public static explicit operator Color(UnityEngine.Color color) => new(color);

#else
    
    public Color(byte r, byte g, byte b) {
        this.r = r;
        this.g = g;
        this.b = b;
    }

#endif



    }
}
