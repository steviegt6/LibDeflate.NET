using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace Tomat.FNB.Deflate;

[NativeMarshalling(typeof(Marshaller))]
public readonly unsafe struct LibDeflateOptions
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
                managed.GetMallocPtr(),
                managed.GetFreePtr()
            );
        }

        // We don't do any freeing ourselves.  Only actual memory to free comes
        // in the form of function pointers, which generally shouldn't be freed.
        // Function pointers from delegates will be taken care of and pointers
        // to native functions shouldn't be freed at all by us.
        // public static void Free(Unmanaged unmanaged) { }
    }

    // Use size from unmanaged representation.  Ours is larger because we store
    // both the native function pointer and managed delegate reference depending
    // on how the struct was constructed.
    private readonly nuint size = (nuint)sizeof(Marshaller.Unmanaged);

    public libdeflate.Malloc? MallocDelegate { get; }

    public libdeflate.Free? FreeDelegate { get; }

    private readonly nint mallocFPtr;
    private readonly nint freeFPtr;

    public LibDeflateOptions() { }

    public LibDeflateOptions(libdeflate.Malloc? malloc, libdeflate.Free? free)
    {
        MallocDelegate = malloc;
        FreeDelegate   = free;
    }

    public LibDeflateOptions(nint malloc, nint free)
    {
        mallocFPtr = malloc;
        freeFPtr   = free;

        if (mallocFPtr != 0)
        {
            MallocDelegate = Marshal.GetDelegateForFunctionPointer<libdeflate.Malloc>(mallocFPtr);
        }

        if (freeFPtr != 0)
        {
            FreeDelegate = Marshal.GetDelegateForFunctionPointer<libdeflate.Free>(freeFPtr);
        }
    }

    public LibDeflateOptions(libdeflate.Malloc malloc, nint free)
    {
        MallocDelegate = malloc;
        freeFPtr       = free;

        if (freeFPtr != 0)
        {
            FreeDelegate = Marshal.GetDelegateForFunctionPointer<libdeflate.Free>(freeFPtr);
        }
    }

    public LibDeflateOptions(nint malloc, libdeflate.Free free)
    {
        mallocFPtr   = malloc;
        FreeDelegate = free;

        if (mallocFPtr != 0)
        {
            MallocDelegate = Marshal.GetDelegateForFunctionPointer<libdeflate.Malloc>(mallocFPtr);
        }
    }

    private nint GetMallocPtr()
    {
        return mallocFPtr != 0 ? mallocFPtr : MallocDelegate is null ? 0 : Marshal.GetFunctionPointerForDelegate(MallocDelegate);
    }

    private nint GetFreePtr()
    {
        return freeFPtr != 0 ? freeFPtr : FreeDelegate is null ? 0 : Marshal.GetFunctionPointerForDelegate(FreeDelegate);
    }
}