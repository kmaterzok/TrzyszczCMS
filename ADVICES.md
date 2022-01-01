# Updating EF Core Table models and *DbContext*
1. Make sure the whole solution is able to be compiled. Otherwise the update won't be done and will fail.
2. Apply all necessary changes in the database so entities will be compliant with latest migrations. It might be done through executing migration on the database.
3. Set *DAL* as startup project and select it as launched project.
4. Open *Package Manager Console* and select *DAL* as *Default project* for this console's instance.
5. Execute the following piece of code in *Package Manager Console*: ```Scaffold-DbContext "Host=localhost;Username=postgres;Password=ZAQ!2wsx;Database=trzyszcz_cms" Npgsql.EntityFrameworkCore.PostgreSQL -force -OutputDir Models/Database``` Of course you can customise the connection string so it will connect to your database with adequate credentials.
6. Rename the obtained *DbContext* class to ```CmsDbContext```. Remove ```OnConfiguring``` method and ```partial void OnModelCreatingPartial(ModelBuilder modelBuilder)``` method.
7. You're done.
