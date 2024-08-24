// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace LibDeflate.Buffers;

/// <summary>
///     An <see cref="IMemoryOwner{T}"/> implementation with an embedded length
///     and a fast <see cref="Span{T}"/> accessor.
/// </summary>
/// <typeparam name="T">
///     The type of items to store in the current instance.
/// </typeparam>
internal sealed class MemoryOwner<T> : IMemoryOwner<T>
{
    /// <summary>
    ///     The starting offset within <see cref="array"/>.
    /// </summary>
    private readonly int start;

    /// <summary>
    ///     The <see cref="ArrayPool{T}"/> instance used to rent
    ///     <see cref="array"/>.
    /// </summary>
    private readonly ArrayPool<T> pool;

    /// <summary>
    ///     The underlying <typeparamref name="T"/> array.
    /// </summary>
    private T[]? array;

    /// <summary>
    ///     Initializes a new instance of the <see cref="MemoryOwner{T}"/> class
    ///     with the specified parameters.
    /// </summary>
    /// <param name="length">The length of the new memory buffer to use.</param>
    /// <param name="pool">
    ///     The <see cref="ArrayPool{T}"/> instance to use.
    /// </param>
    /// <param name="mode">
    ///     Indicates the allocation mode to use for the new buffer to rent.
    /// </param>
    private MemoryOwner(int length, ArrayPool<T> pool, AllocationMode mode)
    {
        start       = 0;
        Length = length;
        this.pool   = pool;
        array       = pool.Rent(length);

        if (mode == AllocationMode.Clear)
        {
            array.AsSpan(0, length).Clear();
        }
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="MemoryOwner{T}"/> class
    ///     with the specified parameters.
    /// </summary>
    /// <param name="start">
    ///     The starting offset within <paramref name="array"/>.
    /// </param>
    /// <param name="length">The length of the array to use.</param>
    /// <param name="pool">
    ///     The <see cref="ArrayPool{T}"/> instance currently in use.
    /// </param>
    /// <param name="array">
    ///     The input <typeparamref name="T"/> array to use.
    /// </param>
    private MemoryOwner(int start, int length, ArrayPool<T> pool, T[] array)
    {
        this.start  = start;
        Length = length;
        this.pool   = pool;
        this.array  = array;
    }

    /// <summary>
    ///     Finalizes an instance of the <see cref="MemoryOwner{T}"/> class.
    /// </summary>
    ~MemoryOwner()
    {
        Dispose();
    }

    /// <summary>
    ///     Gets an empty <see cref="MemoryOwner{T}"/> instance.
    /// </summary>
    [Pure]
    public static MemoryOwner<T> Empty
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(0, ArrayPool<T>.Shared, AllocationMode.Default);
    }

    /// <summary>
    ///     Creates a new <see cref="MemoryOwner{T}"/> instance with the
    ///     specified parameters.
    /// </summary>
    /// <param name="size">The length of the new memory buffer to use.</param>
    /// <returns>
    ///     A <see cref="MemoryOwner{T}"/> instance of the requested length.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when <paramref name="size"/> is not valid.
    /// </exception>
    /// <remarks>
    ///     This method is just a proxy for the <see langword="private"/>
    ///     constructor, for clarity.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemoryOwner<T> Allocate(int size)
    {
        return new MemoryOwner<T>(size, ArrayPool<T>.Shared, AllocationMode.Default);
    }

    /// <summary>
    ///     Creates a new <see cref="MemoryOwner{T}"/> instance with the
    /// specified parameters.
    /// </summary>
    /// <param name="size">The length of the new memory buffer to use.</param>
    /// <param name="pool">
    ///     The <see cref="ArrayPool{T}"/> instance currently in use.
    /// </param>
    /// <returns>
    ///     A <see cref="MemoryOwner{T}"/> instance of the requested length.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when <paramref name="size"/> is not valid.
    /// </exception>
    /// <remarks>
    ///     This method is just a proxy for the <see langword="private"/>
    ///     constructor, for clarity.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemoryOwner<T> Allocate(int size, ArrayPool<T> pool)
    {
        return new MemoryOwner<T>(size, pool, AllocationMode.Default);
    }

    /// <summary>
    ///     Creates a new <see cref="MemoryOwner{T}"/> instance with the
    ///     specified parameters.
    /// </summary>
    /// <param name="size">The length of the new memory buffer to use.</param>
    /// <param name="mode">
    ///     Indicates the allocation mode to use for the new buffer to rent.
    /// </param>
    /// <returns>
    ///     A <see cref="MemoryOwner{T}"/> instance of the requested length.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when <paramref name="size"/> is not valid.
    /// </exception>
    /// <remarks>
    ///     This method is just a proxy for the <see langword="private"/>
    ///     constructor, for clarity.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemoryOwner<T> Allocate(int size, AllocationMode mode)
    {
        return new MemoryOwner<T>(size, ArrayPool<T>.Shared, mode);
    }

    /// <summary>
    ///     Creates a new <see cref="MemoryOwner{T}"/> instance with the
    ///     specified parameters.
    /// </summary>
    /// <param name="size">The length of the new memory buffer to use.</param>
    /// <param name="pool">
    ///     The <see cref="ArrayPool{T}"/> instance currently in use.
    /// </param>
    /// <param name="mode">
    ///     Indicates the allocation mode to use for the new buffer to rent.
    /// </param>
    /// <returns>
    ///     A <see cref="MemoryOwner{T}"/> instance of the requested length.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when <paramref name="size"/> is not valid.
    /// </exception>
    /// <remarks>
    ///     This method is just a proxy for the <see langword="private"/>
    ///     constructor, for clarity.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static MemoryOwner<T> Allocate(int size, ArrayPool<T> pool, AllocationMode mode)
    {
        return new MemoryOwner<T>(size, pool, mode);
    }

    /// <summary>
    ///     Gets the number of items in the current instance.
    /// </summary>
    public int Length { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; }

    /// <inheritdoc/>
    public Memory<T> Memory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (array is null)
            {
                throw new ObjectDisposedException(nameof(MemoryOwner<T>), "The current buffer has already been disposed");
            }

            return new Memory<T>(array, start, Length);
        }
    }

    /// <summary>
    ///     Gets a <see cref="Span{T}"/> wrapping the memory belonging to the
    ///     current instance.
    /// </summary>
    public Span<T> Span
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (array is null)
            {
                throw new ObjectDisposedException(nameof(MemoryOwner<T>), "The current buffer has already been disposed");
            }

            ref var r0 = ref array.DangerousGetReferenceAt(start);

            // On .NET Core runtimes, we can manually create a span from the
            // starting reference to skip the argument validations, which
            // include an explicit null check, covariance check for the array
            // and the actual validation for the starting offset and target
            // length.  We only do this on .NET Core as we can leverage the
            // runtime-specific array layout to get fast access to the initial
            // element, which makes this trick worth it.  Otherwise, on runtimes
            // where we would need to at least access a static field to retrieve
            // default Span<T> constructor and paying the cost of the extra
            // conditional branches, especially if T is a value type, in which
            // case the covariance check is JIT removed.
            return MemoryMarshal.CreateSpan(ref r0, Length);

            // return new Span<T>(array!, this.start, this.length);
        }
    }

    /// <summary>
    ///     Returns a reference to the first element within the current
    ///     instance, with no bounds check.
    /// </summary>
    /// <returns>
    ///     A reference to the first element within the current instance.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown when the buffer in use has already been disposed.
    /// </exception>
    /// <remarks>
    ///     This method does not perform bounds checks on the underlying buffer,
    ///     but does check whether the buffer itself has been disposed or not.
    ///     This check should not be removed, and it's also the reason why the
    ///     method to get a reference at a specified offset is not present.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ref T DangerousGetReference()
    {
        if (array is null)
        {
            throw new ObjectDisposedException(nameof(MemoryOwner<T>), "The current buffer has already been disposed");
        }

        return ref array.DangerousGetReferenceAt(start);
    }

    /// <summary>
    ///     Gets an <see cref="ArraySegment{T}"/> instance wrapping the
    ///     underlying <typeparamref name="T"/> array in use.
    /// </summary>
    /// <returns>
    ///     An <see cref="ArraySegment{T}"/> instance wrapping the underlying
    ///     <typeparamref name="T"/> array in use.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown when the buffer in use has already been disposed.
    /// </exception>
    /// <remarks>
    ///     This method is meant to be used when working with APIs that only
    ///     accept an array as input, and should be used with caution.  In
    ///     particular, the returned array is rented from an array pool, and it
    ///     is responsibility of the caller to ensure that it's not used after
    ///     the current <see cref="MemoryOwner{T}"/> instance is disposed.
    ///     Doing so is considered undefined behavior, as the same array might
    ///     be in use within another <see cref="MemoryOwner{T}"/> instance.
    /// </remarks>
    [Pure]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public ArraySegment<T> DangerousGetArray()
    {
        if (array is null)
        {
            throw new ObjectDisposedException(nameof(MemoryOwner<T>), "The current buffer has already been disposed");
        }

        return new ArraySegment<T>(array, start, Length);
    }

    /// <summary>
    ///     Slices the buffer currently in use and returns a new
    ///     <see cref="MemoryOwner{T}"/> instance.
    /// </summary>
    /// <param name="sliceStart">
    ///     The starting offset within the current buffer.
    /// </param>
    /// <param name="sliceLength">The length of the buffer to use.</param>
    /// <returns>
    ///     A new <see cref="MemoryOwner{T}"/> instance using the target range
    ///     of items.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    ///     Thrown when the buffer in use has already been disposed.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     Thrown when <paramref name="sliceStart"/> or
    ///     <paramref name="sliceLength"/> are not valid.
    /// </exception>
    /// <remarks>
    ///     Using this method will dispose the current instance, and should only
    ///     be used when an over-sized buffer is rented and then adjusted in
    ///     size, to avoid having to rent a new buffer of the new size and copy
    ///     the previous items into the new one, or needing an additional
    ///     variable/field to manually handle to track the used range within a
    ///     given <see cref="MemoryOwner{T}"/> instance.
    /// </remarks>
    public MemoryOwner<T> Slice(int sliceStart, int sliceLength)
    {
        if (array is null)
        {
            throw new ObjectDisposedException(nameof(MemoryOwner<T>), "The current buffer has already been disposed");
        }

        if ((uint)sliceStart > Length)
        {
            throw new ArgumentOutOfRangeException(nameof(sliceStart), "The input start parameter was not valid");
        }

        if ((uint)sliceLength > Length - sliceStart)
        {
            throw new ArgumentOutOfRangeException(nameof(sliceLength), "The input length parameter was not valid");
        }

#pragma warning disable CA1816
        // We're transferring the ownership of the underlying array, so the
        // current instance no longer needs to be disposed.  Because of this, we
        // can manually suppress the finalizer to reduce the overhead on the
        // garbage collector.
        GC.SuppressFinalize(this);
#pragma warning restore CA1816

        return new MemoryOwner<T>(sliceStart, sliceLength, pool, SetNull(ref array)!);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (array is null)
        {
            return;
        }

        pool.Return(SetNull(ref array)!);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc/>
    [Pure]
    public override string ToString()
    {
        // Normally we would throw if the array has been disposed, but in this
        // case we'll just return the non-formatted representation as a
        // fallback, since the ToString method is generally expected not to
        // throw exceptions.
        if (typeof(T) == typeof(char) && array is char[] chars)
        {
            return new string(chars, start, Length);
        }

        // Same representation used in Span<T>
        return $"Tomat.FNB.LibDeflate.Buffers.MemoryOwner<{typeof(T)}>[{Length}]";
    }
}