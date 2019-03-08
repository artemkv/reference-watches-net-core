## Build and run the project

### Running in Docker

## Configuration properties

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
```