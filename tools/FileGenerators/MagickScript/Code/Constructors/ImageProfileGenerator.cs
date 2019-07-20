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

using System.Reflection;

namespace FileGenerator.MagickScript
{
    internal sealed class ImageProfileGenerator : ConstructorCodeGenerator
    {
        protected override string ClassName
        {
            get
            {
                return "ImageProfile";
            }
        }

        protected override bool WriteEnumerable
        {
            get
            {
                return false;
            }
        }

        protected override void WriteCall(MethodBase method, ParameterInfo[] parameters)
        {
            Write("return new ");
            Write(method.DeclaringType.Name);
            Write("(");
            WriteParameters(parameters);
            WriteLine(");");
        }

        protected override void WriteHashtableCall(MethodBase method, ParameterInfo[] parameters)
        {
            Write("return new ");
            Write(method.DeclaringType.Name);
            Write("(");
            WriteHashtableParameters(parameters);
            WriteLine(");");
        }
    }
}
