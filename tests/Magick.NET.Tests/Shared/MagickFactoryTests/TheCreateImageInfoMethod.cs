﻿// Copyright 2013-2019 Dirk Lemstra <https://github.com/dlemstra/Magick.NET/>
//
// Licensed under the ImageMagick License (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
//
//   https://www.imagemagick.org/script/license.php
//
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. See the License for the specific language governing permissions
// and limitations under the License.

using System;
using System.IO;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Magick.NET.Tests
{
    public partial class MagickFactoryTests
    {
        public partial class TheCreateImageInfoMethod
        {
            [TestMethod]
            public void ShouldCreateMagickImageInfo()
            {
                IMagickFactory factory = new MagickFactory();
                IMagickImageInfo info = factory.CreateImageInfo();

                Assert.IsInstanceOfType(info, typeof(MagickImageInfo));
                Assert.AreEqual(0, info.Width);
            }

            [TestClass]
            public class WithByteArray
            {
                [TestMethod]
                public void ShouldThrowExceptionWhenArrayIsNull()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentNullException>("data", () =>
                    {
                        factory.CreateImageInfo((byte[])null);
                    });
                }

                [TestMethod]
                public void ShouldThrowExceptionWhenArrayIsEmpty()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentException>("data", () =>
                    {
                        factory.CreateImageInfo(new byte[] { });
                    });
                }

                [TestMethod]
                public void ShouldCreateMagickImage()
                {
                    IMagickFactory factory = new MagickFactory();
                    var data = File.ReadAllBytes(Files.ImageMagickJPG);

                    IMagickImageInfo info = factory.CreateImageInfo(data);

                    Assert.IsInstanceOfType(info, typeof(MagickImageInfo));
                    Assert.AreEqual(123, info.Width);
                }
            }

            [TestClass]
            public class WithByteArrayAndOffset
            {
                [TestMethod]
                public void ShouldThrowExceptionWhenArrayIsNull()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentNullException>("data", () =>
                    {
                        factory.CreateImageInfo(null, 0, 0);
                    });
                }

                [TestMethod]
                public void ShouldThrowExceptionWhenArrayIsEmpty()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentException>("data", () =>
                    {
                        factory.CreateImageInfo(new byte[] { }, 0, 0);
                    });
                }

                [TestMethod]
                public void ShouldThrowExceptionWhenOffsetIsNegative()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentException>("offset", () =>
                    {
                        factory.CreateImageInfo(new byte[] { 215 }, -1, 0);
                    });
                }

                [TestMethod]
                public void ShouldThrowExceptionWhenCountIsZero()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentException>("count", () =>
                    {
                        factory.CreateImageInfo(new byte[] { 215 }, 0, 0);
                    });
                }

                [TestMethod]
                public void ShouldThrowExceptionWhenCountIsNegative()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentException>("count", () =>
                    {
                        factory.CreateImageInfo(new byte[] { 215 }, 0, -1);
                    });
                }
            }

            [TestClass]
            public class WithFileInfo
            {
                [TestMethod]
                public void ShouldThrowExceptionWhenFileInfoIsNull()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentNullException>("file", () =>
                    {
                        factory.CreateImageInfo((FileInfo)null);
                    });
                }

                [TestMethod]
                public void ShouldCreateMagickImage()
                {
                    IMagickFactory factory = new MagickFactory();
                    var file = new FileInfo(Files.ImageMagickJPG);

                    IMagickImageInfo info = factory.CreateImageInfo(file);

                    Assert.IsInstanceOfType(info, typeof(MagickImageInfo));
                    Assert.AreEqual(123, info.Width);
                }
            }

            [TestClass]
            public class WithFileName
            {
                [TestMethod]
                public void ShouldThrowExceptionWhenFileInfoIsNull()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentNullException>("fileName", () =>
                    {
                        factory.CreateImageInfo((string)null);
                    });
                }

                [TestMethod]
                public void ShouldThrowExceptionWhenFileNameIsEmpty()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentException>("fileName", () =>
                    {
                        factory.CreateImageInfo(string.Empty);
                    });
                }

                [TestMethod]
                public void ShouldCreateMagickImage()
                {
                    IMagickFactory factory = new MagickFactory();

                    IMagickImageInfo info = factory.CreateImageInfo(Files.ImageMagickJPG);

                    Assert.IsInstanceOfType(info, typeof(MagickImageInfo));
                    Assert.AreEqual(123, info.Width);
                }
            }

            [TestClass]
            public class WithStream
            {
                [TestMethod]
                public void ShouldThrowExceptionWhenStreamIsNull()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentNullException>("stream", () =>
                    {
                        factory.CreateImageInfo((Stream)null);
                    });
                }

                [TestMethod]
                public void ShouldThrowExceptionWhenStreamIsEmpty()
                {
                    IMagickFactory factory = new MagickFactory();

                    ExceptionAssert.Throws<ArgumentException>("stream", () =>
                    {
                        factory.CreateImageInfo(new MemoryStream());
                    });
                }

                [TestMethod]
                public void ShouldCreateMagickImage()
                {
                    IMagickFactory factory = new MagickFactory();

                    using (var stream = File.OpenRead(Files.ImageMagickJPG))
                    {
                        IMagickImageInfo info = factory.CreateImageInfo(stream);

                        Assert.IsInstanceOfType(info, typeof(MagickImageInfo));
                        Assert.AreEqual(123, info.Width);
                    }
                }
            }
        }
    }
}
