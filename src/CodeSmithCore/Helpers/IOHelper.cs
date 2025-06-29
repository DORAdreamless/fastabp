using System.IO;

namespace CodeSmithCore.Helpers
{
    public class IOHelper
    {
        public static void WriteToFile(string path, string content)
        {
            string dirName = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
            File.WriteAllText(path, content, System.Text.Encoding.UTF8);
        }
    }


}
