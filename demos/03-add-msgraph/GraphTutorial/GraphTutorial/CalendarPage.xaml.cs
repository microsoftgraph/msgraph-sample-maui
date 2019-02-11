using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GraphTutorial
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage ()
        {
            InitializeComponent ();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Get the events
            var events = await App.GraphClient.Me.Events.Request()
                .Select("subject,organizer,start,end")
                .OrderBy("createdDateTime DESC")
                .GetAsync();

            // Temporary
            JSONResponse.Text = JsonConvert.SerializeObject(events.CurrentPage, Formatting.Indented);
        }
    }
}