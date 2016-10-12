# Ready-to-go-MVC
A ready to go MVC solution wired up with Ninject for DI, EF repository using code first &amp; logging aspects

## Generic Repository Pattern
There is a generic IRepository interface and a Sql implementation of this repostory.
You then just need to define the modal and add them to the Ready2GoContext class which is injected into services.

## Use code first migrations
You can create migrations each time the model changes. In package manager run 'Add-Migration name' where 'name' is the name of your migration. Name them
something descriptive so that you can refer back to the migration easily later for rollbacks ect.
'update-database' will migrate and add any seed data you may add.


## Packages Used
Ninject.MVC5