using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using UnityReplacement;

using static SettingsEncoder;

[Serializable]
public class Settings : ITrasferableObject {

    private string _profileName;
    //private var source;

    private GraphicsSettings _graphicsSettings;
#if UNITY_EDITOR
    [UnityEngine.SerializeField]
#endif
    private NameplatesSettings _nameplatesSettings;

    public NameplatesSettings Nameplates => _nameplatesSettings;

    private interface ILoadableSetting : ITrasferableObject {
        void FromToken(string token);
        string GenerateShareToken();

    }

    [Serializable]
    public class NameplatesSettings : ILoadableSetting
#if UNITY_EDITOR
                                      , UnityEngine.ISerializationCallbackReceiver
#endif
     { 


    //General
        public byte Height;
        public byte Width;
        [UnityEngine.Range(0, 100)]
        public byte UnselectedSizePercent;
        [UnityEngine.Range(0, 100)]
        public byte UnselectedTransparencyPercent;

        //Healthbar
        public Color AllyHealthbarColor;
        public Color EnemyHealthbarColor;

        //Castbar
#if UNITY_EDITOR
        [UnityEngine.SerializeField] private UnityEngine.Color _interruptibleCastColor;
        [UnityEngine.SerializeField] private UnityEngine.Color _notinterruptibleCastColor;
        [UnityEngine.SerializeField] private UnityEngine.Color _channelColor;
        [UnityEngine.SerializeField] private UnityEngine.Color _allyHealthbarColor;
        [UnityEngine.SerializeField] private UnityEngine.Color _enemyHealthbarColor;

        public void OnBeforeSerialize() {

        }

        public void OnAfterDeserialize() {
            AllyHealthbarColor = (Color) _allyHealthbarColor;
            EnemyHealthbarColor= (Color) _enemyHealthbarColor;
            InterruptibleCastColor = (Color) _interruptibleCastColor;
            NotinterruptibleCastColor= (Color) _notinterruptibleCastColor;
            ChannelColor = (Color) _channelColor;
        }
#endif
        public Color InterruptibleCastColor;
        public Color NotinterruptibleCastColor;
        public Color ChannelColor;
        public byte CastbarHeight;
        public byte CastNameTextSize;
        public byte CastRemainTextSize;
        public bool ShowCastingAllyWithSelectedSize;
        public bool ShowCastingEnemyWithSelectedSize;
        public bool ShowCurrentCastName;
        public bool ShowCastIcon;
        public bool ShowCastTimeRemain;

        //StatusBar
        public byte StackCountTextSize;
        public byte MaxBuffsShowAmount;
        public byte MaxDebuffsShowAmount;
        public byte BuffIconSize;
        public byte DebuffIconSize;
        public bool ShowBuffStackCount;


        public byte[] Bytes() {
            throw new System.NotImplementedException();
        }

        public ITrasferableObject FromBytes(byte[] bytes) {
            return this;
        }

        public void FromToken(string token) {
            int cursor = 0;
            Width = DecodeByteFromStringWithIndex(token, cursor);
            cursor += ENCODED_BYTE_STRING_SIZE;
            Height = DecodeByteFromStringWithIndex(token, cursor);
            cursor += ENCODED_BYTE_STRING_SIZE;
            UnselectedSizePercent = DecodeByteFromStringWithIndex(token, cursor);
            cursor += ENCODED_BYTE_STRING_SIZE;
            UnselectedTransparencyPercent = DecodeByteFromStringWithIndex(token, cursor);
            cursor += ENCODED_BYTE_STRING_SIZE;
            AllyHealthbarColor = DecodeColorFromStringWithIndex(token, cursor);
            cursor += ENCODED_COLOR_STRING_SIZE;
            EnemyHealthbarColor = DecodeColorFromStringWithIndex(token, cursor);
            cursor += ENCODED_COLOR_STRING_SIZE;
            InterruptibleCastColor = DecodeColorFromStringWithIndex(token, cursor);
            cursor += ENCODED_COLOR_STRING_SIZE;
            NotinterruptibleCastColor = DecodeColorFromStringWithIndex(token, cursor);
            cursor += ENCODED_COLOR_STRING_SIZE;
            ChannelColor = DecodeColorFromStringWithIndex(token, cursor);
            cursor += ENCODED_COLOR_STRING_SIZE;
            CastbarHeight = DecodeByteFromStringWithIndex(token, cursor);
            cursor += ENCODED_BYTE_STRING_SIZE;
            CastNameTextSize = DecodeByteFromStringWithIndex(token, cursor);
            cursor += ENCODED_BYTE_STRING_SIZE;
            CastRemainTextSize = DecodeByteFromStringWithIndex(token, cursor);
            cursor += ENCODED_BYTE_STRING_SIZE;
            BitArray boolValues = new BitArray(new byte[] { DecodeByteFromStringWithIndex(token, cursor) });
            ShowBuffStackCount = boolValues[0];
            ShowCastTimeRemain = boolValues[1];
            ShowCastIcon = boolValues[2];
            ShowCurrentCastName = boolValues[3];
            ShowCastingEnemyWithSelectedSize = boolValues[4];
            ShowCastingAllyWithSelectedSize = boolValues[5];
            cursor += ENCODED_BYTE_STRING_SIZE;
            StackCountTextSize = DecodeByteFromStringWithIndex(token, cursor);
            cursor += ENCODED_BYTE_STRING_SIZE;
            MaxBuffsShowAmount = DecodeByteFromStringWithIndex(token, cursor);
            cursor += ENCODED_BYTE_STRING_SIZE;
            MaxDebuffsShowAmount = DecodeByteFromStringWithIndex(token, cursor);
            
        }

        public string GenerateShareToken() {
            StringBuilder resultBuffer = new();
            resultBuffer.AppendJoin(
                EncodeByte(Width),
                EncodeByte(Height),
                EncodeByte(UnselectedSizePercent),
                EncodeByte(UnselectedTransparencyPercent),
                EncodeColor(AllyHealthbarColor),
                EncodeColor(EnemyHealthbarColor),
                EncodeColor(InterruptibleCastColor),
                EncodeColor(NotinterruptibleCastColor),
                EncodeColor(ChannelColor),
                EncodeByte(CastbarHeight),
                EncodeByte(CastNameTextSize),
                EncodeByte(CastRemainTextSize),
                EncodeBoolArray(new bool[] {
                    ShowCastingAllyWithSelectedSize, ShowCastingEnemyWithSelectedSize,
                    ShowCurrentCastName, ShowCastIcon, ShowCastTimeRemain,
                    ShowBuffStackCount }),
                EncodeByte(StackCountTextSize),
                EncodeByte(MaxBuffsShowAmount),
                EncodeByte(MaxDebuffsShowAmount),
                EncodeByte(BuffIconSize),
                EncodeByte(DebuffIconSize)
                );
            UnityEngine.Debug.Log(resultBuffer.ToString());
            return resultBuffer.ToString();
        }
    }

    public class InputSettings {

    }

    private class GraphicsSettings {
        public int MaxDamageNumbers;
        public bool MaxDamageNumbersFlexable;
    }

    public class ConnectionSettings {

    }

    public bool UseMaxDamageNumbers => _graphicsSettings.MaxDamageNumbersFlexable;

    public int MaxDamageNumber {
        get {
            return _graphicsSettings.MaxDamageNumbers;
        }
    }

    public Settings CopyWithName(string name) {
        throw new System.NotImplementedException();
    }



    public byte[] Bytes() {
        throw new System.NotImplementedException();
    }

    public ITrasferableObject FromBytes(byte[] bytes) {
        throw new System.NotImplementedException();
    }
}
