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

namespace net.r_eg.hashing
{
    /// <summary>
    /// Based on https://github.com/3F/sandbox/tree/ff293eb3ecc6b590f7d563b6febf5e7df5f3b75f/csharp/FNV-1a
    /// </summary>
    internal static class Fnv1a
    {
        private const uint P128_B = 0x01000000, P128_D = 0x0000013B, P128_BD = 0xFFFEC5;

        public static ulong GetHash128LX4Cnh(ulong baseH, ulong baseL, string input, out ulong low)
        {
            if(input == null) throw new ArgumentNullException(nameof(input));

            ulong a = baseH >> 32, b = (uint)baseH, c = baseL >> 32, d = (uint)baseL;

            ulong f = 0, fLm = 0;
            int i = 0;
            unchecked
            {
                for(; i < input.Length; ++i)
                {
                    d ^= input[i];

                    // Below is an optimized implementation (limited) of the LX4Cnh algorithm specially for Fnv1a128
                    // (c) Denis Kuzmin <x-3F@outlook.com> github/3F

                    f = b * P128_B;

                    ulong v = (uint)f;

                    f = (f >> 32) + v;

                    if(a > b)
                    {
                        f += (uint)((a - b) * P128_B);
                    }
                    else if(a < b)
                    {
                        f -= (uint)((b - a) * P128_B);
                    }

                    ulong fHigh = (f << 32) + (uint)v;
                    ulong r2    = d * P128_D;

                    v = (r2 >> 32) + (r2 & 0xFFF_FFFF_FFFF_FFFF);

                    f = (r2 & 0xF000_0000_0000_0000) >> 32;

                    if(c > d)
                    {
                        fLm = v;
                        v += (c - d) * P128_D;
                        if(fLm > v) f += 0x100000000;
                    }
                    else if(c < d)
                    {
                        fLm = v;
                        v -= (d - c) * P128_D;
                        if(fLm < v) f -= 0x100000000;
                    }

                    fLm = (((ulong)(uint)v) << 32) + (uint)r2;

                    f = fHigh + fLm + f + (v >> 32);

                    fHigh   = (a << 32) + b; //fa
                    v       = (c << 32) + d; //fb

                    if(fHigh < v)
                    {
                        f += (v - fHigh) * P128_BD;
                    }
                    else if(fHigh > v)
                    {
                        f -= (fHigh - v) * P128_BD;
                    }

                    a = f >> 32;
                    b = (uint)f;
                    c = fLm >> 32;
                    d = (uint)fLm;
                }
            }

            if(i < 1)
            {
                low = baseL;
                return baseH;
            }

            low = fLm;
            return f;
        }
    }
}
