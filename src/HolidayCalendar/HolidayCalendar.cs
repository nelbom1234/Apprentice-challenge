using System;
using System.Collections;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;

namespace HolidayCalendar;
public class HolidayCalendar : IHolidayCalendar
{
  public bool IsHoliday(DateTime date)
  {
    // TODO - replace the below exception with your own implementation

    //convert the date to a string in our desired format for the url
    string dateString = date.ToString("yyyy-MM-dd");


    //format url with given dates
    string url = $"https://api.sallinggroup.com/v1/holidays/is-holiday?date={dateString}";
    Task<string> response = webRequest(url);

    string result = response.Result;

    if (result == "true")
    {
      return true;
    }
    else return false;

  }

  public async Task<string> webRequest(string url)
  {

    HttpClient client = new HttpClient();

    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

    //api authorization token
    request.Headers.Add("Authorization", "Bearer ce6d0fcf-9a9d-4124-8fc5-d6e9c8744b9b");

    HttpResponseMessage response = await client.SendAsync(request);
    response.EnsureSuccessStatusCode();
    string responseBody = await response.Content.ReadAsStringAsync();
    return responseBody;

  }

  public ICollection<DateTime> GetHolidays(DateTime startDate, DateTime endDate)
  {
    string startDateString = startDate.ToString("yyyy-MM-dd");
    string endDateString = endDate.ToString("yyyy-MM-dd");

    ICollection<DateTime> retList = new List<DateTime>();

    //format url with given dates
    string url = $"https://api.sallinggroup.com/v1/holidays?startDate={startDateString}&endDate={endDateString}&translation=en-us";
    Task<string> response = webRequest(url);

    string result = response.Result;


    //convert the json date to a JsonElement and then turn it into an array we can iterate on
    JsonElement data = JsonSerializer.Deserialize<JsonElement>(result);
    var dataArray = data.EnumerateArray();

    //Iterate on the array, checking the last value in each element if it is a holiday and if it is,
    //we convert the date in the first value to DateTime and add it to the return list
    foreach (var obj in dataArray)
    {
        var objProperties = obj.EnumerateObject();
        
        bool isHoliday = bool.Parse(objProperties.Last().Value.ToString());
        if (isHoliday)
        {
            DateTime date = DateTime.ParseExact(objProperties.First().Value.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            retList.Add(date);
        }
    }

    return retList;

  }
}
