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
using ImageMagick;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Magick.NET.Tests
{
    public partial class ColorProfileTests
    {
        [TestClass]
        public class TheCopyrightProperty
        {
            [TestMethod]
            public void ShouldReturnTheCorrectValue()
            {
                Assert.AreEqual("Copyright 2000 Adobe Systems Incorporated", ColorProfile.AdobeRGB1998.Copyright);
                Assert.AreEqual("Copyright 2000 Adobe Systems Incorporated", ColorProfile.AppleRGB.Copyright);
                Assert.AreEqual("Copyright 2007 Adobe Systems, Inc.", ColorProfile.CoatedFOGRA39.Copyright);
                Assert.AreEqual("Copyright 2000 Adobe Systems Incorporated", ColorProfile.ColorMatchRGB.Copyright);
                Assert.AreEqual("Copyright (c) 1998 Hewlett-Packard Company", ColorProfile.SRGB.Copyright);
                Assert.AreEqual("Copyright 2000 Adobe Systems, Inc.", ColorProfile.USWebCoatedSWOP.Copyright);
            }

            [TestMethod]
            public void ShouldIgnoreIncorrectTagValueType()
            {
                var data = new byte[148];
                Array.Clear(data, 0, data.Length);

                // Colorspace
                data[16] = (byte)'R';
                data[17] = (byte)'G';
                data[18] = (byte)'B';

                // Tag table count
                data[131] = 1;

                // Copyright tag
                data[132] = 99;
                data[133] = 112;
                data[134] = 114;
                data[135] = 116;

                // Offset
                data[139] = 144;

                // Length
                data[143] = 1;

                // Tag value type
                data[144] = (byte)'m';
                data[145] = (byte)'l';
                data[146] = (byte)'u';
                data[147] = (byte)'c';

                var colorProfile = new ColorProfile(data);
                Assert.IsNull(colorProfile.Copyright);
            }
        }
    }
}
