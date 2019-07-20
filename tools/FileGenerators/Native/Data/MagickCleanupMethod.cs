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

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FileGenerator.Native
{
    [DataContract]
    internal sealed class MagickCleanupMethod
    {
        [DataMember(Name = "arguments")]
        private List<string> _Arguments = new List<string>();

        [DataMember(Name = "name")]
        public string Name
        {
            get;
            set;
        }

        public IEnumerable<string> Arguments
        {
            get
            {
                if (_Arguments != null)
                {
                    foreach (var argument in _Arguments)
                    {
                        yield return argument;
                    }
                }
            }
        }
    }
}
