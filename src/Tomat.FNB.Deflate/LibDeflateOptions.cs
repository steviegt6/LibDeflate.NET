using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Tomat.FNB.Deflate;

[NativeMarshalling(typeof(Marshaller))]
public readonly struct LibDeflateOptions()
{
    [CustomMarshaller(typeof(LibDeflateOptions), MarshalMode.ManagedToUnmanagedIn, typeof(Marshaller))]
    internal static class Marshaller
    {
        public ref struct Unmanaged(nuint size, nint malloc, nint free)
        {
            private nuint size   = size;
            private nint  malloc = malloc;
            private nint  free   = free;
        }

        public static Unmanaged ConvertToUnmanaged(LibDeflateOptions managed)
        {
            return new Unmanaged(
                managed.size,
                managed.Malloc is not null ? Marshal.GetFunctionPointerForDelegate(managed.Malloc) : 0,
                managed.Free is not null ? Marshal.GetFunctionPointerForDelegate(managed.Free) : 0
            );
        }

        // public static void Free(Unmanaged unmanaged) { }
    }

    private readonly nuint size = (nuint)Unsafe.SizeOf<LibDeflateOptions>();

    public libdeflate.Malloc? Malloc { get; init; } = null;

    public libdeflate.Free? Free { get; init; } = null;
}