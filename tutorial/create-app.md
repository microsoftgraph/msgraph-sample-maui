<!-- markdownlint-disable MD002 MD041 -->

Open Visual Studio, and select **File > New > Project**. In the **New Project** dialog, do the following:

1. Select **Visual C# > Cross-Platform**.
1. Select **Mobile App (Xamarin.Forms)**.
1. Enter **GraphTutorial** for the Name of the project.

![Visual Studio 2017 create new project dialog](images/new-project-dialog.png)

> [!IMPORTANT]
> Ensure that you enter the exact same name for the Visual Studio Project that is specified in these lab instructions. The Visual Studio Project name becomes part of the namespace in the code. The code inside these instructions depends on the namespace matching the Visual Studio Project name specified in these instructions. If you use a different project name the code will not compile unless you adjust all the namespaces to match the Visual Studio Project name you enter when you create the project.

Select **OK**. In the **New Cross Platform App** dialog, select the **Blank** template, and ensure that the **Code Sharing Strategy** selection is **.NET Standard**. If you plan to skip a specific platform, you can unselect it now under **Platforms**. Select **OK** to create the solution.

![Visual Studio 2017 new cross platform app dialog](images/new-cross-platform-app-dialog.png)

Before moving on, install some additional NuGet packages that you will use later.

- [Microsoft.Identity.Client](https://www.nuget.org/packages/Microsoft.Identity.Client/) to handle Azure AD authentication and token management.
- [Microsoft.Graph](https://www.nuget.org/packages/Microsoft.Graph/) for making calls to the Microsoft Graph.

Select **Tools > NuGet Package Manager > Package Manager Console**. In the Package Manager Console, enter the following commands.

```Powershell
Install-Package Microsoft.Identity.Client -Version 2.7.0 -Project GraphTutorial
Install-Package Xamarin.Android.Support.Compat -Version 27.0.2.1 -Project GraphTutorial.Android
Install-Package Microsoft.Identity.Client -Version 2.7.0 -Project GraphTutorial.Android
Install-Package Microsoft.Identity.Client -Version 2.7.0 -Project GraphTutorial.iOS
Install-Package Microsoft.Graph -Version 1.12.0 -Project GraphTutorial
```