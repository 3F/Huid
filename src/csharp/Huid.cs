/*
 * The MIT License (MIT)
 *
 * Copyright (c) 2021  Denis Kuzmin <x-3F@outlook.com> github/3F
 * Copyright (c) Huid contributors https://github.com/3F/Huid/graphs/contributors
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using System;
using net.r_eg.algorithms;

namespace net.r_eg.hashing
{
    /// <summary>
    /// High-speed a FNV-1a-128 (*LX4Cnh) hash-based UUID.
    /// </summary>
    /// <remarks>*https://github.com/3F/LX4Cnh </remarks>
    public static class Huid
    {
        public const ulong NS_HUID_H = 0x8296d91cb090857a;
        public const ulong NS_HUID_L = 0xa25cb086916b4b8a;

        /// <summary>
        /// Points to our UUID FNV-1a-128 based version.
        /// </summary>
        /// <remarks>Based on RFC 4122, '4.1.3. Version' https://www.ietf.org/rfc/rfc4122.txt</remarks>
        public const byte VERSION = 0b1000;

        /// <summary>
        /// Default namespace for <see cref="Huid"/>.
        /// </summary>
        public static readonly Guid NamespaceHuid = new("b090857a-d91c-8296-8a4b-6b9186b05ca2");

        public static readonly Guid NamespaceDNS    = new("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
        public static readonly Guid NamespaceURL    = new("6ba7b811-9dad-11d1-80b4-00c04fd430c8");
        public static readonly Guid NamespaceOID    = new("6ba7b812-9dad-11d1-80b4-00c04fd430c8");
        public static readonly Guid NamespaceX500   = new("6ba7b814-9dad-11d1-80b4-00c04fd430c8");

        /// <inheritdoc cref="NewGuid(ulong, ulong, string)"/>
        public static Guid NewGuid(string input) => NewGuid(NS_HUID_H, NS_HUID_L, input);

        /// <inheritdoc cref="NewGuid(ulong, ulong, string)"/>
        /// <param name="ns">Specified namespace.</param>
        /// <param name="input"></param>
        public static Guid NewGuid(Guid ns, string input)
        {
            if(ns == Guid.Empty) return NewGuid(NS_HUID_H, NS_HUID_L, input);

            byte[] v = ns.ToByteArray();

            // RFC 4122; '4.1.2. Layout and Byte Order'
            // ... each field encoded with the Most Significant Byte first(known as network byte order).
            // See `void format_uuid_v3or5(uuid_t *uuid, unsigned char hash[16], int v)`

            /*Eg.:
                [0xC2 0xB9 0xA2 0x30] [0xA5 0x0B] [0x45 0x24]    0xA7 0x32 0x54 0xC9 0xB4 0x2E 0x8C 0xA5

                  3    2    1    0      5    4      7    6        8    9    10   11   12   13   14   15
            */
            // We'll use BitConverter which works around the IsLittleEndian check
            return NewGuid
            (
                BitConverter.ToUInt64(v, 0),
                BitConverter.ToUInt64(v, 8), //TODO: actually here we can use any byte order
                input
            );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Guid"/> using FNV-1a-128 (LX4Cnh).
        /// </summary>
        /// <param name="high">High-order bits of a 128-bit namespace.</param>
        /// <param name="low">Low-order bits of a 128-bit namespace.</param>
        /// <param name="input">The input string for high-speed calculating the hash-based <see cref="Guid"/>.</param>
        /// <returns>A new hash-based UUID.</returns>
        public static Guid NewGuid(ulong high, ulong low, string input)
        {
            if(input == null) return Guid.Empty;

            if(input.Length > 0)
            {
                high = Fnv1a.GetHash128LX4Cnh(high, low, input, out low);
            }

            return new
            (
                (uint)high,
                (ushort)(high >> 32),

                // The version number is in the most significant 4 bits of the time stamp
                // (bits 4 through 7 of the time_hi_and_version field).
                (ushort)((((ushort)(high >> 48)) & 0x0FFF) | (VERSION << 12)),

                // low & 0xFF -> Based on RFC 4122, '4.3. Algorithm for Creating a Name-Based UUID'
                (byte)((low & 0x3F) | 0x80),
                (byte)(low >> 8 & 0xFF),

                (byte)(low >> 16 & 0xFF),
                (byte)(low >> 24 & 0xFF),
                (byte)(low >> 32 & 0xFF),
                (byte)(low >> 40 & 0xFF),
                (byte)(low >> 48 & 0xFF),
                (byte)(low >> 56 & 0xFF)
            );
        }

        /// <summary>
        /// Checks for <see cref="Guid"/> if <see cref="Huid"/> was used to generate it.
        /// </summary>
        /// <param name="g">Input instance.</param>
        public static bool IsHuid(Guid g) => GetVersionOf(g) == VERSION;

        /// <summary>
        /// Get the version which points to the algorithm that was used for generating current <see cref="Guid"/> value.
        /// </summary>
        /// <param name="g">Input instance.</param>
        public static int GetVersionOf(Guid g) => g.ToByteArray()[7] >> 4; // 6->7 network order
    }
}
