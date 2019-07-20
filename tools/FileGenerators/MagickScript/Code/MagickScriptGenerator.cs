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

namespace FileGenerator.MagickScript
{
    internal sealed class MagickScriptGenerator
    {
        private string _OutputFolder;
        private MagickScriptTypes _Types;

        private MagickScriptGenerator()
        {
            _OutputFolder = SetOutputFolder(@"src\Magick.NET\Shared\Script\Generated");
            _Types = new MagickScriptTypes(QuantumDepth.Q16HDRI);
        }

        private void Cleanup()
        {
            foreach (string fileName in Directory.GetFiles(_OutputFolder))
            {
                File.Delete(fileName);
            }
        }

        private void CreateCodeFile(ScriptCodeGenerator generator)
        {
            string outputFile = Path.GetFullPath(_OutputFolder + @"\" + generator.Name + ".cs");
            generator.CreateWriter(outputFile);
            generator.Write(_Types);
            generator.CloseWriter();
        }

        private static string SetOutputFolder(string outputFolder)
        {
            string result = PathHelper.GetFullPath(outputFolder);
            if (result[result.Length - 1] != '\\')
                result += "\\";

            return result;
        }

        private void WriteCollection()
        {
            CreateCodeFile(new CollectionGenerator());
        }

        private void WriteConstructors()
        {
            CreateCodeFile(new ColorProfileGenerator());
            CreateCodeFile(new ImageProfileGenerator());
            CreateCodeFile(new PathArcGenerator());
            CreateCodeFile(new PointDGenerator());
            CreateCodeFile(new PrimaryInfoGenerator());
            CreateCodeFile(new SparseColorArg());
        }

        private void WriteExecute()
        {
            CreateCodeFile(new DrawableGenerator());
            CreateCodeFile(new PathsGenerator());
            CreateCodeFile(new MagickImageCollectionGenerator());
            CreateCodeFile(new MagickImageGenerator());
            CreateCodeFile(new MagickSettingsGenerator());
            CreateCodeFile(new MagickReadSettingsGenerator());
        }

        private void WriteInterfaces()
        {
            CreateCodeFile(new IDefinesGenerator());
        }

        private void WriteSettings()
        {
            CreateCodeFile(new DistortSettingsGenerator());
            CreateCodeFile(new MagickSettingsGenerator());
            CreateCodeFile(new MontageSettingsGenerator());
            CreateCodeFile(new MorphologySettingsGenerator());
            CreateCodeFile(new PixelReadSettingsGenerator());
            CreateCodeFile(new QuantizeSettingsGenerator());
        }

        public static void Generate()
        {
            MagickScriptGenerator generator = new MagickScriptGenerator();

            generator.Cleanup();

            generator.WriteCollection();
            generator.WriteConstructors();
            generator.WriteExecute();
            generator.WriteInterfaces();
            generator.WriteSettings();
        }
    }
}
