using System;
using System.Buffers;

namespace Tomat.FNB.Deflate;

public enum LibDeflateResult
{
    Success           = 0,
    BadData           = 1,
    ShortOutput       = 2,
    InsufficientSpace = 3,
}

public static class LibDeflateResultExtensions
{
    public static OperationStatus ToStatus(this LibDeflateResult @this)
    {
        return @this switch
        {
            LibDeflateResult.Success           => OperationStatus.Done,
            LibDeflateResult.BadData           => OperationStatus.InvalidData,
            LibDeflateResult.ShortOutput       => OperationStatus.NeedMoreData,
            LibDeflateResult.InsufficientSpace => OperationStatus.DestinationTooSmall,
            _                                  => throw new ArgumentOutOfRangeException(nameof(@this), @this, null),
        };
    }
}