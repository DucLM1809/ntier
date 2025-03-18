using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Ntier.API.Filter
{
    public class CamelCaseSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Properties != null)
            {
                var propertiesCopy = schema.Properties.ToDictionary(
                    kvp => ChangeToCamelCase(kvp.Key),
                    kvp => kvp.Value
                );
                schema.Properties = propertiesCopy;
            }
        }

        private string ChangeToCamelCase(string propertyName)
        {
            return char.ToLowerInvariant(propertyName[0]) + propertyName[1..];
        }
    }
}
