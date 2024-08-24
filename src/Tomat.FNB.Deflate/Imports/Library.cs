global using static LibDeflate.Imports.Library;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LibDeflate.Imports;

internal static partial class Library
{
    private const string dll_name = "libdeflate";

#region Compression
    // ReSharper disable InconsistentNaming
    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint libdeflate_alloc_compressor(
        int compressionLevel
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint libdeflate_alloc_compressor_ex(
        int                  compressionLevel,
        in LibDeflateOptions options
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint libdeflate_deflate_compress(
        nint     pCompressor,
        in byte  inBytes,
        nuint    inBytesCount,
        ref byte outBytes,
        nuint    outBytesCount
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint libdeflate_deflate_compress_bound(
        nint  pCompressor,
        nuint nBytesCount
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint libdeflate_zlib_compress(
        nint     pCompressor,
        in byte  inBytes,
        nuint    inBytesCount,
        ref byte outBytes,
        nuint    outBytesCount
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint libdeflate_zlib_compress_bound(
        nint  pCompressor,
        nuint inBytesCount
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint libdeflate_gzip_compress(
        nint     pCompressor,
        in byte  inBytes,
        nuint    inBytesCount,
        ref byte outBytes,
        nuint    outBytesCount
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nuint libdeflate_gzip_compress_bound(
        nint  pCompressor,
        nuint inBytesCount
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void libdeflate_free_compressor(
        nint pCompressor
    );
    // ReSharper restore InconsistentNaming
#endregion

#region Decompression
    // ReSharper disable InconsistentNaming
    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint libdeflate_alloc_decompressor();

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial nint libdeflate_alloc_decompressor_ex(
        in LibDeflateOptions options
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial LibDeflateResult libdeflate_deflate_decompress(
        nint      pDecompressor,
        in byte   inBytes,
        nuint     inBytesCount,
        ref byte  outBytes,
        nuint     outBytesCountExpected,
        out nuint outBytesCountActual
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial LibDeflateResult libdeflate_deflate_decompress_ex(
        nint      pDecompressor,
        in byte   inBytes,
        nuint     inBytesCountExpected,
        ref byte  outBytes,
        nuint     outBytesCountExpected,
        out nuint inBytesCountActual,
        out nuint outBytesCountActual
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial LibDeflateResult libdeflate_zlib_decompress(
        nint      pDecompressor,
        in byte   inBytes,
        nuint     inBytesCount,
        ref byte  outBytes,
        nuint     outBytesCountExpected,
        out nuint outBytesCountActual
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial LibDeflateResult libdeflate_zlib_decompress_ex(
        nint      pDecompressor,
        in byte   inBytes,
        nuint     inBytesCountExpected,
        ref byte  outBytes,
        nuint     outBytesCountExpected,
        out nuint inBytesCountActual,
        out nuint outBytesCountActual
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial LibDeflateResult libdeflate_gzip_decompress(
        nint      pDecompressor,
        in byte   inBytes,
        nuint     inBytesCount,
        ref byte  outBytes,
        nuint     outBytesCountExpected,
        out nuint outBytesCountActual
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial LibDeflateResult libdeflate_gzip_decompress_ex(
        nint      pDecompressor,
        in byte   inBytes,
        nuint     inBytesCount,
        ref byte  outBytesCount,
        nuint     outBytesCountExpected,
        out nuint inBytesCountActual,
        out nuint outBytesCountActual
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void libdeflate_free_decompressor(
        nint pDecompressor
    );
    // ReSharper restore InconsistentNaming
#endregion

#region Checksums
    // TODO: Unimplemented.
#endregion

#region Custom memory allocator
    // ReSharper disable InconsistentNaming
    // TODO: UnmanagedFunctionPointer?
    public delegate nint Malloc(
        nuint size
    );

    // TODO: UnmanagedFunctionPointer?
    public delegate void Free(
        nint alloc
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void libdeflate_set_memory_allocator(
        Malloc malloc,
        Free   free
    );
    // ReSharper restore InconsistentNaming
#endregion
}