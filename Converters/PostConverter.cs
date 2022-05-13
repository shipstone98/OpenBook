using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Shipstone.OpenBook.Models;

namespace Shipstone.OpenBook.Converters
{
    public class PostConverter : JsonConverter<Post>
    {
        public override Post Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Post post = new();
            String propertyName = null;

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                case JsonTokenType.PropertyName:
                    propertyName = reader.GetString();
                    break;

                case JsonTokenType.String:
                    if (!(propertyName is null) && propertyName.ToLower().Equals("content"))
                    {
                        post.Content = reader.GetString();
                    }

                    break;
                }
            }

            return post;
        }

        public override void Write(Utf8JsonWriter writer, Post value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteNumber("id", value.Id);
            writer.WriteString("creator", value.Creator.UserName);
            writer.WriteString("createdUtc", value.CreatedUtc);
            writer.WriteString("content", value.Content);
            writer.WriteEndObject();
        }
    }
}
