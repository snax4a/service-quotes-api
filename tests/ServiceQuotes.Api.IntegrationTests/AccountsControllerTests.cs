using ServiceQuotes.Api.IntegrationTests.Helpers;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ServiceQuotes.Api.IntegrationTests
{
    public class AccountsControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Post_ValidCredentials_ReturnsOk()
        {
            // Arrange
            var client = _factory.RebuildDb().CreateClient();

            // Act
            var accountCredentials = JsonConvert.SerializeObject(new
            {
                Email = "manager@test.com",
                Password = "manager123"
            });
            var content = new StringContent(accountCredentials, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/accounts/authenticate", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            json["id"].Should().NotBeNull();
            json["email"].Should().NotBeNull();
            json["role"].Should().NotBeNull();
            json["created"].Should().NotBeNull();
            json["jwtToken"].Should().NotBeNull();
        }

        [Fact]
        public async Task Post_InValidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var client = _factory.RebuildDb().CreateClient();

            // Act
            var accountCredentials = JsonConvert.SerializeObject(new
            {
                Email = "invalid.email@test.com",
                Password = "invalidPassword"
            });
            var content = new StringContent(accountCredentials, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/accounts/authenticate", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            json["message"].Should().NotBeNull();

            Assert.Equal("Email or password is incorrect", json["message"]);
        }

    }
}
