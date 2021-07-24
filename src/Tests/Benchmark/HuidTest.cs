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

using BenchmarkDotNet.Attributes;
using net.r_eg.hashing.Tests.Impl;

namespace net.r_eg.hashing.Tests
{
    public class HuidTest
    {
        /*
        |                  Method |       Mean |    Error |   StdDev |
        |------------------------ |-----------:|---------:|---------:|
        |            GuidUsingMd5 | 5,225.6 ns | 43.64 ns | 40.82 ns |
        |           GuidUsingSha1 | 5,319.7 ns | 31.59 ns | 29.55 ns |
        | GuidUsingFnv1a128LX4Cnh |   584.7 ns |  1.25 ns |  1.17 ns |
       */

        private const string MSG = "LodgeX4CorrNoHigh (LX4Cnh) algorithm of the high-speed multiplications of 128-bit numbers";

        [Benchmark]
        public void GuidUsingMd5()
        {
            _ = GuidMd5OrSha1.NewGuid(MSG, GuidMd5OrSha1.AlgoVersion.Md5);
        }

        [Benchmark]
        public void GuidUsingSha1()
        {
            _ = GuidMd5OrSha1.NewGuid(MSG, GuidMd5OrSha1.AlgoVersion.Sha1);
        }

        [Benchmark]
        public void GuidUsingFnv1a128LX4Cnh()
        {
            _ = Huid.NewGuid(MSG);
        }
    }
}
