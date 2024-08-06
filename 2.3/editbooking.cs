using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Plugins.Core;
using Microsoft.SemanticKernel.Planning.Handlebars;
using System.Threading.Tasks;
using Microsoft.SemanticKernel.Plugins;


// Create a new kernel
IKernelBuilder builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(
    "gpt-4o",
    "https://bootcampsweden.openai.azure.com//",
    "188d05bf71ae48a58abd9b85c6fb1b88",
    "gpt-4o");
Kernel kernel = builder.Build();


//-------------------------------
// import plugins
kernel.ImportPluginFromType<SearchPlugin>();
kernel.ImportPluginFromType<RestaurantBookingPlugin>();
KernelPlugin prompts = kernel.ImportPluginFromPromptDirectory("C:\\ianweek6update3\\aibootcamp-week6\\prompts");
//-------------------------------



public class MultiRestaurantBookingPlugin : IKernelPlugin
{
    private readonly List<RestaurantBookingModel> _bookings;

    public MultiRestaurantBookingPlugin(LoggerFactory loggerFactory, List<RestaurantBookingModel> bookings)
    {
        _bookings = bookings;
    }

    [KernelFunction("get_bookings")]
    [Description("Gets a list of restaurant bookings and their details")]
    [return: Description("An array of restaurant bookings")]
    public async Task<List<RestaurantBookingModel>> GetBookingsAsync()
    {
        // Simulate async operation
        await Task.Delay(100);
        return _bookings;
    }

    [KernelFunction("add_booking")]
    [Description("Adds a new restaurant booking")]
    public async Task AddBookingAsync(RestaurantBookingModel booking)
    {
        // Simulate async operation
        await Task.Delay(100);
        _bookings.Add(booking);
    }

    [KernelFunction("delete_booking")]
    [Description("Deletes a restaurant booking by ID")]
    public async Task DeleteBookingAsync(int bookingId)
    {
        // Simulate async operation
        await Task.Delay(100);
        RestaurantBookingModel? booking = _bookings.Find(b => b.Id == bookingId);
        if (booking != null)
        {
            _bookings.Remove(booking);
        }
    }

    [KernelFunction("edit_booking")]
    [Description("Edits an existing restaurant booking")]
    public async Task EditBookingAsync(RestaurantBookingModel updatedBooking)
    {
        // Simulate async operation
        await Task.Delay(100);
        RestaurantBookingModel? booking = _bookings.Find(b => b.Id == updatedBooking.Id);
        if (booking != null)
        {
            booking.Date = updatedBooking.Date;
            booking.Time = updatedBooking.Time;
            booking.NumberOfPeople = updatedBooking.NumberOfPeople;
            booking.SpecialRequests = updatedBooking.SpecialRequests;
        }
    }
}


public class RestaurantBookingModel
{
    public int Id { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public int NumberOfPeople { get; set; }
    public string SpecialRequests { get; set; }
}

// Create a list of restaurant bookings
List<RestaurantBookingModel> bookings = new List<RestaurantBookingModel>
{
    new() { Id = 1, Date = "2024-10-01", Time = "18:00", NumberOfPeople = 2, SpecialRequests = "None" },
    new() { Id = 2, Date = "2024-10-02", Time = "19:30", NumberOfPeople = 4, SpecialRequests = "Vegetarian options" },
    new() { Id = 3, Date = "2024-10-03", Time = "20:00", NumberOfPeople = 6, SpecialRequests = "Gluten-free options" }
};

// create an instance of the MultiRestaurantBookingPlugin class and add it to the plugin collection using AddFromObject
kernel.Plugins.AddFromObject(new MultiRestaurantBookingPlugin(bookings));






