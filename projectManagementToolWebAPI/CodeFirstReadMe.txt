To update datbase run the following command on the package manager console
Update-Database -verbose
Update-Database –TargetMigration: AddBlogUrl -verbose

To Enable-Migrations 
Add-Migration
Enable-Migrations
Enable-Migrations -verbose -Force

command in Package Manager Console. This creates an empty migration with the current model as a snapshot. and then Run the Update-Database
Add-Migration InitialCreate –IgnoreChanges 

To Ignore Changes run the code below then the Update-Database
Add-Migration -IgnoreChanges