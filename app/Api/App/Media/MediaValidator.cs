using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.App.Exceptions;

namespace Api.App.Media
{
    public class MediaValidator : IMediaValidator
    {
        private readonly byte[] jpeg;
        private readonly byte[] jpegCannon;
        private readonly byte[] gif;

        public MediaValidator()
        {
            jpeg = new byte[] {0xFF, 0xD8, 0xFF, 0xE0};
            jpegCannon = new byte[] {0xFF, 0xD8, 0xFF, 0xE1};
            gif = new byte[] {0x47, 0x49, 0x46};
        }

        public async Task Assert(ChunkInfo chunk, IEnumerable<MediaConstraint> constraints)
        {
            if (chunk.ChunkNumber == 1)
            {
                var mediaType = await FindSupportedMediaType(chunk, constraints);

                var maxFileSize = constraints.First(c => c.MediaType == mediaType).MaxFileSizeMB;
                AssertFileSizeAllowed(chunk.TotalSizeBytes, maxFileSize);
            }
        }

        private async Task<MediaType> FindSupportedMediaType(ChunkInfo chunk, IEnumerable<MediaConstraint> constraints)
        {
            foreach (var mediaConstraint in constraints)
            {
                var fileTypeValidator = GetFileFormatValidator(mediaConstraint.MediaType);
                if (await fileTypeValidator(chunk.Chunk.LocalFileName))
                {
                    return mediaConstraint.MediaType;
                }
            }

            throw new UnsupportedFileTypeException();
        }

        private Func<string, Task<bool>> GetFileFormatValidator(MediaType mediaType)
        {
            switch (mediaType)
            {
                case MediaType.Jpeg:
                    return AssertIsJpeg;
                default:
                    throw new NotImplementedException(mediaType.ToString());
            }
        }

        private async Task<bool> AssertIsJpeg(string fileName)
        {
            using (var fileChunk = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[Math.Max(jpeg.Length, jpegCannon.Length)];
                await fileChunk.ReadAsync(buffer, 0, buffer.Length);
                return (jpeg.SequenceEqual(buffer.Take(jpeg.Length))
                        ||
                        jpegCannon.SequenceEqual(buffer.Take(jpegCannon.Length)));
            }
        }

        private void AssertFileSizeAllowed(int totalBytes, int allowedFileSizeMB)
        {
            var fileSizeMB = totalBytes / 1024 / 1024;
            if (fileSizeMB > allowedFileSizeMB)
            {
                throw new OverMaxAllowedFileSize(allowedFileSizeMB, fileSizeMB);
            }
        }
    }
}