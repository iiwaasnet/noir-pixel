using System;

namespace Api.App.Exceptions
{
    public class OverMaxAllowedFileSizeException : Exception
    {
        public OverMaxAllowedFileSizeException(int maxAllowedSizeMb, int fileSizeMb)
        {
            MaxAllowedSizeMB = maxAllowedSizeMb;
            FileSizeMB = fileSizeMb;
        }

        public int MaxAllowedSizeMB { get; private set; }
        public int FileSizeMB { get; private set; }
    }
}