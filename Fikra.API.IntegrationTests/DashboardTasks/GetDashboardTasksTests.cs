using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Fikra.API.Helpers;
using Fikra.API.IntegrationTests.Helpers;
using Fikra.API.Models.Task;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Fikra.API.IntegrationTests.DashboardTasks
{
	public class GetDashboardTasksTests : IClassFixture<FikraWebApplicationFactory<API.Startup>>
	{
		private readonly HttpClient _httpClient;
		private readonly FikraWebApplicationFactory<API.Startup> _factory;

		public GetDashboardTasksTests(FikraWebApplicationFactory<API.Startup> factory)
		{
			_factory = factory;
			_httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
			{
				AllowAutoRedirect = false,
				BaseAddress = new Uri("https://localhost:44317/api/dashboards/")
			});
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(3)]
		public async Task GetDashboardTasks_ForExistingDashboard_ReturnsOkWithListOfTasks(int dashboardId)
		{
			// Arrange
			var requestUri = $"{dashboardId}/tasks";

			// Act
			var response = await _httpClient.GetAsync(requestUri);
			var rawData = response.Content.ReadAsByteArrayAsync().Result;
			var tasks = FikraConverter.FromBytesToEntities<TaskDto>(rawData);
			var taskDtos = tasks as TaskDto[] ?? tasks.ToArray();

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			taskDtos.Should().NotBeNull();
			taskDtos.Count().Should().BeGreaterOrEqualTo(1);
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(0)]
		public async Task GetDashboardTasks_ForUnexistingDashboard_ReturnsResourceNotFound(int dashboardId)
		{
			// Arrange
			var requestUri = $"{dashboardId}/tasks";

			// Act
			var response = await _httpClient.GetAsync(requestUri);
			var data = response.Content.ReadAsStringAsync().Result;

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
			data.Should().Be(ResourceMessages.ResourceNotFound);
		}

		[Theory]
		[InlineData("a")]
		[InlineData("dashboardId")]
		public async Task GetDashboardTasks_ForInvalidDashboardId_ReturnsBadRequest(string dashboardId)
		{
			// Arrange
			var requestUri = $"{dashboardId}/tasks";

			// Act
			var response = await _httpClient.GetAsync(requestUri);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}
	}
}
