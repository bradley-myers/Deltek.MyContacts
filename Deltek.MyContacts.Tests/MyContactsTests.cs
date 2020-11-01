using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Deltek.MyContacts.Tests
{
    public class MyContactsTests : IClassFixture<WebApplicationFactory<Deltek.MyContacts.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public MyContactsTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }
        
        [Theory]
        [InlineData("/contact")]
        [InlineData("/contact/2")]
        public async Task GetEndpointsReturnSuccess(string url)
        {
            // Arrange:
            HttpClient httpClient = _factory.CreateClient();
            
            // Act:
            var response = await httpClient.GetAsync(url);
            
            // Assert:
            response.EnsureSuccessStatusCode();
        }
    }
}