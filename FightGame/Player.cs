using System;

namespace FightGame
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Lives { get; set; }
        public int Power { get; set; }
        public int Gems { get; set; }
        public Gender Gender { get; set; }

        public void Status()
        {
            var genderDisplay = (Gender == Gender.Male)
                ? "Hombre"
                : "Mujer";

            ConsoleHelper.Write($"{Name.PadRight(15)}\t\t{Id}\t{Lives}\t{Power}\t{Gems}\t{genderDisplay}", 
                Lives > 0 ? ConsoleColor.White : ConsoleColor.Red);
        }
    }
}
