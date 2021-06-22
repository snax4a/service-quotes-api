using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceQuotes.Api.IntegrationTests.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ServiceQuotes.Api.IntegrationTests
{
    public class AccountsControllerTests : IntegrationTest
    {
        public AccountsControllerTests(WebApplicationFactory<Startup> fixture) : base(fixture) { }

        #region GET

        [Fact]
        public async Task Get_AllAccounts_ReturnsOk_ForAuthorizedRole()
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var response = await client.GetAsync("/api/accounts");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            string json = await response.Content.ReadAsStringAsync();
            var array = JArray.Parse(json);
            array.HasValues.Should().BeTrue();
            array.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task Get_AllAccounts_ReturnsUnauthorized_ForUnAuthorizedRole()
        {
            // Arrange
            var accountId = Guid.Parse("8720c542-7784-460a-91ca-31bf633eae50");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var response = await client.GetAsync("/api/accounts");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            json["message"].Should().NotBeNull();

            Assert.Equal("Unauthorized", json["message"]);
        }

        [Fact]
        public async Task Get_ExistingAccountsWithFilter_ReturnsOk()
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var response = await client.GetAsync("/api/accounts?searchString=customer@test.com");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            string json = await response.Content.ReadAsStringAsync();
            var array = JArray.Parse(json);
            array.HasValues.Should().BeTrue();
            array.Should().OnlyHaveUniqueItems();
            array.Should().ContainSingle();
        }

        [Fact]
        public async Task Get_NonExistingAccountsWithFilter_ReturnsOk()
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var response = await client.GetAsync("/api/accounts?searchString=not.existing@email.com");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            string json = await response.Content.ReadAsStringAsync();
            var array = JArray.Parse(json);
            array.Should().BeEmpty();
        }

        [Theory]
        [InlineData("8720c542-7784-460a-91ca-31bf633eae50")]
        [InlineData("8411910e-5d47-40fb-a29b-6ad9dbbd9f63")]
        [InlineData("be95846f-6298-45eb-b732-dddadb2e4f83")]
        public async Task Get_ExistingAccountById_ReturnsOk(Guid id)
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var response = await client.GetAsync($"/api/accounts/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            json["id"].Should().NotBeNull();
            json["email"].Should().NotBeNull();
            json["role"].Should().NotBeNull();
            json["created"].Should().NotBeNull();

            Assert.Equal(id.ToString(), json["id"]);
        }

        [Theory]
        [InlineData("7c7b7ab3-456f-484c-b90b-0116443df2aa")]
        [InlineData("5751ca54-3f43-4393-adc9-ad6e69fcdacd")]
        [InlineData("92422c00-7ba8-43e9-8426-8107674761c5")]
        public async Task Get_NonExistingAccountById_ReturnsNotFound(Guid id)
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var response = await client.GetAsync($"/api/accounts/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion

        #region POST

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

        [Fact]
        public async Task Post_ValidAccount_ReturnsCreated_ForAuthorizedRole()
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var newAccount = JsonConvert.SerializeObject(new
            {
                Email = "new@test.com",
                Password = "newPassword123",
                RepeatPassword = "newPassword123",
                Role = "Customer",
                CompanyName = "Test Company Name",
                VatNumber = "PL7922298336"
            });
            var content = new StringContent(newAccount, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/accounts", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            json["id"].Should().NotBeNull();
            json["customerId"].Should().NotBeNull();
            json["created"].Should().NotBeNull();

            Assert.Equal("new@test.com", json["email"]);
            Assert.Equal("Customer", json["role"]);
            Assert.Equal("Test Company Name", json["companyName"]);
            Assert.Equal("PL7922298336", json["vatNumber"]);
        }

        [Fact]
        public async Task Post_ValidAccount_ReturnsUnauthorized_ForUnAuthorizedRole()
        {
            // Arrange
            var accountId = Guid.Parse("87aed32c-dcf7-45bb-a4e1-57c8c2b7262e");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var newAccount = JsonConvert.SerializeObject(new
            {
                Email = "new@test.com",
                Password = "newPassword123",
                RepeatPassword = "newPassword123",
                Role = "Customer",
                CompanyName = "Test Company",
                VatNumber = "PL7922298336"
            });
            var content = new StringContent(newAccount, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/accounts", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Theory]
        [InlineData("manager@test.com")]
        [InlineData("customer@test.com")]
        public async Task Post_ValidAccount_ReturnsBadRequest_ForUsedEmail(string email)
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var newAccount = JsonConvert.SerializeObject(new
            {
                Email = email,
                Password = "newPassword123",
                RepeatPassword = "newPassword123",
                Role = "Customer",
                CompanyName = "Test Company",
                VatNumber = "PL7922298336"
            });
            var content = new StringContent(newAccount, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/accounts", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var json = JObject.Parse(await response.Content.ReadAsStringAsync());
            json["message"].Should().NotBeNull();

            Assert.Equal($"Email '{email}' is already taken", json["message"]);
        }

        [Fact]
        public async Task Post_InValidAccount_ReturnsBadRequest()
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var newAccount = JsonConvert.SerializeObject(new
            {
                Email = "newaccount@test.com",
                Password = "",
                RepeatPassword = "newPassword123",
                Role = "Customer"
            });
            var content = new StringContent(newAccount, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/accounts", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_EmptyAccount_ReturnsBadRequest()
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var newAccount = JsonConvert.SerializeObject(new { });
            var content = new StringContent(newAccount, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/api/accounts", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        #endregion

        #region PUT

        [Theory]
        [InlineData("8720c542-7784-460a-91ca-31bf633eae50")]
        public async Task Put_ValidAccount_ReturnsOk(Guid id)
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var newAccount = JsonConvert.SerializeObject(new
            {
                Email = "customer2@test.com",
                Password = "newPassword12345",
                RepeatPassword = "newPassword12345",
            });
            var content = new StringContent(newAccount, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/accounts/{id}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("8720c542-7784-460a-91ca-31bf633eae50")]
        public async Task Put_AccountWithUsedEmail_ReturnsBadRequest(Guid id)
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var newAccount = JsonConvert.SerializeObject(new
            {
                Email = "manager@test.com"
            });
            var content = new StringContent(newAccount, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/accounts/{id}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData("2a992e27-bd6a-4385-a4e2-8386d741e6fe")]
        public async Task Put_InvalidAccountId_ReturnsNotFound(Guid id)
        {
            // Arrange
            var accountId = Guid.Parse("be95846f-6298-45eb-b732-dddadb2e4f83");
            var client = _factory.RebuildDb().CreateClientWithAuth(accountId);

            // Act
            var newAccount = JsonConvert.SerializeObject(new
            {
                Email = "john.doe@test.com"
            });
            var content = new StringContent(newAccount, Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"/api/accounts/{id}", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        #endregion
    }
}
