using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.App.Media
{
    public interface IMediaValidator
    {
        Task Assert(ChunkInfo chunk, IEnumerable<MediaConstraint> constraints);
    }
}