using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager
{
    static class ConsoleHelper
    {
        public static void ClearRect(int x, int y, int height, int width)
        {
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write(new string(Enumerable.Repeat(' ', width).ToArray()));
            }
            Console.SetCursorPosition(x, y);
        }

        public static void PrintCenterLine(int width, int height)
        {
            for (int i = 0; i < height - 4; i++)
            {
                Console.SetCursorPosition((width / 2), i);
                Console.Write((char)9553);
            }
        }

        public static void PrintHorizontalLine(int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(i, height - 4);
                Console.Write((char)9552);
            }
        }

        public static void ShowMessage(int x, int y, string message)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(message);
            Console.ReadKey();
            ClearLine(y);
        }

        public static string GetString(int x, int y)
        {
            Console.CursorVisible = true;
            Console.SetCursorPosition(x, y);
            string readSting = Console.ReadLine();
            Console.SetCursorPosition(x, y);
            Console.CursorVisible = false;
            ClearLine(y);
            return readSting;
        }

        public static void PrintKeyInfo(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write("F1-copy F2-paste F3-rename F4-info F5-disks F6-create folder");
        }

        private static void ClearLine(int line)
        {
            Console.SetCursorPosition(0, line);
            Console.Write(new string(Enumerable.Repeat(' ', 120).ToArray()));

        }
    }
}
