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
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ImageMagick
{
    /// <summary>
    /// Represents the collection of images.
    /// </summary>
    public sealed partial class MagickImageCollection : IMagickImageCollection
    {
        private readonly List<IMagickImage> _images;
        private readonly NativeMagickImageCollection _nativeInstance;

        private EventHandler<WarningEventArgs> _warning;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        public MagickImageCollection()
        {
            _images = new List<IMagickImage>();
            _nativeInstance = new NativeMagickImageCollection();
            _nativeInstance.Warning += OnWarning;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickImageCollection(byte[] data)
            : this()
        {
            Read(data);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <param name="offset">The offset at which to begin reading data.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickImageCollection(byte[] data, int offset, int count)
            : this()
        {
            Read(data, offset, count);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <param name="offset">The offset at which to begin reading data.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickImageCollection(byte[] data, int offset, int count, MagickReadSettings readSettings)
            : this()
        {
            Read(data, offset, count, readSettings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickImageCollection(byte[] data, MagickReadSettings readSettings)
            : this()
        {
            Read(data, readSettings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        /// <param name="file">The file to read the image from.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickImageCollection(FileInfo file)
            : this()
        {
            Read(file);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        /// <param name="file">The file to read the image from.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickImageCollection(FileInfo file, MagickReadSettings readSettings)
            : this()
        {
            Read(file, readSettings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        /// <param name="images">The images to add to the collection.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickImageCollection(IEnumerable<IMagickImage> images)
            : this()
        {
            AddRange(images);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        /// <param name="stream">The stream to read the image data from.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickImageCollection(Stream stream)
            : this()
        {
            Read(stream);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        /// <param name="stream">The stream to read the image data from.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickImageCollection(Stream stream, MagickReadSettings readSettings)
            : this()
        {
            Read(stream, readSettings);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickImageCollection(string fileName)
            : this()
        {
            Read(fileName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickImageCollection(string fileName, MagickReadSettings readSettings)
            : this()
        {
            Read(fileName, readSettings);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MagickImageCollection"/> class.
        /// </summary>
        ~MagickImageCollection()
        {
            Dispose(false);
        }

        /// <summary>
        /// Event that will we raised when a warning is thrown by ImageMagick.
        /// </summary>
        public event EventHandler<WarningEventArgs> Warning
        {
            add => _warning += value;
            remove => _warning -= value;
        }

        /// <summary>
        /// Gets the number of images in the collection.
        /// </summary>
        public int Count => _images.Count;

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// Gets or sets the image at the specified index.
        /// </summary>
        /// <param name="index">The index of the image to get.</param>
        public IMagickImage this[int index]
        {
            get => _images[index];
            set
            {
                if (value == null)
                    throw new InvalidOperationException("Not allowed to add null value.");

                if (!ReferenceEquals(value, _images[index]))
                    CheckDuplication(value);

                _images[index] = value;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>An enumerator that iterates through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator() => _images.GetEnumerator();

        /// <summary>
        /// Adds an image to the collection.
        /// </summary>
        /// <param name="item">The image to add.</param>
        public void Add(IMagickImage item)
        {
            Throw.IfNull(nameof(item), item);

            CheckDuplication(item);

            _images.Add(item);
        }

        /// <summary>
        /// Adds an image with the specified file name to the collection.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Add(string fileName) => _images.Add(new MagickImage(fileName));

        /// <summary>
        /// Adds the image(s) from the specified byte array to the collection.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void AddRange(byte[] data) => AddRange(data, null);

        /// <summary>
        /// Adds the image(s) from the specified byte array to the collection.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void AddRange(byte[] data, MagickReadSettings readSettings)
        {
            Throw.IfNullOrEmpty(nameof(data), data);

            AddImages(data, 0, data.Length, readSettings, false);
        }

        /// <summary>
        /// Adds the specified images to this collection.
        /// </summary>
        /// <param name="images">The images to add to the collection.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void AddRange(IEnumerable<IMagickImage> images)
        {
            Throw.IfNull(nameof(images), images);
            Throw.IfTrue(nameof(images), images is MagickImageCollection, "Not allowed to add collection.");

            foreach (IMagickImage image in images)
            {
                Add(image);
            }
        }

        /// <summary>
        /// Adds the image(s) from the specified file name to the collection.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void AddRange(string fileName) => AddRange(fileName, null);

        /// <summary>
        /// Adds the image(s) from the specified file name to the collection.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void AddRange(string fileName, MagickReadSettings readSettings) => AddImages(fileName, readSettings, false);

        /// <summary>
        /// Adds the image(s) from the specified stream to the collection.
        /// </summary>
        /// <param name="stream">The stream to read the images from.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void AddRange(Stream stream) => AddRange(stream, null);

        /// <summary>
        /// Adds the image(s) from the specified stream to the collection.
        /// </summary>
        /// <param name="stream">The stream to read the images from.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void AddRange(Stream stream, MagickReadSettings readSettings) => AddImages(stream, readSettings, false);

        /// <summary>
        /// Creates a single image, by appending all the images in the collection horizontally (+append).
        /// </summary>
        /// <returns>A single image, by appending all the images in the collection horizontally (+append).</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public IMagickImage AppendHorizontally()
        {
            ThrowIfEmpty();

            try
            {
                AttachImages();
                IntPtr image = _nativeInstance.Append(_images[0], false);
                return MagickImage.Create(image, _images[0].Settings);
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Creates a single image, by appending all the images in the collection vertically (-append).
        /// </summary>
        /// <returns>A single image, by appending all the images in the collection vertically (-append).</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public IMagickImage AppendVertically()
        {
            ThrowIfEmpty();

            try
            {
                AttachImages();
                IntPtr image = _nativeInstance.Append(_images[0], true);
                return MagickImage.Create(image, _images[0].Settings);
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Merge a sequence of images. This is useful for GIF animation sequences that have page
        /// offsets and disposal methods.
        /// </summary>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Coalesce()
        {
            ThrowIfEmpty();

            MagickSettings settings = _images[0].Settings.Clone();

            IntPtr images;
            try
            {
                AttachImages();
                images = _nativeInstance.Coalesce(_images[0]);
            }
            finally
            {
                DetachImages();
            }

            Clear();
            foreach (MagickImage image in MagickImage.CreateList(images, settings))
                Add(image);
        }

        /// <summary>
        /// Removes all images from the collection.
        /// </summary>
        public void Clear()
        {
            foreach (MagickImage image in _images)
            {
                if (image != null)
                    image.Dispose();
            }

            _images.Clear();
        }

        /// <summary>
        /// Creates a clone of the current image collection.
        /// </summary>
        /// <returns>A clone of the current image collection.</returns>
        public IMagickImageCollection Clone()
        {
            IMagickImageCollection result = new MagickImageCollection();
            foreach (MagickImage image in this)
                result.Add(image.Clone());

            return result;
        }

        /// <summary>
        /// Combines the images into a single image. The typical ordering would be
        /// image 1 => Red, 2 => Green, 3 => Blue, etc.
        /// </summary>
        /// <returns>The images combined into a single image.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public IMagickImage Combine() => Combine(ColorSpace.sRGB);

        /// <summary>
        /// Combines the images into a single image. The grayscale value of the pixels of each image
        /// in the sequence is assigned in order to the specified channels of the combined image.
        /// The typical ordering would be image 1 => Red, 2 => Green, 3 => Blue, etc.
        /// </summary>
        /// <param name="colorSpace">The image colorspace.</param>
        /// <returns>The images combined into a single image.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public IMagickImage Combine(ColorSpace colorSpace)
        {
            ThrowIfEmpty();

            try
            {
                AttachImages();
                IntPtr image = _nativeInstance.Combine(_images[0], colorSpace);
                return MagickImage.Create(image, _images[0].Settings);
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Determines whether the collection contains the specified image.
        /// </summary>
        /// <param name="item">The image to check.</param>
        /// <returns>True when the collection contains the specified image.</returns>
        public bool Contains(IMagickImage item) => _images.Contains(item);

        /// <summary>
        /// Copies the images to an Array, starting at a particular Array index.
        /// </summary>
        /// <param name="array">The one-dimensional Array that is the destination.</param>
        /// <param name="arrayIndex">The zero-based index in 'destination' at which copying begins.</param>
        public void CopyTo(IMagickImage[] array, int arrayIndex)
        {
            if (_images.Count == 0)
                return;

            Throw.IfNull(nameof(array), array);
            Throw.IfOutOfRange(nameof(arrayIndex), arrayIndex, _images.Count);
            Throw.IfOutOfRange(nameof(arrayIndex), arrayIndex, array.Length);

            int indexI = 0;
            int length = Math.Min(array.Length, _images.Count);
            for (int indexA = arrayIndex; indexA < length; indexA++)
            {
                array[indexA] = _images[indexI++].Clone();
            }
        }

        /// <summary>
        /// Break down an image sequence into constituent parts. This is useful for creating GIF or
        /// MNG animation sequences.
        /// </summary>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Deconstruct()
        {
            ThrowIfEmpty();

            MagickSettings settings = _images[0].Settings.Clone();

            IntPtr images;
            try
            {
                AttachImages();
                images = _nativeInstance.Deconstruct(_images[0]);
            }
            finally
            {
                DetachImages();
            }

            Clear();
            foreach (IMagickImage image in MagickImage.CreateList(images, settings))
                Add(image);
        }

        /// <summary>
        /// Disposes the <see cref="MagickImageCollection"/> instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Evaluate image pixels into a single image. All the images in the collection must be the
        /// same size in pixels.
        /// </summary>
        /// <param name="evaluateOperator">The operator.</param>
        /// <returns>The resulting image of the evaluation.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public IMagickImage Evaluate(EvaluateOperator evaluateOperator)
        {
            ThrowIfEmpty();

            try
            {
                AttachImages();
                IntPtr image = _nativeInstance.Evaluate(_images[0], evaluateOperator);
                return MagickImage.Create(image, _images[0].Settings);
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Use the virtual canvas size of first image. Images which fall outside this canvas is clipped.
        /// This can be used to 'fill out' a given virtual canvas.
        /// </summary>
        /// <returns>The resulting image of the flatten operation.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public IMagickImage Flatten() => Merge(LayerMethod.Flatten);

        /// <summary>
        /// Flatten this collection into a single image.
        /// This is useful for combining Photoshop layers into a single image.
        /// </summary>
        /// <param name="backgroundColor">The background color of the output image.</param>
        /// <returns>The resulting image of the flatten operation.</returns>
        public IMagickImage Flatten(MagickColor backgroundColor)
        {
            ThrowIfEmpty();

            var originalColor = _images[0].BackgroundColor;
            _images[0].BackgroundColor = backgroundColor;

            try
            {
                AttachImages();
                IntPtr image = _nativeInstance.Merge(_images[0], LayerMethod.Flatten);
                return MagickImage.Create(image, _images[0].Settings);
            }
            finally
            {
                DetachImages();
                _images[0].BackgroundColor = originalColor;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the images.
        /// </summary>
        /// <returns>An enumerator that iterates through the images.</returns>
        public IEnumerator<IMagickImage> GetEnumerator() => _images.GetEnumerator();

        /// <summary>
        /// Determines the index of the specified image.
        /// </summary>
        /// <param name="item">The image to check.</param>
        /// <returns>The index of the specified image.</returns>
        public int IndexOf(IMagickImage item) => _images.IndexOf(item);

        /// <summary>
        /// Inserts an image into the collection.
        /// </summary>
        /// <param name="index">The index to insert the image.</param>
        /// <param name="item">The image to insert.</param>
        public void Insert(int index, IMagickImage item)
        {
            CheckDuplication(item);

            _images.Insert(index, item);
        }

        /// <summary>
        /// Inserts an image with the specified file name into the collection.
        /// </summary>
        /// <param name="index">The index to insert the image.</param>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        public void Insert(int index, string fileName) => _images.Insert(index, new MagickImage(fileName));

        /// <summary>
        /// Remap image colors with closest color from reference image.
        /// </summary>
        /// <param name="image">The image to use.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Map(IMagickImage image) => Map(image, new QuantizeSettings());

        /// <summary>
        /// Remap image colors with closest color from reference image.
        /// </summary>
        /// <param name="image">The image to use.</param>
        /// <param name="settings">Quantize settings.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Map(IMagickImage image, QuantizeSettings settings)
        {
            ThrowIfEmpty();

            Throw.IfNull(nameof(image), image);
            Throw.IfNull(nameof(settings), settings);

            try
            {
                AttachImages();
                _nativeInstance.Map(_images[0], settings, image);
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Merge all layers onto a canvas just large enough to hold all the actual images. The virtual
        /// canvas of the first image is preserved but otherwise ignored.
        /// </summary>
        /// <returns>The resulting image of the merge operation.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public IMagickImage Merge() => Merge(LayerMethod.Merge);

        /// <summary>
        /// Create a composite image by combining the images with the specified settings.
        /// </summary>
        /// <param name="settings">The settings to use.</param>
        /// <returns>The resulting image of the montage operation.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public IMagickImage Montage(MontageSettings settings)
        {
            ThrowIfEmpty();

            Throw.IfNull(nameof(settings), settings);

            IntPtr images;
            try
            {
                AttachImages();
                if (!string.IsNullOrEmpty(settings.Label))
                    _images[0].Label = settings.Label;
                images = _nativeInstance.Montage(_images[0], settings);
            }
            finally
            {
                DetachImages();
            }

            using (IMagickImageCollection collection = new MagickImageCollection())
            {
                collection.AddRange(MagickImage.CreateList(images, _images[0].Settings));
                if (settings.TransparentColor != null)
                {
                    foreach (IMagickImage image in collection)
                    {
                        image.Transparent(settings.TransparentColor);
                    }
                }

                return collection.Merge();
            }
        }

        /// <summary>
        /// The Morph method requires a minimum of two images. The first image is transformed into
        /// the second by a number of intervening images as specified by frames.
        /// </summary>
        /// <param name="frames">The number of in-between images to generate.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Morph(int frames)
        {
            ThrowIfCountLowerThan(2);

            MagickSettings settings = _images[0].Settings.Clone();

            IntPtr images;
            try
            {
                AttachImages();
                images = _nativeInstance.Morph(_images[0], frames);
            }
            finally
            {
                DetachImages();
            }

            Clear();
            foreach (IMagickImage image in MagickImage.CreateList(images, settings))
                Add(image);
        }

        /// <summary>
        /// Start with the virtual canvas of the first image, enlarging left and right edges to contain
        /// all images. Images with negative offsets will be clipped.
        /// </summary>
        /// <returns>The resulting image of the mosaic operation.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public IMagickImage Mosaic() => Merge(LayerMethod.Mosaic);

        /// <summary>
        /// Compares each image the GIF disposed forms of the previous image in the sequence. From
        /// this it attempts to select the smallest cropped image to replace each frame, while
        /// preserving the results of the GIF animation.
        /// </summary>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Optimize()
        {
            ThrowIfEmpty();

            MagickSettings settings = _images[0].Settings.Clone();

            IntPtr images;
            try
            {
                AttachImages();
                images = _nativeInstance.Optimize(_images[0]);
            }
            finally
            {
                DetachImages();
            }

            Clear();
            foreach (IMagickImage image in MagickImage.CreateList(images, settings))
                Add(image);
        }

        /// <summary>
        /// OptimizePlus is exactly as Optimize, but may also add or even remove extra frames in the
        /// animation, if it improves the total number of pixels in the resulting GIF animation.
        /// </summary>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void OptimizePlus()
        {
            ThrowIfEmpty();

            MagickSettings settings = _images[0].Settings.Clone();

            IntPtr images;
            try
            {
                AttachImages();
                images = _nativeInstance.OptimizePlus(_images[0]);
            }
            finally
            {
                DetachImages();
            }

            Clear();
            foreach (IMagickImage image in MagickImage.CreateList(images, settings))
                Add(image);
        }

        /// <summary>
        /// Compares each image the GIF disposed forms of the previous image in the sequence. Any
        /// pixel that does not change the displayed result is replaced with transparency.
        /// </summary>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void OptimizeTransparency()
        {
            ThrowIfEmpty();

            try
            {
                AttachImages();
                _nativeInstance.OptimizeTransparency(_images[0]);
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Read only metadata and not the pixel data from all image frames.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Ping(byte[] data) => Ping(data, null);

        /// <summary>
        /// Reads only metadata and not the pixel data from all image frames.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <param name="offset">The offset at which to begin reading data.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Ping(byte[] data, int offset, int count) => Ping(data, offset, count, null);

        /// <summary>
        /// Reads only metadata and not the pixel data from all image frames.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <param name="offset">The offset at which to begin reading data.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Ping(byte[] data, int offset, int count, MagickReadSettings readSettings)
        {
            Throw.IfNullOrEmpty(nameof(data), data);
            Throw.IfTrue(nameof(offset), offset < 0, "The offset should be positive.");
            Throw.IfTrue(nameof(count), count < 1, "The number of bytes should be at least 1.");
            Throw.IfTrue(nameof(offset), offset >= data.Length, "The offset should not exceed the length of the array.");
            Throw.IfTrue(nameof(count), offset + count > data.Length, "The number of bytes should not exceed the length of the array.");

            Clear();
            AddImages(data, offset, count, readSettings, true);
        }

        /// <summary>
        /// Read only metadata and not the pixel data from all image frames.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Ping(byte[] data, MagickReadSettings readSettings)
        {
            Throw.IfNullOrEmpty(nameof(data), data);

            Clear();
            AddImages(data, 0, data.Length, readSettings, true);
        }

        /// <summary>
        /// Read only metadata and not the pixel data from all image frames.
        /// </summary>
        /// <param name="file">The file to read the frames from.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Ping(FileInfo file) => Ping(file, null);

        /// <summary>
        /// Read only metadata and not the pixel data from all image frames.
        /// </summary>
        /// <param name="file">The file to read the frames from.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Ping(FileInfo file, MagickReadSettings readSettings)
        {
            Throw.IfNull(nameof(file), file);

            Ping(file.FullName, readSettings);
        }

        /// <summary>
        /// Read only metadata and not the pixel data from all image frames.
        /// </summary>
        /// <param name="stream">The stream to read the image data from.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Ping(Stream stream) => Ping(stream, null);

        /// <summary>
        /// Read only metadata and not the pixel data from all image frames.
        /// </summary>
        /// <param name="stream">The stream to read the image data from.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Ping(Stream stream, MagickReadSettings readSettings)
        {
            Clear();
            AddImages(stream, readSettings, true);
        }

        /// <summary>
        /// Read only metadata and not the pixel data from all image frames.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Ping(string fileName) => Ping(fileName, null);

        /// <summary>
        /// Read only metadata and not the pixel data from all image frames.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Ping(string fileName, MagickReadSettings readSettings)
        {
            Clear();
            AddImages(fileName, readSettings, true);
        }

        /// <summary>
        /// Returns a new image where each pixel is the sum of the pixels in the image sequence after applying its
        /// corresponding terms (coefficient and degree pairs).
        /// </summary>
        /// <param name="terms">The list of polynomial coefficients and degree pairs and a constant.</param>
        /// <returns>A new image where each pixel is the sum of the pixels in the image sequence after applying its
        /// corresponding terms (coefficient and degree pairs).</returns>
        public IMagickImage Polynomial(double[] terms)
        {
            ThrowIfEmpty();

            Throw.IfNullOrEmpty(nameof(terms), terms);

            try
            {
                AttachImages();
                IntPtr image = _nativeInstance.Polynomial(_images[0], terms, terms.Length);
                return MagickImage.Create(image, _images[0].Settings);
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Quantize images (reduce number of colors).
        /// </summary>
        /// <returns>The resulting image of the quantize operation.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickErrorInfo Quantize() => Quantize(new QuantizeSettings());

        /// <summary>
        /// Quantize images (reduce number of colors).
        /// </summary>
        /// <param name="settings">Quantize settings.</param>
        /// <returns>The resulting image of the quantize operation.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public MagickErrorInfo Quantize(QuantizeSettings settings)
        {
            ThrowIfEmpty();

            Throw.IfNull(nameof(settings), settings);

            try
            {
                AttachImages();
                _nativeInstance.Quantize(_images[0], settings);
            }
            finally
            {
                DetachImages();
            }

            if (settings.MeasureErrors)
                return _images[0].CreateErrorInfo();
            else
                return null;
        }

        /// <summary>
        /// Read all image frames.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Read(byte[] data) => Read(data, null);

        /// <summary>
        /// Read all image frames.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <param name="offset">The offset at which to begin reading data.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Read(byte[] data, int offset, int count) => Read(data, offset, count, null);

        /// <summary>
        /// Read all image frames.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <param name="offset">The offset at which to begin reading data.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Read(byte[] data, int offset, int count, MagickReadSettings readSettings)
        {
            Throw.IfNullOrEmpty(nameof(data), data);
            Throw.IfTrue(nameof(offset), offset < 0, "The offset should be positive.");
            Throw.IfTrue(nameof(count), count < 1, "The number of bytes should be at least 1.");
            Throw.IfTrue(nameof(offset), offset >= data.Length, "The offset should not exceed the length of the array.");
            Throw.IfTrue(nameof(count), offset + count > data.Length, "The number of bytes should not exceed the length of the array.");

            Clear();
            AddImages(data, offset, count, readSettings, false);
        }

        /// <summary>
        /// Read all image frames.
        /// </summary>
        /// <param name="data">The byte array to read the image data from.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Read(byte[] data, MagickReadSettings readSettings)
        {
            Throw.IfNullOrEmpty(nameof(data), data);

            Clear();
            AddImages(data, 0, data.Length, readSettings, false);
        }

        /// <summary>
        /// Read all image frames.
        /// </summary>
        /// <param name="file">The file to read the frames from.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Read(FileInfo file) => Read(file, null);

        /// <summary>
        /// Read all image frames.
        /// </summary>
        /// <param name="file">The file to read the frames from.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Read(FileInfo file, MagickReadSettings readSettings)
        {
            Throw.IfNull(nameof(file), file);

            Read(file.FullName, readSettings);
        }

        /// <summary>
        /// Read all image frames.
        /// </summary>
        /// <param name="stream">The stream to read the image data from.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Read(Stream stream) => Read(stream, null);

        /// <summary>
        /// Read all image frames.
        /// </summary>
        /// <param name="stream">The stream to read the image data from.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Read(Stream stream, MagickReadSettings readSettings)
        {
            Clear();
            AddImages(stream, readSettings, false);
        }

        /// <summary>
        /// Read all image frames.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Read(string fileName) => Read(fileName, null);

        /// <summary>
        /// Read all image frames.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        /// <param name="readSettings">The settings to use when reading the image.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Read(string fileName, MagickReadSettings readSettings)
        {
            Clear();
            AddImages(fileName, readSettings, false);
        }

        /// <summary>
        /// Removes the first occurrence of the specified image from the collection.
        /// </summary>
        /// <param name="item">The image to remove.</param>
        /// <returns>True when the image was found and removed.</returns>
        public bool Remove(IMagickImage item)
        {
            return _images.Remove(item);
        }

        /// <summary>
        /// Removes the image at the specified index from the collection.
        /// </summary>
        /// <param name="index">The index of the image to remove.</param>
        public void RemoveAt(int index) => _images.RemoveAt(index);

        /// <summary>
        /// Resets the page property of every image in the collection.
        /// </summary>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void RePage()
        {
            foreach (IMagickImage image in _images)
            {
                image.RePage();
            }
        }

        /// <summary>
        /// Reverses the order of the images in the collection.
        /// </summary>
        public void Reverse() => _images.Reverse();

        /// <summary>
        /// Smush images from list into single image in horizontal direction.
        /// </summary>
        /// <param name="offset">Minimum distance in pixels between images.</param>
        /// <returns>The resulting image of the smush operation.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public IMagickImage SmushHorizontal(int offset)
        {
            ThrowIfEmpty();

            try
            {
                AttachImages();
                IntPtr image = _nativeInstance.Smush(_images[0], offset, false);
                return MagickImage.Create(image, _images[0].Settings);
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Smush images from list into single image in vertical direction.
        /// </summary>
        /// <param name="offset">Minimum distance in pixels between images.</param>
        /// <returns>The resulting image of the smush operation.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public IMagickImage SmushVertical(int offset)
        {
            ThrowIfEmpty();

            try
            {
                AttachImages();
                IntPtr image = _nativeInstance.Smush(_images[0], offset, true);
                return MagickImage.Create(image, _images[0].Settings);
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Converts this instance to a <see cref="byte"/> array.
        /// </summary>
        /// <returns>A <see cref="byte"/> array.</returns>
        public byte[] ToByteArray()
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                Write(memStream);
                return memStream.ToArray();
            }
        }

        /// <summary>
        /// Converts this instance to a <see cref="byte"/> array.
        /// </summary>
        /// <param name="defines">The defines to set.</param>
        /// <returns>A <see cref="byte"/> array.</returns>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public byte[] ToByteArray(IWriteDefines defines)
        {
            SetDefines(defines);
            return ToByteArray(defines);
        }

        /// <summary>
        /// Converts this instance to a <see cref="byte"/> array.
        /// </summary>
        /// <returns>A <see cref="byte"/> array.</returns>
        /// <param name="format">The format to use.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public byte[] ToByteArray(MagickFormat format)
        {
            SetFormat(format);
            return ToByteArray();
        }

        /// <summary>
        /// Converts this instance to a base64 <see cref="string"/>.
        /// </summary>
        /// <returns>A base64 <see cref="string"/>.</returns>
        public string ToBase64()
        {
            var bytes = ToByteArray();
            return ToBase64(bytes);
        }

        /// <summary>
        /// Converts this instance to a base64 string.
        /// </summary>
        /// <param name="format">The format to use.</param>
        /// <returns>A base64 <see cref="string"/>.</returns>
        public string ToBase64(MagickFormat format)
        {
            var bytes = ToByteArray(format);
            return ToBase64(bytes);
        }

        /// <summary>
        /// Determine the overall bounds of all the image layers just as in <see cref="Merge()"/>,
        /// then adjust the the canvas and offsets to be relative to those bounds,
        /// without overlaying the images.
        /// </summary>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void TrimBounds()
        {
            ThrowIfEmpty();

            try
            {
                AttachImages();
                _nativeInstance.Merge(_images[0], LayerMethod.Trimbounds);
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Writes the images to the specified file. If the output image's file format does not
        /// allow multi-image files multiple files will be written.
        /// </summary>
        /// <param name="file">The file to write the image to.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Write(FileInfo file)
        {
            Throw.IfNull(nameof(file), file);

            Write(file.FullName);
            file.Refresh();
        }

        /// <summary>
        /// Writes the images to the specified file. If the output image's file format does not
        /// allow multi-image files multiple files will be written.
        /// </summary>
        /// <param name="file">The file to write the image to.</param>
        /// <param name="defines">The defines to set.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Write(FileInfo file, IWriteDefines defines)
        {
            SetDefines(defines);
            Write(file);
        }

        /// <summary>
        /// Writes the imagse to the specified stream. If the output image's file format does not
        /// allow multi-image files multiple files will be written.
        /// </summary>
        /// <param name="stream">The stream to write the images to.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Write(Stream stream)
        {
            Throw.IfNull(nameof(stream), stream);

            if (_images.Count == 0)
                return;

            MagickSettings settings = _images[0].Settings.Clone();
            settings.FileName = null;

            try
            {
                AttachImages();

                using (StreamWrapper wrapper = StreamWrapper.CreateForWriting(stream))
                {
                    ReadWriteStreamDelegate writer = new ReadWriteStreamDelegate(wrapper.Write);
                    ReadWriteStreamDelegate reader = null;
                    SeekStreamDelegate seeker = null;
                    TellStreamDelegate teller = null;

                    if (stream.CanSeek)
                    {
                        seeker = new SeekStreamDelegate(wrapper.Seek);
                        teller = new TellStreamDelegate(wrapper.Tell);
                    }

                    if (stream.CanRead)
                        reader = new ReadWriteStreamDelegate(wrapper.Read);

                    _nativeInstance.WriteStream(_images[0], settings, writer, seeker, teller, reader);
                }
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Writes the imagse to the specified stream. If the output image's file format does not
        /// allow multi-image files multiple files will be written.
        /// </summary>
        /// <param name="stream">The stream to write the images to.</param>
        /// <param name="defines">The defines to set.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Write(Stream stream, IWriteDefines defines)
        {
            SetDefines(defines);
            SetFormat(defines.Format);
            Write(stream);
        }

        /// <summary>
        /// Writes the image to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write the image data to.</param>
        /// <param name="format">The format to use.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Write(Stream stream, MagickFormat format)
        {
            SetFormat(format);
            Write(stream);
        }

        /// <summary>
        /// Writes the images to the specified file name. If the output image's file format does not
        /// allow multi-image files multiple files will be written.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Write(string fileName)
        {
            string filePath = FileHelper.CheckForBaseDirectory(fileName);

            Throw.IfNullOrEmpty(nameof(fileName), filePath);

            if (_images.Count == 0)
                return;

            MagickSettings settings = _images[0].Settings.Clone();
            settings.FileName = fileName;

            try
            {
                AttachImages();
                _nativeInstance.WriteFile(_images[0], settings);
            }
            finally
            {
                DetachImages();
            }
        }

        /// <summary>
        /// Writes the images to the specified file name. If the output image's file format does not
        /// allow multi-image files multiple files will be written.
        /// </summary>
        /// <param name="fileName">The fully qualified name of the image file, or the relative image file name.</param>
        /// <param name="defines">The defines to set.</param>
        /// <exception cref="MagickException">Thrown when an error is raised by ImageMagick.</exception>
        public void Write(string fileName, IWriteDefines defines)
        {
            SetDefines(defines);
            Write(fileName);
        }

        private static MagickSettings CreateSettings(MagickReadSettings readSettings)
        {
            if (readSettings == null)
                return new MagickSettings();

            return new MagickReadSettings(readSettings);
        }

        private static string ToBase64(byte[] bytes)
        {
            if (bytes == null)
                return string.Empty;

            return Convert.ToBase64String(bytes);
        }

        private void AddImages(byte[] data, int offset, int count, MagickReadSettings readSettings, bool ping)
        {
            MagickSettings settings = CreateSettings(readSettings);
            settings.Ping = ping;

            IntPtr result = _nativeInstance.ReadBlob(settings, data, offset, count);
            AddImages(result, settings);
        }

        private void AddImages(string fileName, MagickReadSettings readSettings, bool ping)
        {
            string filePath = FileHelper.CheckForBaseDirectory(fileName);
            Throw.IfNullOrEmpty(nameof(fileName), filePath);

            MagickSettings settings = CreateSettings(readSettings);
            settings.FileName = filePath;
            settings.Ping = ping;

            IntPtr result = _nativeInstance.ReadFile(settings);
            AddImages(result, settings);
        }

        private void AddImages(Stream stream, MagickReadSettings readSettings, bool ping)
        {
            Throw.IfNullOrEmpty(nameof(stream), stream);

            Bytes bytes = Bytes.FromStreamBuffer(stream);
            if (bytes != null)
            {
                AddImages(bytes.Data, 0, bytes.Length, readSettings, ping);
                return;
            }

            MagickSettings settings = CreateSettings(readSettings);
            settings.Ping = ping;
            settings.FileName = null;

            using (StreamWrapper wrapper = StreamWrapper.CreateForReading(stream))
            {
                ReadWriteStreamDelegate reader = new ReadWriteStreamDelegate(wrapper.Read);
                SeekStreamDelegate seeker = null;
                TellStreamDelegate teller = null;

                if (stream.CanSeek)
                {
                    seeker = new SeekStreamDelegate(wrapper.Seek);
                    teller = new TellStreamDelegate(wrapper.Tell);
                }

                IntPtr result = _nativeInstance.ReadStream(settings, reader, seeker, teller);
                AddImages(result, settings);
            }
        }

        private void AddImages(IntPtr result, MagickSettings settings)
        {
            settings.Format = MagickFormat.Unknown;

            foreach (IMagickImage image in MagickImage.CreateList(result, settings))
            {
                _images.Add(image);
            }
        }

        private void AttachImages()
        {
            for (int i = 0; i < _images.Count - 1; i++)
            {
                _images[i].SetNext(_images[i + 1]);
            }
        }

        private void CheckDuplication(IMagickImage item)
        {
            foreach (var image in _images)
            {
                if (ReferenceEquals(image, item))
                    throw new InvalidOperationException("Not allowed to add the same image to the collection.");
            }
        }

        private void DetachImages()
        {
            for (int i = _images.Count - 2; i > 0; i--)
            {
                _images[i].SetNext(null);
            }
        }

        private void Dispose(bool disposing)
        {
            if (_nativeInstance != null)
                _nativeInstance.Warning -= OnWarning;

            if (disposing)
                Clear();
        }

        private IMagickImage Merge(LayerMethod layerMethod)
        {
            ThrowIfEmpty();

            try
            {
                AttachImages();
                IntPtr image = _nativeInstance.Merge(_images[0], layerMethod);
                return MagickImage.Create(image, _images[0].Settings);
            }
            finally
            {
                DetachImages();
            }
        }

        private void OnWarning(object sender, WarningEventArgs arguments) => _warning?.Invoke(this, arguments);

        private void SetDefines([ValidatedNotNull] IWriteDefines defines)
        {
            foreach (IMagickImage image in _images)
            {
                image.Settings.SetDefines(defines);
            }
        }

        private void SetFormat(MagickFormat format)
        {
            foreach (IMagickImage image in _images)
            {
                image.Format = format;
            }
        }

        private void ThrowIfEmpty()
        {
            if (_images.Count == 0)
                throw new InvalidOperationException("Operation requires at least one image.");
        }

        private void ThrowIfCountLowerThan(int count)
        {
            if (_images.Count < count)
                throw new InvalidOperationException("Operation requires at least " + count + " images.");
        }
    }
}