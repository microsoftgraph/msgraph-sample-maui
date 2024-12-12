---
page_type: sample
description: This sample demonstrates how to use the Microsoft Graph .NET SDK to access data in Office 365 from .NET MAUI apps.
products:
- ms-graph
- office-exchange-online
languages:
- csharp
---

# Microsoft Graph sample .NET MAUI app

[![.NET](https://github.com/microsoftgraph/msgraph-sample-maui/actions/workflows/dotnet.yml/badge.svg)](https://github.com/microsoftgraph/msgraph-sample-maui/actions/workflows/dotnet.yml) ![License.](https://img.shields.io/badge/license-MIT-green.svg)

This sample demonstrates how to use the Microsoft Graph .NET SDK to access data in Office 365 from .NET MAUI apps.

> **NOTE:** This sample was originally built from a tutorial published on the [Microsoft Graph tutorials](https://learn.microsoft.com/graph/tutorials) page. That tutorial has been removed.

## Prerequisites

To run the completed project in this folder, you need the following:

- Visual Studio 2022 Version 17.12 or later (Windows) or Visual Studio for Mac Version 14.4.2 or later (MacOS).
- Either a personal Microsoft account with a mailbox on Outlook.com, or a Microsoft work or school account.

If you don't have a Microsoft account, there are a couple of options to get a free account:

- You can [sign up for a new personal Microsoft account](https://signup.live.com/signup?wa=wsignin1.0&rpsnv=12&ct=1454618383&rver=6.4.6456.0&wp=MBI_SSL_SHARED&wreply=https://mail.live.com/default.aspx&id=64855&cbcxt=mai&bk=1454618383&uiflavor=web&uaid=b213a65b4fdc484382b6622b3ecaa547&mkt=E-US&lc=1033&lic=1).
- You can [sign up for the Microsoft 365 Developer Program](https://developer.microsoft.com/microsoft-365/dev-program) to get a free Office 365 subscription.

## Register a web application with the Azure Active Directory admin center

1. Open your browser and navigate to the [Microsoft Entra admin center](https://entra.microsoft.com/). Sign in with a **Work or School Account**.

1. Select **Applications** in the left-hand navigation bar, then select **App registrations**.

1. Select **New registration**. On the **Register an application** page, set the values as follows.

    - Set **Name** to `.NET MAUI Graph Sample`.
    - Set **Supported account types** to **Accounts in any organizational directory and personal Microsoft accounts**.
    - Leave **Redirect URI** empty.

1. Select **Register**. On the **.NET MAUI Graph Sample** page, copy the value of the **Application (client) ID** and save it, you will need it in the next step.

1. Select **Authentication** under **Manage**.

1. Select **Add a platform**, then select **Mobile and desktop applications**.

1. Enable the value that starts with `msal`, then select **Configure**.

## Configure the sample

1. Replace `YOUR_CLIENT_ID_HERE` with your **Application (client) ID** value in the following files:

    - [appSettings.json](GraphMAUI/appSettings.json)
    - [MsalActivity.cs](GraphMAUI/Platforms/Android/MsalActivity.cs)

## Run the sample

Open **GraphMAUI.sln** in Visual Studio or Visual Studio for Mac and press **F5** to build and run the sample.

## Code of conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Disclaimer

**THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.**
