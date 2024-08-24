﻿using System;
using System.Runtime.InteropServices;

namespace LibDeflate;

public sealed class ZlibCompressor(int compressionLevel) : Compressor(compressionLevel)
{
    protected override nuint CompressCore(
        ReadOnlySpan<byte> input,
        Span<byte>         output
    )
    {
        return libdeflate_zlib_compress(
            CompressorPtr,
            MemoryMarshal.GetReference(input),
            (nuint)input.Length,
            ref MemoryMarshal.GetReference(output),
            (nuint)output.Length
        );
    }

    protected override nuint GetBoundCore(
        nuint inputLength
    )
    {
        return libdeflate_zlib_compress_bound(CompressorPtr, inputLength);
    }
}