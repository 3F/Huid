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
using System.Security.Cryptography;
using System.Text;

namespace net.r_eg.hashing.Tests.Impl
{
    internal class GuidMd5OrSha1
    {
        private static readonly Guid NamespaceTest = new Guid("34652648-8BA4-4136-904A-9AFEAAC26F33");

        internal enum AlgoVersion
        {
            Md5 = 3,
            Sha1 = 5,
        }

        internal static Guid NewGuid(string input, AlgoVersion version)
        {
            const int _FMT = 16; // The UUID format is 16 octets

            if(input == null) return Guid.Empty;

            byte[] ret = new byte[_FMT];
            using(HashAlgorithm alg = (version == AlgoVersion.Md5) ? MD5.Create() : (HashAlgorithm)SHA1.Create())
            {
                byte[] ns = NamespaceTest.ToByteArray();

                alg.TransformBlock(ChangeByteOrder(ns), 0, _FMT, null, 0);

                byte[] strBytes = Encoding.UTF8.GetBytes(input);
                alg.TransformFinalBlock(strBytes, 0, strBytes.Length);

                Array.Copy(alg.Hash, 0, ret, 0, _FMT);
            }

            ret[6] &= 0x0F;
            ret[6] |= (byte)((byte)version << 4);

            ret[8] &= 0x3F;
            ret[8] |= 0x80;

            return new Guid(ChangeByteOrder(ret));
        }

        private static byte[] ChangeByteOrder(byte[] input)
        {
            /*
                [0xC2 0xB9 0xA2 0x30] [0xA5 0x0B] [0x45 0x24]    0xA7 0x32 0x54 0xC9 0xB4 0x2E 0x8C 0xA5

                    3    2    1    0      5    4      7    6        8    9    10   11   12   13   14   15
            */
            (input[0], input[3]) = (input[3], input[0]);
            (input[1], input[2]) = (input[2], input[1]);
            (input[4], input[5]) = (input[5], input[4]);
            (input[6], input[7]) = (input[7], input[6]);
            return input;
        }
    }
}
