using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLib.ExceptionHelper
{
    public static class MessageHelper
    {
        static MessageHelper()
        {
            ExceptionMessageMap = new();
            MessageKey[] keys = Enum.GetValues<MessageKey>();
            string[] messages = Enum.GetNames<MessageKey>();
            for (int i = 0; i < keys.Length; i++)
                ExceptionMessageMap.Add(keys[i], messages[i]);
        }

        private static Dictionary<MessageKey, string> ExceptionMessageMap;

        public static string GetMessage(MessageKey key)
        {
            return ExceptionMessageMap.ContainsKey(key) ? ExceptionMessageMap[key] : "";
        }
    }
}
