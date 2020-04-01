// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.Graph;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GraphTutorial
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            InitializeComponent();
        }

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

            // Add the events to the list view
            CalendarList.ItemsSource = events.CurrentPage.ToList();
        }
    }
}