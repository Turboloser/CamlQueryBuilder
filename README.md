# CamlQueryBuilder
A simple CAML query builder for C# using builder and fluent patterns to easily create querys which are customizable at runtime.

## Install:

Simple install via NuGet Package Manager:
```
Install-Package CamlQueryBuilder -Version 1.0.1
```

Or use the .NET CLI:
```
dotnet add package CamlQueryBuilder --version 1.0.1
```

## Usage:

Using the CAMLQueryBuilder is very simple. You only need rudimentary knowledge about the structure of Caml querys. Let's assume that user data is to be exported from a SharePoint list. We are only interested in users with the last name "Doe" and the first name "John" or "Jane". Furthermore the users should not be born on 01/01/1970, because the administrator uses this birthdate for obsolete accounts. So the caml query could look like this:

```c#
string camlQuery = new CamlQueryBuilderRoot(new And(new Eq("LastName", "Doe"),
                                                    new Or(new Eq("FirstName", "John"),
                                                           new Eq("FirstName", "Jane")),
                                                    new Neq("DateOfBirth", "01.01.1970", CamlQueryBuilderRoot.ValueType.DateTime)))
    .SetRecursiveAll()
    .SetRowLimit(10)
    .OrderBy("DateOfBirth", CamlQueryBuilderRoot.SortingOrder.Ascending)
    .FormatOutput()
    .ToCamlQueryString();
```

```xml
<View Scope="RecursiveAll">
  <Query>
    <OrderBy>
      <FieldRef Name="DateOfBirth" Ascending="TRUE" />
    </OrderBy>
    <Where>
      <And>
        <Eq>
          <FieldRef Name="FirstName" />
          <Value Type="Text">Doe</Value>
        </Eq>
        <And>
          <Or>
            <Eq>
              <FieldRef Name="LastName" />
              <Value Type="Text">John</Value>
            </Eq>
            <Eq>
              <FieldRef Name="LastName" />
              <Value Type="Text">Jane</Value>
            </Eq>
          </Or>
          <Neq>
            <FieldRef Name="DateOfBirth" />
            <Value Type="DateTime">01.01.1970</Value>
          </Neq>
        </And>
      </And>
    </Where>
  </Query>
  <RowLimit>10</RowLimit>
</View>
```

As you can see, first a CamlQueryBuilderRoot object is instantiated, which is then filled with elements. These can be logical joins (And, Or) or operators (Eq, Neq, ...). Logical joins can connect 1-n elements with an And or Or operation. All operators, on the other hand, reflect the operations of a Caml querys with the same name. For more information on operators see: https://docs.microsoft.com/de-de/sharepoint/dev/schema/query-schema

By default, comparisons are assumed to be text comparisons, but other data types are available. This is used when comparing the date of birth, which uses DateTime. To limit the number of results, a limit is specified via the SetRowLimit method. The results are further sorted in ascending order by date of birth. Since we want to take a closer look at the generated Caml query, we also call the FormatOutput method, which makes the query much more readable. The call of the ToCamlQueryString method completes the configuration of the Caml query and creates the query.

Lets have a look at a more complex example:

```c#
//Testdata
string sharePointFieldName_FirstName = "FirstName";
string[] namesToCheck = new string[] { "John", "Jane", "Max", "Powel", "Arthur" };

//Create the CamlQuery
string complexCamlQuery = new CamlQueryBuilderRoot(new Or(namesToCheck.Select(name => new Eq(sharePointFieldName_FirstName, name))))
    .SetRecursiveAll()
    .OrderBy("FirstName", CamlQueryBuilderRoot.SortingOrder.Ascending)
    .OrderBy("Account", CamlQueryBuilderRoot.SortingOrder.Descending)
    .GroupBy("FirstName")
    .AddViewField("FirstName")
    .AddViewField("LastName")
    .AddViewField("Account")
    .AddViewField("DateOfBirth")
    .SetRowLimit(10)
    .FormatOutput()
    .ToCamlQueryString();
```

In this example, we have a list of first names that we want to search the SharePoint list for. All users with any of those first names are to be output. The users can also be in subfolders of the SharePoint list, which is why the SetRecursiveAll method is called. This causes the query to also look into subfolders for results. Furthermore we want to group the users by first name and sort them by first name (primary) and account (secondary). In this case we are also not interested in all fields of the list, but only in some selected fields (FirstName, LastName, Account, DateOfBirth).

```xml
<View Scope="RecursiveAll">
  <Query>
    <OrderBy>
      <FieldRef Name="FirstName" Ascending="TRUE" />
      <FieldRef Name="Account" Ascending="FALSE" />
    </OrderBy>
    <GroupBy>
      <FieldRef Name="FirstName" />
    </GroupBy>
    <ViewFields>
      <FieldRef Name="FirstName" />
      <FieldRef Name="LastName" />
      <FieldRef Name="Account" />
      <FieldRef Name="DateOfBirth" />
    </ViewFields>
    <Where>
      <Or>
        <Eq>
          <FieldRef Name="FirstName" />
          <Value Type="Text">John</Value>
        </Eq>
        <Or>
          <Eq>
            <FieldRef Name="FirstName" />
            <Value Type="Text">Jane</Value>
          </Eq>
          <Or>
            <Eq>
              <FieldRef Name="FirstName" />
              <Value Type="Text">Max</Value>
            </Eq>
            <Or>
              <Eq>
                <FieldRef Name="FirstName" />
                <Value Type="Text">Powel</Value>
              </Eq>
              <Eq>
                <FieldRef Name="FirstName" />
                <Value Type="Text">Arthur</Value>
              </Eq>
            </Or>
          </Or>
        </Or>
      </Or>
    </Where>
  </Query>
  <RowLimit>10</RowLimit>
</View>
```
