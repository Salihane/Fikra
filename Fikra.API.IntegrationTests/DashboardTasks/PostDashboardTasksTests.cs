using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Fikra.API.Helpers;
using Fikra.API.IntegrationTests.Helpers;
using Fikra.API.Models.Task;
using Fikra.DAL;
using Fikra.Model.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace Fikra.API.IntegrationTests.DashboardTasks
{
    public class PostDashboardTasksTests : IClassFixture<FikraWebApplicationFactory<API.Startup>>
    {

		private readonly HttpClient _httpClient;
		private readonly FikraWebApplicationFactory<API.Startup> _factory;

		public PostDashboardTasksTests(FikraWebApplicationFactory<API.Startup> factory)
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
		[InlineData(3)]
		public async Task PostDashboardTask_ValidTaskForExistingDashboard_ReturnsCreated(int dashboardId)
		{
			// Arrange
			var requestUri = $"{dashboardId}/task";
			var taskEntity = _factory.DummyDataManager.CreateDashboardTask($"New Task {dashboardId}");
			var taskBytes = FikraConverter.FromEntityToBytes(taskEntity);
			var content = FikraConverter.ToByteArrayContent(taskBytes);

			// Act
			var response = await _httpClient.PostAsync(requestUri, content);
			var data = response.Content.ReadAsByteArrayAsync().Result;
			var createdDto = FikraConverter.FromBytesToEntitY<TaskDto>(data);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.Created);
			createdDto.Should().NotBeNull();
			createdDto.Name.Should().Be($"New Task {dashboardId}");
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(0)]
		public async Task PostDashboardTask_ValidTaskForNonExistingDashboard_ReturnsNotFound(int dashboardId)
		{
			// Arrange
			var requestUri = $"{dashboardId}/task";
			var taskEntity = _factory.DummyDataManager.CreateDashboardTask($"New Task {dashboardId}");
			var taskBytes = FikraConverter.FromEntityToBytes(taskEntity);
			var content = FikraConverter.ToByteArrayContent(taskBytes);

			// Act
			var response = await _httpClient.PostAsync(requestUri, content);
			var data = response.Content.ReadAsStringAsync().Result;
			var expectedMsg = string.Format(ResourceMessages.ResourceWithIdNotFound, nameof(Dashboard), dashboardId);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.NotFound);
			data.Should().Be(expectedMsg);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(3)]
		public async Task PostDashboardTask_NullTaskForExistingDashboard_ReturnsCreated(int dashboardId)
		{
			// Arrange
			var requestUri = $"{dashboardId}/task";
			var taskBytes = FikraConverter.FromEntityToBytes((TaskDto)null);
			var content = FikraConverter.ToByteArrayContent(taskBytes);

			// Act
			var response = await _httpClient.PostAsync(requestUri, content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(3)]
		public async Task PostDashboardTask_InvalidTaskForExistingDashboard_ReturnsUnprocessedEntity(int dashboardId)
		{
			// Arrange
			var requestUri = $"{dashboardId}/task";
			var taskEntity = _factory.DummyDataManager.CreateDashboardTask(taskName:"My Task");
			var taskBytes = FikraConverter.FromEntityToBytes(taskEntity);
			var content = FikraConverter.ToByteArrayContent(taskBytes);

			// Act
			var response = await _httpClient.PostAsync(requestUri, content);

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
		}

	}
}
