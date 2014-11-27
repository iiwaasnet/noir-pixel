namespace Api.App.Errors
{
    public class ApiError
    {
        public StringResource Main { get; set; }
        public StringResource Aux { get; set; }

        public class StringResource
        {
            public string Text { get; set; }
            public string Id { get; set; }
        }     
    }

    
}