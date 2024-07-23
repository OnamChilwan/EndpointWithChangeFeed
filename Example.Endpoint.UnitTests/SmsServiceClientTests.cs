using System.Net;
using System.Net.Http.Json;
using Azure.Core;
using Example.Endpoint.Handlers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NSubstitute;

namespace Example.Endpoint.UnitTests;

public class SmsServiceClientTests
{
    [Test]
    public async Task When()
    {
        var fakeHttpMessageHandler = new FakeHttpMessageHandler();
        var subject = new SmsServiceClient(new HttpClient(fakeHttpMessageHandler), Substitute.For<ILogger<SmsServiceClient>>());
        
        fakeHttpMessageHandler.Returns(new HttpResponseMessage
        {
            Content = JsonContent.Create(new SmsResponse()),
            StatusCode = HttpStatusCode.OK
        });

        var smsServiceRequest = new SmsServiceRequest
        {
            Message = "Payment successful",
            TelephoneNumber = "999"
        };
        await subject.SendSms(smsServiceRequest);

        fakeHttpMessageHandler.HttpRequestMessage.RequestUri.Should().Be("");
        fakeHttpMessageHandler.HttpRequestMessage.Method.Should().Be(HttpMethod.Post);
        
        var request = await fakeHttpMessageHandler.HttpRequestMessage.Content!.ReadFromJsonAsync<SmsServiceRequest>();
        request.Should().NotBeNull();
        request!.Message.Should().Be(smsServiceRequest.Message);
    }
}

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private HttpResponseMessage _httpResponse = null!;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpRequestMessage = request;
        return Task.FromResult(_httpResponse);
    }

    public void Returns(HttpResponseMessage httpResponseMessage)
    {
        _httpResponse = httpResponseMessage;
    }
    
    public HttpRequestMessage HttpRequestMessage { get; private set; }
}