using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.ConsoleUtil
{
    public class ConsoleText
    {
        public ConsoleText()
        {
            Text = string.Empty;
            Color = new();
        }

        public ConsoleText(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Color = TextColor.Now;
        }

        public ConsoleText(string text, TextColor color)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
            Color = color;
        }

        public string Text { get; set; }

        public TextColor Color { get; set; }

        public static ConsoleText Space => new(" ", TextColor.Now);

        public static ConsoleText Wrap => new("\n", TextColor.Now);

        public void Write()
        {
            TextColor def = TextColor.Now;
            Color.SetToConsole();
            Console.Write(Text);
            def.SetToConsole();
        }

        public void WriteLine()
        {
            Write();
            Console.WriteLine();
        }

        public static void WriteAll(ConsoleText[] texts)
        {
            if (texts is null)
                throw new ArgumentNullException(nameof(texts));

            TextColor def = TextColor.Now;
            foreach (ConsoleText text in texts)
            {
                text.Color.SetToConsole();
                Console.Write(text.Text);
            }
            def.SetToConsole();
        }

        public static void WriteLineAll(ConsoleText[] texts)
        {
            WriteAll(texts);
            Console.WriteLine();
        }

        public static void WriteAll(ConsoleText[] texts, char separator)
        {
            if (texts is null)
                throw new ArgumentNullException(nameof(texts));

            if (texts.Length == 0)
                return;

            TextColor def = TextColor.Now;
            texts[0].Color.SetToConsole();
            Console.Write(texts[0].Text);

            if (texts.Length == 1)
                goto end;

            Console.Write(separator);

            for (int i = 1; i < texts.Length; i++)
            {
                def.SetToConsole();
                Console.Write(separator);
                texts[i].Color.SetToConsole();
                Console.Write(texts[i].Text);
            }

            end:
            def.SetToConsole();
        }

        public static void WriteLineAll(ConsoleText[] texts, char separator)
        {
            WriteAll(texts, separator);
            Console.WriteLine();
        }
    }
}
