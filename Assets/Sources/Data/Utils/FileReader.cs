namespace Data.Utils
{
    internal static class FileReader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Unmanaged file. Dispose after using.</returns>
        public static File ReadFile(string path)
        {
            return new File(path);
        }
    }
}
