namespace RestApi.Common
{
    public class SortOption
    {
        public string Column { get; set; }
        public OrderDirection Direction { get; set; } = OrderDirection.Asc;
    }
}
