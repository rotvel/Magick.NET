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

using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Magick.NET.Tests
{
    public partial class UnsafePixelCollectionTests
    {
        [TestClass]
        public class TheGetPixelMethod
        {
            [TestMethod]
            public void ShouldNotThrowExceptionWhenWidthOutOfRange()
            {
                using (IMagickImage image = new MagickImage(Files.ImageMagickJPG))
                {
                    using (IPixelCollection pixels = image.GetPixelsUnsafe())
                    {
                        Pixel pixel = pixels.GetPixel(image.Width + 1, 0);
                    }
                }
            }

            [TestMethod]
            public void ShouldNotThrowExceptionWhenHeightOutOfRange()
            {
                using (IMagickImage image = new MagickImage(Files.ImageMagickJPG))
                {
                    using (IPixelCollection pixels = image.GetPixelsUnsafe())
                    {
                        Pixel pixel = pixels.GetPixel(0, image.Height + 1);
                    }
                }
            }

            [TestMethod]
            public void ShouldReturnPixelWhenIndexInsideImage()
            {
                using (IMagickImage image = new MagickImage(Files.MagickNETIconPNG))
                {
                    using (IPixelCollection pixels = image.GetPixels())
                    {
                        Pixel pixel = pixels.GetPixel(55, 68);
                        ColorAssert.AreEqual(new MagickColor("#a8dff8ff"), pixel.ToColor());
                    }
                }
            }
        }
    }
}
