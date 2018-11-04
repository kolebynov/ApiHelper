namespace RestApi.Common
{
    public class Filter
    {
        public string PropertyPath { get; set; }
        public ComparisonType ComparisonType { get; set; }
        public string Value { get; set; }
    }
}
