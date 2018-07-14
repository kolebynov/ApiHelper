using System;

namespace RestApi
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }
}