// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using UIKit;

namespace GraphMAUI;

/// <summary>
/// Main entry point for iOS.
/// </summary>
public class Program
{
    /// <summary>
    /// This is the main entry point of the application.
    /// </summary>
    /// <param name="args">The args.</param>
    public static void Main(string[] args)
    {
        // if you want to use a different Application Delegate class from "AppDelegate"
        // you can specify it here.
        UIApplication.Main(args, null, typeof(AppDelegate));
    }
}
