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
using ImageMagick.ImageOptimizers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Magick.NET.Tests
{
    [TestClass]
    public class GifOptimizerTests : ImageOptimizerTestHelper<GifOptimizer>
    {
        [TestMethod]
        public void OptimalCompression_DefaultIsFalse()
        {
            Assert.IsFalse(Optimizer.OptimalCompression);
        }

        [TestMethod]
        public void Compress_FileIsNull_ThrowsException()
        {
            ExceptionAssert.Throws<ArgumentNullException>("file", () =>
            {
                Optimizer.Compress((FileInfo)null);
            });
        }

        [TestMethod]
        public void Compress_FileNameIsNull_ThrowsException()
        {
            ExceptionAssert.Throws<ArgumentNullException>("fileName", () =>
            {
                Optimizer.Compress((string)null);
            });
        }

        [TestMethod]
        public void Compress_FileNameIsEmpty_ThrowsException()
        {
            ExceptionAssert.Throws<ArgumentException>("fileName", () =>
            {
                Optimizer.Compress(string.Empty);
            });
        }

        [TestMethod]
        public void Compress_FileNameIsInvalid_ThrowsException()
        {
            ExceptionAssert.Throws<MagickBlobErrorException>(() =>
            {
                Optimizer.Compress(Files.Missing);
            }, "error/blob.c/OpenBlob");
        }

        [TestMethod]
        public void Compress_InvalidFile_ThrowsException()
        {
            ExceptionAssert.Throws<MagickCorruptImageErrorException>(() =>
            {
                Optimizer.Compress(Files.InvitationTIF);
            });
        }

        [TestMethod]
        public void Compress_StreamIsNull_ThrowsException()
        {
            ExceptionAssert.Throws<ArgumentNullException>("stream", () =>
            {
                Optimizer.Compress((Stream)null);
            });
        }

        [TestMethod]
        public void Compress_StreamCannotRead_ThrowsException()
        {
            using (TestStream stream = new TestStream(false, true, true))
            {
                ExceptionAssert.Throws<ArgumentException>("stream", () =>
                {
                    Optimizer.Compress(stream);
                });
            }
        }

        [TestMethod]
        public void Compress_StreamCannotWrite_ThrowsException()
        {
            using (TestStream stream = new TestStream(true, false, true))
            {
                ExceptionAssert.Throws<ArgumentException>("stream", () =>
                {
                    Optimizer.Compress(stream);
                });
            }
        }

        [TestMethod]
        public void Compress_StreamCannotSeek_ThrowsException()
        {
            using (TestStream stream = new TestStream(true, true, false))
            {
                ExceptionAssert.Throws<ArgumentException>("stream", () =>
                {
                    Optimizer.Compress(stream);
                });
            }
        }

        [TestMethod]
        public void Compress_CanCompress_FileIsSmaller()
        {
            AssertCompressSmaller(Files.FujiFilmFinePixS1ProGIF);
        }

        [TestMethod]
        public void Compress_CannotCompress_FileIsNotSmaller()
        {
            AssertCompressNotSmaller(Files.RoseSparkleGIF);
        }

        [TestMethod]
        public void Compress_CanCompress_CanBeCalledTwice()
        {
            AssertCompressTwice(Files.FujiFilmFinePixS1ProGIF);
        }

        [TestMethod]
        public void LosslessCompress_FileIsNull_ThrowsException()
        {
            ExceptionAssert.Throws<ArgumentNullException>("file", () =>
            {
                Optimizer.LosslessCompress((FileInfo)null);
            });
        }

        [TestMethod]
        public void LosslessCompress_FileNameIsNull_ThrowsException()
        {
            ExceptionAssert.Throws<ArgumentNullException>("fileName", () =>
            {
                Optimizer.LosslessCompress((string)null);
            });
        }

        [TestMethod]
        public void LosslessCompress_FileNameIsEmpty_ThrowsException()
        {
            ExceptionAssert.Throws<ArgumentException>("fileName", () =>
            {
                Optimizer.LosslessCompress(string.Empty);
            });
        }

        [TestMethod]
        public void LosslessCompress_FileNameIsInvalid_ThrowsException()
        {
            ExceptionAssert.Throws<MagickBlobErrorException>(() =>
            {
                Optimizer.LosslessCompress(Files.Missing);
            }, "error/blob.c/OpenBlob");
        }

        [TestMethod]
        public void LosslessCompress_InvalidFile_ThrowsException()
        {
            ExceptionAssert.Throws<MagickCorruptImageErrorException>(() =>
            {
                Optimizer.LosslessCompress(Files.InvitationTIF);
            });
        }

        [TestMethod]
        public void LosslessCompress_StreamIsNull_ThrowsException()
        {
            ExceptionAssert.Throws<ArgumentNullException>("stream", () =>
            {
                Optimizer.LosslessCompress((Stream)null);
            });
        }

        [TestMethod]
        public void LosslessCompress_StreamCannotRead_ThrowsException()
        {
            using (TestStream stream = new TestStream(false, true, true))
            {
                ExceptionAssert.Throws<ArgumentException>("stream", () =>
                {
                    Optimizer.LosslessCompress(stream);
                });
            }
        }

        [TestMethod]
        public void LosslessCompress_StreamCannotWrite_ThrowsException()
        {
            using (TestStream stream = new TestStream(true, false, true))
            {
                ExceptionAssert.Throws<ArgumentException>("stream", () =>
                {
                    Optimizer.LosslessCompress(stream);
                });
            }
        }

        [TestMethod]
        public void LosslessCompress_StreamCannotSeek_ThrowsException()
        {
            using (TestStream stream = new TestStream(true, true, false))
            {
                ExceptionAssert.Throws<ArgumentException>("stream", () =>
                {
                    Optimizer.LosslessCompress(stream);
                });
            }
        }

        [TestMethod]
        public void LosslessCompress_CanCompress_FileIsSmaller()
        {
            AssertLosslessCompressSmaller(Files.FujiFilmFinePixS1ProGIF);
        }

        [TestMethod]
        public void LosslessCompress_CannotCompress_FileIsNotSmaller()
        {
            AssertLosslessCompressNotSmaller(Files.RoseSparkleGIF);
        }

        [TestMethod]
        public void LosslessCompress_CanCompress_CanBeCalledTwice()
        {
            AssertCompressTwice(Files.FujiFilmFinePixS1ProGIF);
        }
    }
}
