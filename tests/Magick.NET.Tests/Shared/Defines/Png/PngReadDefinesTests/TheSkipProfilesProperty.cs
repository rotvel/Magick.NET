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
using ImageMagick.Defines;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Magick.NET.Tests
{
    public partial class PngReadDefinesTests
    {
        [TestClass]
        public class TheSkipProfilesProperty
        {
            [TestMethod]
            public void ShouldNotSetDefineWhenValueIsInvalid()
            {
                using (IMagickImage image = new MagickImage())
                {
                    image.Settings.SetDefines(new PngReadDefines()
                    {
                        SkipProfiles = (ProfileTypes)64,
                    });

                    Assert.AreEqual(null, image.Settings.GetDefine("profile:skip"));
                }
            }

            [TestMethod]
            public void ShouldSetTheDefine()
            {
                using (IMagickImage image = new MagickImage())
                {
                    image.Settings.SetDefines(new PngReadDefines()
                    {
                        SkipProfiles = ProfileTypes.Icc | ProfileTypes.Iptc,
                    });

                    Assert.AreEqual("Icc,Iptc", image.Settings.GetDefine("profile:skip"));
                }
            }

            [TestMethod]
            public void ShouldSkipProfilesWhenLoadingImage()
            {
                var settings = new MagickReadSettings()
                {
                    Defines = new PngReadDefines()
                    {
                        SkipProfiles = ProfileTypes.Xmp | ProfileTypes.Exif,
                    },
                };

                using (IMagickImage image = new MagickImage())
                {
                    image.Read(Files.FujiFilmFinePixS1ProPNG);
                    Assert.IsNotNull(image.GetExifProfile());
                    Assert.IsNotNull(image.GetXmpProfile());

                    image.Read(Files.FujiFilmFinePixS1ProPNG, settings);
                    Assert.IsNull(image.GetExifProfile());
                    Assert.IsNull(image.GetXmpProfile());
                }
            }
        }
    }
}
