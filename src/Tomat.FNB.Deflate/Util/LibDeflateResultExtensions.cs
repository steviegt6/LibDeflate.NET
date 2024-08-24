using System;
using System.Buffers;

using LibDeflate.Imports;

namespace LibDeflate.Util;

internal static class LibDeflateResultExtensions
{
    internal static OperationStatus ToStatus(this LibDeflateResult @this)
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