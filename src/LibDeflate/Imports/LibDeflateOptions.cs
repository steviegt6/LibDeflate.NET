using System.Runtime.CompilerServices;

namespace LibDeflate.Imports;

internal readonly struct LibDeflateOptions()
{
    private readonly nuint size = (nuint)Unsafe.SizeOf<LibDeflateOptions>();

    public Library.Malloc? Malloc { get; init; } = null;

    public Library.Free? Free { get; init; } = null;
}