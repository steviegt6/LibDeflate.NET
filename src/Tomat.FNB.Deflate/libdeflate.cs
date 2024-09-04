using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace Tomat.FNB.Deflate;

#pragma warning disable CS8981
#pragma warning disable CA1401
public static partial class libdeflate
{
    private const string dll_name = "libdeflate";

#region Compression
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
#endregion

#region Decompression
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
#endregion

#region Checksums
    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint libdeflate_adler32(
        uint    adler,
        in byte buffer,
        nuint   len
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial uint libdeflate_crc32(
        uint    crc,
        in byte buffer,
        nuint   len
    );
#endregion

#region Custom memory allocator
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate nint Malloc(
        nuint size
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void Free(
        nint alloc
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void libdeflate_set_memory_allocator(
        Malloc malloc,
        Free   free
    );

    [LibraryImport(dll_name)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static unsafe partial void libdeflate_set_memory_allocator(
        delegate* unmanaged[Cdecl]<nuint, void*> malloc,
        delegate* unmanaged[Cdecl]<void*, void>  free
    );
#endregion
}
#pragma warning restore CS8981
#pragma warning restore CA1401