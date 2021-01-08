// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

// <NavMenuItemSnippet>
namespace GraphTutorial.Models
{
    public enum MenuItemType
    {
        Welcome,
        Calendar,
        NewEvent
    }

    public class NavMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
// </NavMenuItemSnippet>
