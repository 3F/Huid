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
using Xunit;

namespace net.r_eg.hashing.Tests
{
    public class UnitTest
    {
        [Theory]
        [InlineData("Hello World!")]
        [InlineData("")]
        [InlineData(" ")]
        public void Test1(string input)
        {
            Guid g = Huid.NewGuid(input);

            Assert.Equal(g, Huid.NewGuid(0x8296d91cb090857a, 0xa25cb086916b4b8a, input));
            Assert.Equal(g, Huid.NewGuid(new Guid("{b090857a-d91c-8296-8a4b-6b9186b05ca2}"), input));
        }

        [Fact]
        public void Test2()
        {
            Assert.True(Huid.NamespaceHuid == Huid.NewGuid(string.Empty));
            Assert.True(Huid.NamespaceHuid == Huid.NewGuid(Huid.NS_HUID_H, Huid.NS_HUID_L, string.Empty));
            Assert.True(Huid.NamespaceHuid == Huid.NewGuid(Huid.NamespaceHuid, string.Empty));

            Assert.True(Guid.Empty == Huid.NewGuid(null));
            Assert.True(Guid.Empty == Huid.NewGuid(0, 0, null));
            Assert.True(Guid.Empty == Huid.NewGuid(Huid.NamespaceHuid, null));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal(Huid.NewGuid(string.Empty), Huid.NewGuid(Guid.Empty, string.Empty));
            Assert.Equal(Huid.NewGuid("123"), Huid.NewGuid(Guid.Empty, "123"));
        }

        [Fact]
        public void DefTest1()
        {
            Assert.True(new Guid("2eefb8ac-3b3e-89cc-892a-ad0304640484") == Huid.NewGuid("Hello World!"));
            Assert.True(new Guid("b090857a-d91c-8296-8a4b-6b9186b05ca2") == Huid.NewGuid(string.Empty));
            Assert.True(new Guid("ebd43de5-9198-862e-ae1a-06ef94350dc8") == Huid.NewGuid(" "));

            Assert.True(new Guid("ebd43de5-9198-862e-ae1a-06ef94350dc8") == Huid.NewGuid(Guid.Empty, " "));
        }

        [Fact]
        public void DefTest2()
        {
            Assert.True(new Guid("1bf116a1-2282-83da-bf7e-c8c3dfb04978") == Huid.NewGuid(Huid.NamespaceDNS, "Hello World!"));
            Assert.True(new Guid("6ba7b810-9dad-81d1-80b4-00c04fd430c8") == Huid.NewGuid(Huid.NamespaceDNS, string.Empty));
            Assert.True(new Guid("175f7ca6-0518-8cad-a040-de40213e1554") == Huid.NewGuid(Huid.NamespaceDNS, " "));
        }

        [Fact]
        public void DefTest3()
        {
            Guid ns = new Guid("5BDA9893-6F75-4973-877E-E817D6262603");

            Assert.True(new Guid("fc542e7e-756c-8425-94c7-32db1dae20af") == Huid.NewGuid(ns, "Hello World!"));
            Assert.True(new Guid("5bda9893-6f75-8973-877e-e817d6262603") == Huid.NewGuid(ns, string.Empty));
            Assert.True(new Guid("acf9bce4-0de6-8722-bdd7-136b6fc9f1df") == Huid.NewGuid(ns, " "));
        }

        [Fact]
        public void VersionTest1()
        {
            Assert.False(Huid.IsHuid(Huid.NamespaceDNS));
            Assert.True(Huid.IsHuid(Huid.NamespaceHuid));
        }

        [Fact]
        public void VersionTest2()
        {
            Guid g1 = new Guid("B0D6D183-2D18-4C27-A723-9BA7E5D7B857");
            Guid g2 = Huid.NewGuid(g1, string.Empty);

            Assert.False(Huid.IsHuid(g1));
            Assert.True(Huid.IsHuid(g2));
        }

        [Fact]
        public void VersionTest3()
        {
            Assert.Equal(1, Huid.GetVersionOf(Huid.NamespaceDNS));
            Assert.Equal(Huid.VERSION, Huid.GetVersionOf(Huid.NamespaceHuid));
            Assert.Equal(4, Huid.GetVersionOf(Guid.NewGuid()));
        }
    }
}
