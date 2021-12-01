# Updating EF Core Table models and *DbContext*
1. Make sure the whole solution is able to be compiled. Otherwise the update won't be done and will fail.
2. Set *DAL* as startup project and select it as launched project.
3. Open *Package Manager Console* and select *DAL* as *Default project* for this console's instance.
4. Execute the following piece of code in *Package Manager Console*: ```Scaffold-DbContext "Host=localhost;Username=postgres;Password=ZAQ!2wsx;Database=trzyszcz_cms" Npgsql.EntityFrameworkCore.PostgreSQL -force -OutputDir Models/Database``` Of course you can customise the connection string so it will connect to your database with adequate credentials.
5. Rename the obtained *DbContext* class to ```CmsDbContext```. Remove ```OnConfiguring``` method and ```partial void OnModelCreatingPartial(ModelBuilder modelBuilder)``` method.
6. You're done.
