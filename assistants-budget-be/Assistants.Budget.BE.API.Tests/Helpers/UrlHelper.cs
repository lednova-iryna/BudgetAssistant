using Microsoft.AspNetCore.WebUtilities;

namespace Assistants.Budget.BE.API.Tests.Helpers;

public class UrlHelper
{
    public static string ObjectToQueryString(string url, object queryObj) =>
        QueryHelpers.AddQueryString(
            url,
            queryObj
                .GetType()
                .GetProperties()
                .Select(x => KeyValuePair.Create(x.Name, ParseTypes(x.GetValue(queryObj))))
                .AsEnumerable()
        );

    private static string? ParseTypes(object? value)
    {
        if (value?.GetType() == typeof(DateTime))
        {
            return ((DateTime)value).ToString("u").Replace(" ", "T");
        }
        return value?.ToString();
    }
}
