using System;
using RestApi;

namespace TestRestApi
{
    public class TestModel : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
