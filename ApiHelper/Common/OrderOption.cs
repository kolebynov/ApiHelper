namespace RestApi.Common
{
    public class OrderOption
    {
        public string Column { get; set; }
        public OrderDirection Direction { get; set; } = OrderDirection.Asc;
    }
}
