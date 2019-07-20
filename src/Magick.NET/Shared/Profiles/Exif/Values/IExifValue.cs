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

namespace ImageMagick
{
    /// <summary>
    /// A value of the exif profile.
    /// </summary>
    public interface IExifValue
    {
        /// <summary>
        /// Gets the data type of the exif value.
        /// </summary>
        ExifDataType DataType { get; }

        /// <summary>
        /// Gets a value indicating whether the value is an array.
        /// </summary>
        bool IsArray { get; }

        /// <summary>
        /// Gets the tag of the exif value.
        /// </summary>
        ExifTag Tag { get; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Tries to set the value and returns a value indicating whether the value could be set.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A value indicating whether the value could be set.</returns>
        bool TrySetValue(object value);
    }
}
