using QuanLib.Core.Extension;
using System.Collections.Specialized;
using System.Text;

namespace QuanLib.Java
{
    public static class JarUtil
    {
        public static NameValueCollection ParseManifest(Stream stream)
        {
            NameValueCollection result = new();
            string[] lines = stream.ToUtf8TextLines();
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                string[]? item = line.Split(": ");
                if (item.Length != 2)
                    continue;

                result.Add(item[0], item[1]);
            }

            return result;
        }
    }
}
