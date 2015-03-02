using System.Collections.Generic;

namespace Api.App.Images
{
    public interface IImageValidator
    {
        void Assert(string fileName, IEnumerable<ImageConstraint> constraints);
    }
}