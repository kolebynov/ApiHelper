using System;
using RestApi;

namespace TestRestApi
{
    public class TestModel2 : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
