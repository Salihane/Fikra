using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Fikra.API.IntegrationTests.Helpers
{
	public static class FikraConverter
	{
		public static IEnumerable<T> FromBytesToEntities<T>(byte[] bytes)
		{
			var json = Encoding.UTF8.GetString(bytes);
			return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
		}

		public static T FromBytesToEntitY<T>(byte[] bytes)
		{
			var json = Encoding.UTF8.GetString(bytes);
			return JsonConvert.DeserializeObject<T>(json);
		}

		public static byte[] FromEntityToBytes<T>(T entity)
		{
			var json = JsonConvert.SerializeObject(entity);
			return Encoding.UTF8.GetBytes(json);
		}

		public static ByteArrayContent ToByteArrayContent(byte[] contentData)
		{
			var content = new ByteArrayContent(contentData);
			content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
			return content;
		}

	}
}
