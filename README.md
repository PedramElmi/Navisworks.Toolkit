# Community.Navisworks.Toolkit

Community.Navisworks.Toolkit is a .NET Framework class library for Autodesk Navisworks plugin development. It adds focused extension methods around common API tasks such as exporting model properties, traversing item hierarchies, working with user-defined property categories, resolving model item icons, and flattening selection sets.

The assembly namespace is `Community.Navisworks.Toolkit`. The library is intended to be referenced from Navisworks add-ins and other in-process tooling.

## Features

- Export `ModelItem` properties to dictionaries or JSON files.
- Export full `ModelItem` hierarchies, including child nodes.
- Convert `PropertyCategory`, `DataProperty`, and `VariantData` objects into CLR-friendly values.
- Update, upsert, and remove user-defined property categories through the Navisworks COM bridge.
- Find property categories and property names shared across multiple selected items.
- Recursively collect `SelectionSet` objects from `DocumentSelectionSets` and nested folders.
- Retrieve embedded icons for common Navisworks model item types.

## API overview

The current public surface is organized around these types:

- `ModelItemExtensions`
  Serialization, hierarchy export, common-property analysis, icon lookup, and custom property mutation.
- `PropertyCategoryExtensions`
  Category and property name extraction plus dictionary conversion.
- `DataPropertyExtensions`
  Dictionary conversion for property collections.
- `VariantDataExtensions`
  Conversion helpers for `VariantData` values, including numeric parsing.
- `DocumentSelectionSetsExtentions` and `FolderItemExtentions`
  Recursive retrieval of `SelectionSet` instances.
- `CustomPropertyCategory`
  A simple wrapper for user-defined property category payloads.
- `IconType` and `IconImage`
  Classification and embedded bitmap lookup for supported item icons.

## Requirements

- Autodesk Navisworks Manage installed locally.
- .NET Framework 4.8.
- `x64` build target.
- Access to these Navisworks assemblies:
  - `Autodesk.Navisworks.Api`
  - `Autodesk.Navisworks.ComApi`
  - `Autodesk.Navisworks.Interop.ComApi`

The project currently defaults to this installation path when no override is provided:

```text
C:\Program Files\Autodesk\Navisworks Manage 2027
```

## Build configuration

If your Navisworks version or installation directory is different, create or update `src/Community.Navisworks.Toolkit.csproj.user` so MSBuild can resolve the Autodesk references.

Example:

```xml
<?xml version="1.0" encoding="utf-8"?>
<Project ToolsVersion="15.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <NavisworksVersion>2027</NavisworksVersion>
    <NavisworksInstallationPath>C:\Program Files\Autodesk\Navisworks Manage $(NavisworksVersion)</NavisworksInstallationPath>
  </PropertyGroup>
</Project>
```

The project also depends on `Newtonsoft.Json` for serialization.

## Getting started

Reference the compiled assembly from your Navisworks plugin project and import the toolkit namespace:

```csharp
using Community.Navisworks.Toolkit;
```

## Usage examples

### Export model item properties to JSON

```csharp
using Autodesk.Navisworks.Api;

ModelItem item = Autodesk.Navisworks.Api.Application.ActiveDocument.CurrentSelection.SelectedItems.First;
item.Serialize(@"C:\temp\item-properties.json");
```

This serializes the model item's property categories and properties through `Newtonsoft.Json`.

### Export a model hierarchy

```csharp
using Autodesk.Navisworks.Api;

ModelItem root = Autodesk.Navisworks.Api.Application.ActiveDocument.Models.RootItem;
root.SerializeHierarchy(@"C:\temp\model-tree.json");
```

Hierarchy exports include a `Children` entry for nested items.

### Work with in-memory dictionaries

```csharp
using System.Collections.Generic;

IDictionary<string, object> properties = item.PropertyCategories.ToDictionary();
```

### Find shared categories and properties

```csharp
using Autodesk.Navisworks.Api;
using System.Collections.Generic;

IEnumerable<ModelItem> items = Autodesk.Navisworks.Api.Application.ActiveDocument
    .CurrentSelection
    .SelectedItems;

IEnumerable<string> sharedCategories = items.GetIntersectedCategoriesDisplayName();
IEnumerable<string> sharedProperties = items.GetIntersectedPropertiesDisplayName("Item");
```

### Add or merge a custom property category

```csharp
using Autodesk.Navisworks.Api;

var category = new CustomPropertyCategory("QA")
{
    Properties = new DataPropertyCollection
    {
        new DataProperty("Status", "Status", new VariantData("Approved")),
        new DataProperty("CheckedBy", "Checked By", new VariantData("Coordinator"))
    }
};

item.Upsert(category);
```

Available mutation methods:

- `Update` overwrites the target user-defined category.
- `Upsert` merges the provided properties with an existing category.
- `Remove` deletes the user-defined category from the model item.

### Flatten document selection sets

```csharp
using Autodesk.Navisworks.Api;

IEnumerable<SelectionSet> selectionSets = Autodesk.Navisworks.Api.Application
    .ActiveDocument
    .SelectionSets
    .GetSelectionSets();
```

### Resolve a model item icon

```csharp
using System.Windows.Media.Imaging;

IconType iconType = item.GetIconType();
BitmapImage icon = item.GetIcon();
```

The current code maps these icon types:

- `File`
- `Layer`
- `Collection`
- `CompositeObject`
- `InsertGroup`
- `Geometry`
- `Unidentified`

## Notes on custom property support

Custom property category writes are implemented through `ComApiBridge`, which is the key mechanism this library uses to extend `ModelItem` instances with user-defined properties.

`CustomPropertyCategory` always uses the internal category name `LcOaPropOverrideCat`, while the visible category label is controlled by `DisplayName`.

## Contributing

Issues and pull requests are welcome. The current project scope is intentionally focused on reusable helper methods for Navisworks API development rather than a full plugin framework.

## License

This project is licensed under the [MIT License](LICENSE).