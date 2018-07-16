using RestApi;
using System;
using System.ComponentModel.DataAnnotations;

namespace TestRestApi
{
    public class TestEntity : IIdentifiable
    {
        public string Name { get; set; }
        public string Name2 { get; set; }
        [Key]
        public Guid Id { get; set; }
    }
}
