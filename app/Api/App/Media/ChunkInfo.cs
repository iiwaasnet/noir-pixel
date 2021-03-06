﻿using System.Net.Http;

namespace Api.App.Media
{
    public class ChunkInfo
    {
        public int ChunkNumber { get; set; }
        public int TotalChunks { get; set; }
        public string Identifier { get; set; }
        public string FileName { get; set; }
        public string UserId { get; set; }
        public int TotalSizeBytes { get; set; }
        public MultipartFileData Chunk { get; set; }
    }
}