using System.Collections.Generic;

namespace Api.App.Images
{
    public class PendingPhotos
    {
        public IEnumerable<Photo> Photos { get; set; }
        public Paging Paging { get; set; }
    }
}