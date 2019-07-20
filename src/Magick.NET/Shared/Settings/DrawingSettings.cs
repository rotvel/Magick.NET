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
using System.Collections.Generic;
using System.Text;

namespace ImageMagick
{
    internal sealed partial class DrawingSettings
    {
        private double[] _strokeDashArray;

        internal DrawingSettings()
        {
            using (NativeDrawingSettings instance = new NativeDrawingSettings())
            {
                BorderColor = instance.BorderColor;
                FillColor = instance.FillColor;
                FillRule = instance.FillRule;
                Font = instance.Font;
                FontFamily = instance.FontFamily;
                FontPointsize = instance.FontPointsize;
                FontStyle = instance.FontStyle;
                FontWeight = instance.FontWeight;
                StrokeAntiAlias = instance.StrokeAntiAlias;
                StrokeColor = instance.StrokeColor;
                StrokeDashOffset = instance.StrokeDashOffset;
                StrokeLineCap = instance.StrokeLineCap;
                StrokeLineJoin = instance.StrokeLineJoin;
                StrokeMiterLimit = instance.StrokeMiterLimit;
                StrokeWidth = instance.StrokeWidth;
                TextAntiAlias = instance.TextAntiAlias;
                TextDirection = instance.TextDirection;
                TextEncoding = GetTextEncoding(instance);
                TextGravity = instance.TextGravity;
                TextInterlineSpacing = instance.TextInterlineSpacing;
                TextInterwordSpacing = instance.TextInterwordSpacing;
                TextKerning = instance.TextKerning;
                TextUnderColor = instance.TextUnderColor;
            }
        }

        public DrawableAffine Affine { get; set; }

        public MagickColor BorderColor { get; set; }

        public MagickColor FillColor { get; set; }

        public IMagickImage FillPattern { get; set; }

        public FillRule FillRule { get; set; }

        public string Font { get; set; }

        public string FontFamily { get; set; }

        public double FontPointsize { get; set; }

        public FontStyleType FontStyle { get; set; }

        public FontWeight FontWeight { get; set; }

        public bool StrokeAntiAlias { get; set; }

        public MagickColor StrokeColor { get; set; }

        public IEnumerable<double> StrokeDashArray
        {
            get => _strokeDashArray;
            set
            {
                if (value != null)
                    _strokeDashArray = new List<double>(value).ToArray();
            }
        }

        public double StrokeDashOffset { get; set; }

        public LineCap StrokeLineCap { get; set; }

        public LineJoin StrokeLineJoin { get; set; }

        public int StrokeMiterLimit { get; set; }

        public IMagickImage StrokePattern { get; set; }

        public double StrokeWidth { get; set; }

        public string Text { get; set; }

        public bool TextAntiAlias { get; set; }

        public TextDirection TextDirection { get; set; }

        public Encoding TextEncoding { get; set; }

        public Gravity TextGravity { get; set; }

        public double TextInterlineSpacing { get; set; }

        public double TextInterwordSpacing { get; set; }

        public double TextKerning { get; set; }

        public MagickColor TextUnderColor { get; set; }

        internal DrawingSettings Clone()
        {
            return new DrawingSettings
            {
                BorderColor = MagickColor.Clone(BorderColor),
                FillColor = MagickColor.Clone(FillColor),
                FillRule = FillRule,
                Font = Font,
                FontFamily = FontFamily,
                FontPointsize = FontPointsize,
                FontStyle = FontStyle,
                FontWeight = FontWeight,
                StrokeAntiAlias = StrokeAntiAlias,
                StrokeColor = MagickColor.Clone(StrokeColor),
                StrokeDashOffset = StrokeDashOffset,
                StrokeLineCap = StrokeLineCap,
                StrokeLineJoin = StrokeLineJoin,
                StrokeMiterLimit = StrokeMiterLimit,
                StrokeWidth = StrokeWidth,
                TextAntiAlias = TextAntiAlias,
                TextDirection = TextDirection,
                TextEncoding = TextEncoding,
                TextGravity = TextGravity,
                TextInterlineSpacing = TextInterlineSpacing,
                TextInterwordSpacing = TextInterwordSpacing,
                TextKerning = TextKerning,
                TextUnderColor = MagickColor.Clone(TextUnderColor),

                Affine = Affine,
                FillPattern = MagickImage.Clone(FillPattern),
                _strokeDashArray = _strokeDashArray != null ? (double[])_strokeDashArray.Clone() : null,
                StrokePattern = MagickImage.Clone(StrokePattern),
                Text = Text,
            };
        }

        private static Encoding GetTextEncoding(NativeDrawingSettings instance)
        {
            string name = instance.TextEncoding;
            if (string.IsNullOrEmpty(name))
                return null;

            try
            {
                return Encoding.GetEncoding(name);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        private INativeInstance CreateNativeInstance()
        {
            NativeDrawingSettings instance = new NativeDrawingSettings();
            instance.BorderColor = BorderColor;
            instance.FillColor = FillColor;
            instance.FillRule = FillRule;
            instance.Font = Font;
            instance.FontFamily = FontFamily;
            instance.FontPointsize = FontPointsize;
            instance.FontStyle = FontStyle;
            instance.FontWeight = FontWeight;
            instance.StrokeAntiAlias = StrokeAntiAlias;
            instance.StrokeColor = StrokeColor;
            instance.StrokeDashOffset = StrokeDashOffset;
            instance.StrokeLineCap = StrokeLineCap;
            instance.StrokeLineJoin = StrokeLineJoin;
            instance.StrokeMiterLimit = StrokeMiterLimit;
            instance.StrokeWidth = StrokeWidth;
            instance.TextAntiAlias = TextAntiAlias;
            instance.TextDirection = TextDirection;
            if (TextEncoding != null)
                instance.TextEncoding = TextEncoding.WebName;
            instance.TextGravity = TextGravity;
            instance.TextInterlineSpacing = TextInterlineSpacing;
            instance.TextInterwordSpacing = TextInterwordSpacing;
            instance.TextKerning = TextKerning;
            instance.TextUnderColor = TextUnderColor;

            if (Affine != null)
                instance.SetAffine(Affine.ScaleX, Affine.ScaleY, Affine.ShearX, Affine.ShearY, Affine.TranslateX, Affine.TranslateY);
            if (FillPattern != null)
                instance.SetFillPattern(FillPattern);
            if (_strokeDashArray != null)
                instance.SetStrokeDashArray(_strokeDashArray, _strokeDashArray.Length);
            if (StrokePattern != null)
                instance.SetStrokePattern(StrokePattern);
            if (!string.IsNullOrEmpty(Text))
                instance.SetText(Text);

            return instance;
        }
    }
}
