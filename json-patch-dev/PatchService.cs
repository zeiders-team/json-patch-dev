using System.Net.NetworkInformation;
using System.Text.Encodings.Web;
using System.Text.Json;
using Json.Patch;

namespace json_patch_dev
{
    public class PatchService
    {

        public string Compare (string jsonSource, string jsonDest)
        {
            var source = Parse(jsonSource).RootElement;
            var dest = Parse(jsonDest).RootElement;

			var patch = source.CreatePatch(dest);
			return Serialize(patch);

        }

        public string Patch (string jsonSource, string jsonPatch)
        {
			JsonPatch patch;
			try
			{
				patch = JsonSerializer.Deserialize<JsonPatch>(jsonPatch) ?? throw new ArgumentException(nameof(jsonPatch), "Unable to deserialize JSON Patch");
			}
			catch (Exception e)
			{
				throw;
			}

		    var source = Parse(jsonSource);
			var result = patch.Apply(source) ?? throw new NullReferenceException("Error applying patch");

			return Serialize(result);
        }

		private JsonDocument Parse(string json) 
		{
			JsonDocument jsonDoc;

		    try
		    {
			    jsonDoc = JsonDocument.Parse(json);
		    }
		    catch (Exception e)
		    {
                throw;
		    }

			return jsonDoc;
		}

		private string Serialize(object obj)
		{
			return  JsonSerializer.Serialize(obj, new JsonSerializerOptions
			{
				WriteIndented = true,
				Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
			});
		}
    }
}