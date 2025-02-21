using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

public static class SessionExtensions
{
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
    }

    public static void SetDecimal(this ISession session, string key, decimal value)
    {
        session.SetString(key, value.ToString());
    }

    public static decimal GetDecimal(this ISession session, string key)
    {
        var value = session.GetString(key);
        if (string.IsNullOrEmpty(value))
        {
            return 0; // Return a default value, or throw an exception if you prefer
        }
        return decimal.Parse(value); // You can also use TryParse to handle invalid cases
    }
}
