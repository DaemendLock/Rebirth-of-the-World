using UnityReplacement;

public static class SettingsEncoder {
    public const byte ENCODE_CHAR_FIRST_VALUE = 62;
    public const byte ENCODE_BYTE_MAXVALUE = 64;
    public const byte ENCODE_BYTE_STEP = 4;
    public const int ENCODE_FLOAT_PRECISION_MULTIPLIER = 100;
    public const int ENCODED_BYTE_STRING_SIZE = 2;
    public const int ENCODED_FLOAT_STRING_SIZE = 2;
    public const int ENCODED_COLOR_STRING_SIZE = 3 * ENCODED_BYTE_STRING_SIZE;

    public static string EncodeByte(byte encodableValue) =>
        $"{EncodeChar(encodableValue, 0)}{EncodeChar(encodableValue, 1)}";

    public static string EncodeFloat(float encodableValue) =>
        EncodeByte((byte) (encodableValue * ENCODE_FLOAT_PRECISION_MULTIPLIER));

    public static string EncodeColor(Color encodeValue) =>
        $"{EncodeByte(encodeValue.r)}{EncodeByte(encodeValue.g)}{EncodeByte(encodeValue.b)}";

    public static string EncodeBoolArray(bool[] encodeValue) {
        int encodeResult = 0;
        foreach (bool b in encodeValue) {
            encodeResult = (encodeResult << 1) | (b ? 1 : 0);
        }
        return EncodeByte((byte) encodeResult);
    }

    public static byte DecodeByteFromStringWithIndex(string decodeValue, int firstCharIndex) =>
        (byte) ((DecodeByteFromChar(decodeValue[firstCharIndex + 1]) << ENCODE_BYTE_STEP) + DecodeByteFromChar(decodeValue[firstCharIndex]));

    public static byte DecodeByteFromChar(char decodeValue) => (byte) (decodeValue - ENCODE_CHAR_FIRST_VALUE);

    public static float DecodeFloatFromStrin(string decodeValue, int firstCharIndex) => (float) DecodeByteFromStringWithIndex(decodeValue, firstCharIndex) / ENCODE_FLOAT_PRECISION_MULTIPLIER;

    public static Color DecodeColorFromStringWithIndex(string decodeValue, int firstCharIndex)
        => new Color(DecodeByteFromStringWithIndex(decodeValue, firstCharIndex),
            DecodeByteFromStringWithIndex(decodeValue, firstCharIndex + ENCODED_BYTE_STRING_SIZE),
            DecodeByteFromStringWithIndex(decodeValue, firstCharIndex + ENCODED_BYTE_STRING_SIZE + ENCODED_BYTE_STRING_SIZE));


    private static char EncodeChar(byte encodableValue, int step) =>
        (char) ((encodableValue >> (step * ENCODE_BYTE_STEP)) % ENCODE_BYTE_MAXVALUE + ENCODE_CHAR_FIRST_VALUE);

}