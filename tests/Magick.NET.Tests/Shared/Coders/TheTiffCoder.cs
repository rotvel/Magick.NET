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

using System.IO;
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Magick.NET.Tests
{
    [TestClass]
    public partial class TheTiffCoder
    {
        [TestMethod]
        public void ShouldIgnoreTheSpecifiedTags()
        {
            using (IMagickImage image = new MagickImage())
            {
                image.Settings.SetDefine(MagickFormat.Tiff, "ignore-tags", "32934");
                image.Read(Files.Coders.IgnoreTagTIF);
            }

            using (IMagickImage image = new MagickImage())
            {
                MagickReadSettings readSettings = new MagickReadSettings(new TiffReadDefines()
                {
                    IgnoreTags = new string[] { "32934" },
                });
                image.Read(Files.Coders.IgnoreTagTIF, readSettings);
            }
        }

        [TestMethod]
        public void ShouldBeAbleToReadAndWriteIptcValues()
        {
            using (IMagickImage input = new MagickImage(Files.MagickNETIconPNG))
            {
                IptcProfile profile = input.GetIptcProfile();
                Assert.IsNull(profile);

                profile = new IptcProfile();
                profile.SetValue(IptcTag.Headline, "Magick.NET");
                profile.SetValue(IptcTag.CopyrightNotice, "Copyright.NET");

                input.AddProfile(profile);

                using (MemoryStream memStream = new MemoryStream())
                {
                    input.Format = MagickFormat.Tiff;
                    input.Write(memStream);

                    memStream.Position = 0;
                    using (IMagickImage output = new MagickImage(memStream))
                    {
                        profile = output.GetIptcProfile();
                        Assert.IsNotNull(profile);
                        TestValue(profile, IptcTag.Headline, "Magick.NET");
                        TestValue(profile, IptcTag.CopyrightNotice, "Copyright.NET");
                    }
                }
            }
        }

        [TestMethod]
        public void ShouldBeAbleToWriteLzwPTiffToStream()
        {
            using (IMagickImage image = new MagickImage(Files.InvitationTIF))
            {
                image.Settings.Compression = CompressionMethod.LZW;
                using (var stream = new MemoryStream())
                {
                    image.Write(stream, MagickFormat.Ptif);
                }
            }
        }

        [TestMethod]
        public void ShouldBeAbleToUseGroup4Compression()
        {
            using (IMagickImage input = new MagickImage(Files.Builtin.Logo))
            {
                input.Settings.Compression = CompressionMethod.Group4;
                using (var stream = new MemoryStream())
                {
                    input.Write(stream, MagickFormat.Tiff);

                    stream.Position = 0;
                    using (IMagickImage output = new MagickImage(stream))
                    {
                        Assert.AreEqual(output.Compression, CompressionMethod.Group4);
                    }
                }
            }
        }

        [TestMethod]
        public void ShouldBeAbleToUseFaxCompression()
        {
            using (IMagickImage input = new MagickImage(Files.Builtin.Logo))
            {
                input.Settings.Compression = CompressionMethod.Fax;
                using (var stream = new MemoryStream())
                {
                    input.Write(stream, MagickFormat.Tiff);

                    stream.Position = 0;
                    using (IMagickImage output = new MagickImage(stream))
                    {
                        Assert.AreEqual(output.Compression, CompressionMethod.Fax);
                    }
                }
            }
        }

        private static void TestValue(IptcProfile profile, IptcTag tag, string expectedValue)
        {
            IptcValue value = profile.GetValue(tag);
            Assert.IsNotNull(value);
            Assert.AreEqual(expectedValue, value.Value);
        }
    }
}