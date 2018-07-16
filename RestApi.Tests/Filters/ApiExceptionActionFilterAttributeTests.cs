using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using RestApi.ApiResults;
using RestApi.Filters;
using RestApi.Services.Api;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace RestApi.Tests.Filters
{
    public class ApiExceptionActionFilterAttributeTests
    {
        [Fact]
        public void ReturnRightResult()
        {
            //Arrange
            Exception exc = new Exception("test");
            Mock<IApiHelper> mock = new Mock<IApiHelper>();
            ApiResult apiResult = new ApiResult
            {
                Success = false,
                Errors = new[] {new ApiError {Message = exc.Message}}
            };
            mock.Setup(m => m.GetErrorResultFromException(exc))
                .Returns(apiResult);
            ApiExceptionActionFilterAttribute attribute = new ApiExceptionActionFilterAttribute(mock.Object);
            ExceptionContext context = new Mock<ExceptionContext>(new ActionContext(Mock.Of<HttpContext>(), new RouteData(), new ActionDescriptor()), 
                new List<IFilterMetadata>())
                .SetupProperty(m => m.Exception, exc)
                .SetupProperty(m => m.Result, null)
                .Object;

            //Act
            attribute.OnException(context);

            //Assert
            Assert.IsAssignableFrom<BadRequestObjectResult>(context.Result);
            Assert.Equal(apiResult, ((BadRequestObjectResult)context.Result).Value);
        }
    }
}
