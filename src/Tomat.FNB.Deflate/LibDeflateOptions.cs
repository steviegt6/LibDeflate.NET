using System.Runtime.CompilerServices;

namespace Tomat.FNB.Deflate;

public readonly struct LibDeflateOptions()
{
    private readonly nuint size = (nuint)Unsafe.SizeOf<LibDeflateOptions>();

    public libdeflate.Malloc? Malloc { get; init; } = null;

    public libdeflate.Free? Free { get; init; } = null;
}