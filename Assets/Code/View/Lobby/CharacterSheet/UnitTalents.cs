namespace View.Lobby.CharacterSheet
{
    public class UnitTalents
    {
        public readonly bool[] reserched = new bool[] { false };
        public override string ToString()
        {
            string res = "";
            for (int i = 0; i < reserched.Length; i += sizeof(char))
            {
                res += (reserched[i..(i + sizeof(char))]);
            }
            return res;
        }
    }
}