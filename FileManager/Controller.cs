using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    delegate void OnCkick(ConsoleKey key);

    class Controller
    {
        public event OnCkick KeyKlick;

        private List<ConsoleKey> userKeys = new List<ConsoleKey>() {
            ConsoleKey.UpArrow,
            ConsoleKey.DownArrow,
            ConsoleKey.Enter,
            ConsoleKey.Escape,
            ConsoleKey.Tab,
            ConsoleKey.F1,
            ConsoleKey.F2,
            ConsoleKey.F3,
            ConsoleKey.F4,
            ConsoleKey.F5,
            ConsoleKey.Delete,
            ConsoleKey.F6
        };

        public void Check()
        {
            ConsoleKey pressedKey = Console.ReadKey().Key;

            if (userKeys.Contains(pressedKey))
                KeyKlick(pressedKey);
        }
    }
}
