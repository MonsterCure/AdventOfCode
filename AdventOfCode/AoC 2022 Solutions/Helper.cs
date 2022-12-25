namespace AoC_2022_Solutions
{
    internal static class Helper
    {
        public static string GetUntilOrEmpty(this string text, string stopAt = "-")
        {
            if (!String.IsNullOrWhiteSpace(text))
            {
                int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

                if (charLocation > 0) return text.Substring(0, charLocation);
            }

            return String.Empty;
        }

        public static int MaxOfMany(params int[] items)
        {
            int result = items[0];
            for (int i = 1; i < items.Length; i++) result = Math.Max(result, items[i]);
            return result;
        }

        public static V Read<K, V>(this Dictionary<K, V> dict, K key)
        {
            if (dict.ContainsKey(key)) return dict[key];
            return default(V);
        }

        public static V Read<K, V>(this Dictionary<K, V> dict, K key, V def)
        {
            if (dict.ContainsKey(key)) return dict[key];
            return def;
        }
    }

    public record Coord(int X, int Y)
    {
        public static Coord operator +(Coord a, Coord b) => new(a.X + b.X, a.Y + b.Y);
    }

    public record Coord<T>(T X, T Y);
}
