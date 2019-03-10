# .Net Core API Reference Application

This project is intended as a reference for building .Net Core APIs.

One thing it does differently than most of the into guides is that it does not follow "database first" or "code first" approach. It actually builds code and database independently, using specific SQL scripts for database and .Net objects for entities. .Net Core EF is used purely to map database tables to .Net entities. As an advantage, this allows full control of database schema and permits using very specific features of the database. As a disadvantage, it doesn't allow you to easily switch database providers, and you are required to create db upgrade scripts manually using SQL, and make sure they are executed somehow. All in all, I feel this more manual approach might be too manual, and maybe I should really accept "new .Net way" of doing database migrations.

## Build and run the project

### Running in Docker

```
# update images
docker pull artemkv/ref-watches-net:latest

# stats
docker run -d -p 8500:80 --env CONNECTIONSTRINGS__DEFAULTCONNECTION="Data Source=192.168.1.7;Initial Catalog=Watches;User Id=watches;Password=watch123;" artemkv/ref-watches-net:latest
```

## Configuration properties

ConnectionStrings:DefaultConnection - Database connection string

```
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=.;Initial Catalog=Watches;Integrated Security=True"
  }
}
```

ApiLimits:PageSizeLimit - Limit on the page size when retrieving collections (100)

```
{  
  "ApiLimits": {
    "PageSizeLimit": 66
  }
}
```

ApiDefaults:PageSize - Default page size when retrieving collections (20)

```
{  
  "ApiDefaults": {
    "PageSize": 20
  }
}
```

## Environment variables:

Note: Values of environment variables override the configuration parameters.

```
WATCHES_APILIMITS__PAGESIZELIMIT - limit on the page size when retrieving collections (100)
WATCHES_APIDEFAULTS__PAGESIZE - default page size when retrieving collections (20)
CONNECTIONSTRINGS__DEFAULTCONNECTION - database connection string
```
