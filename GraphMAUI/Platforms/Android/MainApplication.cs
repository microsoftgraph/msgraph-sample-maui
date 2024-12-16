// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Android.App;
using Android.Runtime;

namespace GraphMAUI;

/// <summary>
/// Entry point for Android.
/// </summary>
/// <param name="handle">The handle.</param>
/// <param name="ownership">The ownership.</param>
[Application]
public class MainApplication(IntPtr handle, JniHandleOwnership ownership) : MauiApplication(handle, ownership)
{
    /// <inheritdoc/>
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
