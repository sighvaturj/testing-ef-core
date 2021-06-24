# testing-ef-core
*From: Price, Mark J.. C# 9 and .NET 5 Modern Cross-Platform Development (Chapter 11). Packt Publishing.*

Working with EntityFrameworkCore for database connection and mapping in .NET 5.

## Setup

**1. Clone the repository to a directory with SSH or HTTPS connection to GitHub - or simply download a ZIP file.**

`$ git clone git@github.com/sighvaturj/testing-ef-core.git`

`$ git clone https://github.com/sighvaturj/testing-ef-core.git`

**2. Remember to grab a copy of the SDK for .NET 5.0 if you don't have it installed on your machine.**

[Download .NET SDK](https://dotnet.microsoft.com/download)

**3. To build and run the program, use the following commands in a terminal where you saved/extracted the project.**

`$ dotnet build`
`$ dotnet run`

**4. Run the program by providing a number (i.e. '100') in the console.**

```Enter a minimum for units in stock: 100```

**5. The main method of 'Program.cs' includes other two methods that can be run by uncommenting them.**

```
        static void Main(string[] args)
        {
                // QueryingCategories();
                FilteredIncludes();
                // QueryingProducts();
        }
```
