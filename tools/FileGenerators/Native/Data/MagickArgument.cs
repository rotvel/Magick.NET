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

using System.Runtime.Serialization;

namespace FileGenerator.Native
{
    [DataContract]
    internal sealed class MagickArgument
    {
        [DataMember(Name = "type")]
        private string _Type
        {
            get;
            set;
        }

        [OnDeserialized]
        private void Deserializated(StreamingContext context)
        {
            Type = new MagickType(_Type);
        }

        [DataMember(Name = "const")]
        public bool IsConst
        {
            get;
            set;
        }

        public bool IsEnum
        {
            get;
            private set;
        }

        public bool IsHidden
        {
            get;
            set;
        }

        [DataMember(Name = "out")]
        public bool IsOut
        {
            get;
            set;
        }

        [DataMember(Name = "name")]
        public string Name
        {
            get;
            set;
        }

        public MagickType Type
        {
            get;
            private set;
        }

        public static MagickArgument CreateException()
        {
            return new MagickArgument()
            {
                Name = "exception",
                IsHidden = true,
                IsOut = true,
                Type = new MagickType("Instance")
            };
        }
    }
}
