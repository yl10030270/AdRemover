namespace AdRemover.Core
{
    public static class BlockListParser
    {
        public static async Task<HashSet<string>> Parse(string filePath)
        {
            var       result = new HashSet<string>();
            using var stream = File.OpenText(filePath);
            while (await stream.ReadLineAsync() is { } line)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;
                result.Add(line.Split()[1]);
            }

            return result;
        }
    }
}
