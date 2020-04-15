<!-- markdownlint-disable MD002 MD041 -->

In this exercise you will incorporate the Microsoft Graph into the application. For this application, you will use the [Microsoft Graph Client Library for .NET](https://github.com/microsoftgraph/msgraph-sdk-dotnet) to make calls to Microsoft Graph.

## Get calendar events from Outlook

1. Open **CalendarPage.xaml** in the **GraphTutorial** project and replace its contents with the following.

    ```xaml
    <?xml version="1.0" encoding="utf-8" ?>
    <ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
                 xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
                 Title="Calendar"
                 x:Class="GraphTutorial.CalendarPage">
        <ContentPage.Content>
            <StackLayout>
                <Editor x:Name="JSONResponse"
                        IsSpellCheckEnabled="False"
                        HorizontalOptions="FillAndExpand"
                        VerticalOptions="FillAndExpand"/>
            </StackLayout>
        </ContentPage.Content>
    </ContentPage>
    ```

1. Open **CalendarPage.xaml.cs** and add the following `using` statements at the top of the file.

    ```csharp
    using Microsoft.Graph;
    using Newtonsoft.Json;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    ```

1. Add the following function to the `CalendarPage` class to get the user's events and display the JSON response.

    ```csharp
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Get the events
        var events = await App.GraphClient.Me.Events.Request()
            .Select(e => new
            {
                e.Subject,
                e.Organizer,
                e.Start,
                e.End
            })
            .OrderBy("createdDateTime DESC")
            .GetAsync();

        // Temporary
        JSONResponse.Text = JsonConvert.SerializeObject(events.CurrentPage, Formatting.Indented);
    }
    ```

    Consider what the code in `OnAppearing` is doing.

    - The URL that will be called is `/v1.0/me/events`.
    - The `Select` function limits the fields returned for each events to just those the view will actually use.
    - The `OrderBy` function sorts the results by the date and time they were created, with the most recent item being first.

1. Run the app, sign in, and click the **Calendar** navigation item in the menu. You should see a JSON dump of the events on the user's calendar.

## Display the results

Now you can replace the JSON dump with something to display the results in a user-friendly manner.

Start by creating a [binding value converter](/xamarin/xamarin-forms/xaml/xaml-basics/data-binding-basics#binding-value-converters) to convert the [dateTimeTimeZone](/graph/api/resources/datetimetimezone?view=graph-rest-1.0) values returned by Microsoft Graph into the date and time formats the user expects.

1. Right-click the **Models** folder in the **GraphTutorial** project and select **Add**, then **Class...**. Name the class `GraphDateTimeTimeZoneConverter` and select **Add**.

1. Replace the entire contents of the file with the following.

    :::code language="csharp" source="../demo/GraphTutorial/GraphTutorial/Models/GraphDateTimeTimeZoneConverter.cs" id="DateTimeConverterSnippet":::

1. Replace the entire contents of **CalendarPage.xaml** with the following.

    :::code language="xaml" source="../demo/GraphTutorial/GraphTutorial/CalendarPage.xaml":::

    This replaces the `Editor` with a `ListView`. The `DataTemplate` used to render each item uses the `GraphDateTimeTimeZoneConverter` to convert the `Start` and `End` properties of the event to a string.

1. Open **CalendarPage.xaml.cs** and remove the following lines from the `OnAppearing` function.

    ```csharp
    // Temporary
    JSONResponse.Text = JsonConvert.SerializeObject(events.CurrentPage, Formatting.Indented);
    ```

1. In their place, add the following code.

    ```csharp
    // Add the events to the list view
    CalendarList.ItemsSource = events.CurrentPage.ToList();
    ```

1. Run the app, sign in, and click the **Calendar** navigation item. You should see the list of events with the **Start** and **End** values formatted.

    ![A screenshot of the table of events](./images/calendar-page.png)
