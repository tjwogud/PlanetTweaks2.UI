using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using UnityEngine;

namespace PlanetTweaks2.UI
{
    public abstract class PlanetImage
    {
        public static PlanetImage Create(string path)
        {
            var ext = Path.GetExtension(path);
            var isAnim = ext == ".gif" || ext == ".apng";
            var isPtAnim = ext == ".ptgif";

            if (!isAnim && !isPtAnim)
            {
                var data = File.ReadAllBytes(path);
                var sprite = ToSprite(data);
                sprite.name = Path.GetFileNameWithoutExtension(path);
                return new SimplePlanetImage { sprite = sprite };
            }

            if (!isPtAnim)
            {
                var image = Image.FromFile(path);
                var fd = new FrameDimension(image.FrameDimensionsList[0]);
                var frameCount = image.GetFrameCount(fd);
                var images = new Sprite[frameCount];
                var lengths = new int[frameCount];
                if (frameCount > 1)
                {
                    var times = image.GetPropertyItem(0x5100).Value;
                    var totalLength = 0;
                    for (int i = 0; i < frameCount; i++)
                    {
                        image.SelectActiveFrame(fd, i);
                        var length = BitConverter.ToInt32(times, i * 4) * 10;
                        totalLength += length;
                        Debug.Log(i + ": " + length);

                        var sprite = ToSprite(ToBytes(image));
                        sprite.name = Path.GetFileNameWithoutExtension(path) + "_" + i;

                        images[i] = sprite;
                        lengths[i] = length;
                    }

                    var result = new AnimPlanetImage();
                    result.frames = frameCount;
                    result.duration = totalLength;
                    result.images = images;
                    result.lengths = lengths;
                    return result;
                }
                else
                {
                    var sprite = ToSprite(ToBytes(image));
                    sprite.name = Path.GetFileNameWithoutExtension(path);

                    return new SimplePlanetImage() { sprite = sprite };
                }
            }
            else
            {
                using var reader = new BinaryReader(new DeflateStream(File.Open(path, FileMode.Open), CompressionMode.Decompress));
                var frames = reader.ReadInt32();
                var duration = 0;
                var images = new Sprite[frames];
                var lengths = new int[frames];
                for (int i = 0; i < frames; i++)
                {
                    var length = reader.ReadInt32();
                    duration += length;
                    lengths[i] = length;

                    var dataLength = reader.ReadInt32();
                    var data = reader.ReadBytes(dataLength);
                    var sprite = ToSprite(data);
                    sprite.name = Path.GetFileNameWithoutExtension(path) + "_" + i;
                    images[i] = sprite;
                }
                return new AnimPlanetImage()
                {
                    frames = frames,
                    duration = duration, 
                    images = images,
                    lengths = lengths
                };
            }
        }

        private static byte[] ToBytes(Image image)
        {
            using var stream = new MemoryStream();
            image.Save(stream, ImageFormat.Png);
            return stream.ToArray();
        }

        private static Sprite ToSprite(byte[] bytes)
        {
            var texture = new Texture2D(0, 0);
            if (!texture.LoadImage(bytes))
                return null; // this is wrong... surely...
            var sprite = Sprite.Create(texture, new(0, 0, texture.width, texture.height), Vector2.one / 2, 512);
            return sprite;
        }

        public abstract Sprite GetImage();

        public abstract string GetExtension();

        public abstract void Save(string path);
    }

    public class SimplePlanetImage : PlanetImage
    {
        public Sprite sprite;

        public override Sprite GetImage()
        {
            return sprite;
        }

        public override string GetExtension()
        {
            return ".png";
        }

        public override void Save(string path)
        {
            var png = sprite.texture.EncodeToPNG();
            File.WriteAllBytes(path, png);
        }
    }

    public class AnimPlanetImage : PlanetImage
    {
        public int frames;
        public int duration;
        public Sprite[] images;
        public int[] lengths;

        public override Sprite GetImage()
        {
            var time = Time.unscaledTime * 1000 % duration;
            for (int i = 0; i < frames - 1; i++)
            {
                time -= lengths[i];
                if (time < 0)
                    return images[i];
            }
            return images[frames - 1];
        }

        public override string GetExtension()
        {
            return ".ptgif";
        }

        public override void Save(string path)
        {
            using var writer = new BinaryWriter(new DeflateStream(File.Create(path), System.IO.Compression.CompressionLevel.Optimal));
            writer.Write(frames);
            for (int i = 0; i < frames; i++)
            {
                writer.Write(lengths[i]);
                var data = images[i].texture.EncodeToPNG();
                writer.Write(data.Length);
                writer.Write(data);
            }
        }
    }
}
