using System.Collections.Generic;

namespace Api.App.Images
{
    public class PendingPhotos
    {
        public IEnumerable<ImageData> Photos { get; set; }
        public Paging Paging { get; set; }
    }
}