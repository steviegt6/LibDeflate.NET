global using static LibDeflate.Util.ObjectExtensions;

namespace LibDeflate.Util;

internal static class ObjectExtensions
{
    public static T? SetNull<T>(ref T? obj) where T : class
    {
        // ArgumentNullException.ThrowIfNull(obj);

        var original = obj;
        obj = null;
        return original;
    }
}