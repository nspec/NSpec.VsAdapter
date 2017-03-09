using System;

namespace NSpec.VsAdapter.Logging
{
    [Serializable]
    public class ExceptionLogInfo
    {
        public ExceptionLogInfo() { }  // needed for deserialization

        public ExceptionLogInfo(Exception ex)
        {
            Type = ex.GetType().ToString();
            Content = ex.ToString();
        }

        public string Type { get; set; }

        public string Content { get; set; }
    }
}
