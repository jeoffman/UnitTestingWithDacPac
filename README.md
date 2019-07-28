# UnitTestingWithDacPac

Back in the day...made a lot of SQL databases with stored procedures to do some business logic.

When I write projects to use those stored procedures from C# then I end up smearing my business logic between SQL and C#

Now I am in a place where I want to write "unit tests" (more like "functional tests" or nearly "integrations tests" even) but I am too lazy to port all the sprocs down into a proper EF repo so that I can test/mock them in C#.

This is a stub project that uses MS LocalDB to spin up a temporary DB Engine, then apply a DACPAC to that engine, and finally to run am XUnit test that hits the database.