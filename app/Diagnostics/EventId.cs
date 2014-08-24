namespace Diagnostics
{
    /// <summary>
    ///     1xxxx - General exception
    /// </summary>
    public class EventId
    {
        public static EventId GeneralError = new EventId {Id = 10000};

        public int Id { get; private set; }

        public override string ToString()
        {
            return Id.ToString();
        }
    }
}