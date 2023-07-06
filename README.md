# Navisworks.Toolkit

Navisworks.Toolkit is an open-source assembly library for Autodesk Navisworks. It provides additional functionalities for Navisworks API developers who want to create plugins, extend the capabilities of the software, and work more efficiently.

This library includes extension methods for the Navisworks API objects that allow developers to:

*   Serialize ModelItem Property Categories data into a JSON format for easier data transfer and manipulation.
*   Manage new custom categories and properties with the API, enabling more complex and customized workflows within Navisworks.

Navisworks.Toolkit is an MIT-licensed project available on GitHub. It is designed with good software design principles in mind, such as modularity, extensibility, and maintainability, making it easy to use, flexible, and adaptable to future changes.


## Getting Started
---

To get started with Navisworks.Toolkit, simply download the latest release from the [releases page](https://github.com/pedramelmi/Navisworks.Toolkit/releases) or clone the repository to your local machine.

The project includes detailed documentation and examples demonstrating how to use the various extension methods and functionalities provided by the library. Additionally, the source code is well-documented and follows industry-standard conventions, making it easy to understand and modify as needed.

## Contributing
---

Contributions to Navisworks.Toolkit are welcome and encouraged. If you find a bug or want to suggest a new feature, please open an issue on the GitHub repository. If you want to contribute code, please fork the repository and submit a pull request with your changes.

### `NavisworksInstallationPath` Property

The project supports configuration of the Navisworks installation path to locate the required API references, such as Autodesk.Navisworks.Api.dll file. By default, it uses the path `"$(ProgramFiles)\Autodesk\Navisworks Manage 2023"`. Alternatively, you can set the Navisworks installation path in the user project option file.

`Community.Navisworks.Toolkit.csproj.user` sample
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="Current" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <NavisworksInstallationPath>C:\YourInstallationPath\Navisworks Manage 2024</NavisworksInstallationPath>
  </PropertyGroup>
</Project>
```


## License
---

Navisworks.Toolkit is released under the MIT license. Please see the [LICENSE](https://github.com/pedramelmi/Navisworks.Toolkit/blob/main/LICENSE) file for more details.

## Acknowledgments
---

Navisworks.Toolkit is built upon the Navisworks API and is inspired by the work of other open-source projects in the Navisworks development community. We thank the Autodesk team for their continued development and support of Navisworks, as well as the many developers who have contributed to the Navisworks development community over the years.