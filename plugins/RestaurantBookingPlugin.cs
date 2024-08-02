#pragma warning disable SKEXP0050

using System.ComponentModel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Web;
using Microsoft.SemanticKernel.Plugins.Web.Bing;// add another class that makes a restaurant booking

public class RestaurantBookingPlugin
{
    [KernelFunction("restaurant_booking")]
    [Description("Books a table at a restaurant.")]
    [return: Description("Booking confirmation.")]
    public async Task<string> BookRestaurant([Description("The restaurant name")] string restaurant_name, [Description("The number of people")] int number_of_people, [Description("The date")] string date, [Description("The time")] string time)
    {
        // CHALLENGE 2.2
        // Write a native function that calls a REST API (e.g. OpenTable) to automatically book a table at a restaurant.
        var kernel = Kernel.CreateBuilder().Build();
        
        var bingConnector = new BingConnector("740c2934c2374a0d9666decce132c9e2");
        kernel.ImportPluginFromObject(new WebSearchEnginePlugin(bingConnector), "bing");

        var function = kernel.Plugins["bing"]["search"];
        var bingResult = await kernel.InvokeAsync(function, new() { ["query"] = "Book a table at " + restaurant_name + " for " + number_of_people + " people on " + date + " at " + time });
        return bingResult.ToString();
    }
}