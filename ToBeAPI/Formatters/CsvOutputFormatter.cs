using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using ToBeApi.Data.DTO;
using System.Text;

namespace ToBeApi.Data.Formatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type? type)
        {
            if (typeof(PostDTO).IsAssignableFrom(type) ||
                typeof(IEnumerable<PostDTO>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext
        context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<PostDTO>)
            {
                foreach (var company in (IEnumerable<PostDTO>)context.Object)
                {
                    FormatCsv(buffer, company);
                }
            }
            else
            {
                FormatCsv(buffer, (PostDTO)context.Object);
            }
            await response.WriteAsync(buffer.ToString());
        }

        private static void FormatCsv(StringBuilder buffer, PostDTO post)
        {
            buffer.AppendLine($"{post.Description},\"{post.Title},\"{post.Content}\"," +
                $"\"{post.CreatedAt}\",\"{post.UserId}\",\"{post.CategoryId}\"");
        }

    }
}
