// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");


DateTime CurrentDate = DateTime.Now;
Console.WriteLine($"The current time is {DateTime.Now}");

static TimeSpan days_until_christmas(DateTime date) {
    DateTime christmas = new DateTime(date.Year, 12, 25);
    if (date > christmas) {
        christmas = new DateTime(date.Year + 1, 12, 25);
    }
    return christmas - date;
}

TimeSpan ChristmasDays = days_until_christmas(CurrentDate);
Console.WriteLine($"There are {ChristmasDays.Days} days until Christmas.");
 