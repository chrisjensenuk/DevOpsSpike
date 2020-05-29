using DevOpsSpike;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace DevOpsSpikeTests
{
    public class Function1Tests
    {
        [Fact]
        public async Task RunAsync_when_name_specfied_then_should_be_in_response()
        {
            var query = new Dictionary<string, StringValues>
            {
                { "name", new StringValues("Bob") }
            };

            var req = CreateHttpRequest(query, "");

            var result = (OkObjectResult)await Function1.RunAsync(req, Mock.Of<ILogger>());

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().Be("Hello, Bob. This HTTP triggered function executed successfully.");
        }

        [Fact]
        public async Task RunAsync_when_name_not_specfied_then_should_show_instruction()
        {
            var query = new Dictionary<string, StringValues>();

            var req = CreateHttpRequest(query, "");

            var result = (OkObjectResult)await Function1.RunAsync(req, Mock.Of<ILogger>());

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().Be("BROKEN TEST. This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response.");
        }

        public HttpRequest CreateHttpRequest(Dictionary<string, StringValues> query, string body)
        {
            var req = Mock.Of<HttpRequest>();

            Mock.Get(req).Setup(req => req.Query).Returns(new QueryCollection(query));

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(body);
            writer.Flush();
            stream.Position = 0;
            Mock.Get(req).Setup(req => req.Body).Returns(stream);

            return req;
        }
    }
}
