using System;
using System.Collections.Generic;

namespace CoCSharp.Server.Core
{
    // Write stuff to the stdout with more fanciness.

    // TODO: This should form part of CoCSharp.Server.API.
    public static partial class FancyConsole
    {
        static FancyConsole()
        {
            _colorDictionary = new Dictionary<string, ConsoleColor>();
            _colorDictionary.Add("default", ConsoleColor.White);
            _colorDictionary.Add("black", ConsoleColor.Black);
            _colorDictionary.Add("darkblue", ConsoleColor.DarkBlue);
            _colorDictionary.Add("darkgreen", ConsoleColor.DarkGreen);
            _colorDictionary.Add("darkcyan", ConsoleColor.DarkCyan);
            _colorDictionary.Add("darkred", ConsoleColor.DarkRed);
            _colorDictionary.Add("darkmagenta", ConsoleColor.DarkMagenta);
            _colorDictionary.Add("darkyellow", ConsoleColor.DarkYellow);

            _colorDictionary.Add("gray", ConsoleColor.Gray);
            _colorDictionary.Add("darkgray", ConsoleColor.DarkGray);
            _colorDictionary.Add("blue", ConsoleColor.Blue);
            _colorDictionary.Add("green", ConsoleColor.Green);
            _colorDictionary.Add("cyan", ConsoleColor.Cyan);
            _colorDictionary.Add("red", ConsoleColor.Red);
            _colorDictionary.Add("magenta", ConsoleColor.Magenta);
            _colorDictionary.Add("yellow", ConsoleColor.Yellow);
            _colorDictionary.Add("white", ConsoleColor.White);
        }

        private static Dictionary<string, ConsoleColor> _colorDictionary;

        private static List<WriteInfo> Parse(string value)
        {
            var writeInfos = new List<WriteInfo>();

            var info = new WriteInfo(Console.ForegroundColor, string.Empty);
            for (int i = 0; i < value.Length; i++)
            {
                var character = value[i];
                if (character == '&')
                {
                    // Check if the next character == '('.
                    var seek = SeekString(value, i);
                    if (seek == '(')
                    {
                        var indexClosing = value.IndexOf(')', (i + 1));
                        if (indexClosing == -1)
                        {
                            // Add '&' if the value has an opening bracket but not a closing one.
                            info.Text += '&';
                            continue;
                        }

                        var colorString = value.Substring(i + 2, indexClosing - (i + 2));

                        var color = Console.ForegroundColor;
                        if (_colorDictionary.TryGetValue(colorString, out color))
                        {
                            if (info.Text != string.Empty)
                                writeInfos.Add(info);

                            info = new WriteInfo(color, string.Empty);
                            i = indexClosing;
                            //job.Color = color;
                        }
                        else
                        {
                            // Add '&' if the value does not has a correct color value.
                            info.Text += '&';
                        }
                    }
                    else
                    {
                        // Add '&' if the value has a closing bracket but not an opening one.
                        // NOTE: Its the same thing as job.Text += character.
                        info.Text += '&';
                    }
                }
                else
                {
                    info.Text += character;
                }
            }
            writeInfos.Add(info);
            return writeInfos;
        }

        public static char SeekString(string value, int i)
        {
            if (i + 1 <= value.Length - 1)
                return value[i + 1];
            return '\0';
        }

        private class WriteInfo
        {
            public WriteInfo(ConsoleColor color, string text)
            {
                Color = color;
                Text = text;
            }

            public ConsoleColor Color { get; set; }
            public string Text { get; set; }

            public void Write()
            {
                Console.ForegroundColor = Color;
                Console.Write(Text);
            }
        }
    }
}
