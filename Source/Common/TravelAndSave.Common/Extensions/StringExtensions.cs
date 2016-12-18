namespace TravelAndSave.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceLastOccurrence(this string source, string oldValue, string newValue)
        {
            int place = source.LastIndexOf(oldValue);

            if (place == -1)
            {
                return string.Empty;
            }

            string result = source.Remove(place, oldValue.Length).Insert(place, newValue);

            return result;
        }
    }
}
