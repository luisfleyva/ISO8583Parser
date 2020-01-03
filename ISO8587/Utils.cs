using System.Linq;

namespace ISO8583
{
    public static class Utils
    {
        public static string ConvertToString(this int[] intArray)
        {
            return string.Join(",",
                               intArray.Select(i => i.ToString()).ToArray());
        }
    }
}
