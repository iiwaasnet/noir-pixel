using System.Net.Http;

namespace Api.App.Media
{
    internal class ChunkInfo
    {
        public int ChunkNumber { get; set; }
        public int TotalChunks { get; set; }
        public string Identifier { get; set; }
        public string FileName { get; set; }
        public MultipartFileData Chunk { get; set; }
    }
}