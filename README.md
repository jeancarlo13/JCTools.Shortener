# JCTools. Shortener
A simple **links shortener** for include in .net core projects.

Allows add support for create and manage **random** and **unique** short links. Its short links are ready for share link to the features or pages of your site.

The short links are created with **the letters of the alphabet of the english language and the numerals 0-9**, a total use 62 distinct characters.

This characters can will generate more of 57 billions of distinct combinations.

By default, the short links have a longitude between 2 and 6 characters; but, too can configure this longitude.

The longitude and the characters can was changed in the service initialization into the startup class.

## Usage

1. Add the package **JCTools.Shortener** to your project.

``` bash
    dotnet add package JCTools.Shortener -v 1.0.5
```

2. Implement the interface IDatabaseContext in your database contest

``` csharp
    public class DataContext : Microsoft.EntityFrameworkCore.DbContext, JCTools.Shortener.Settings.IDatabaseContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        { }
        /// <summary>
        /// the short links collection
        /// </summary>
        public DbSet<ShortLink> ShortLinks { get; set; }

    }
```

3. Add the namespace **JCTools.Shortener** to the startup class

``` csharp
    using JCTools.Shortener;
```

4. Configure the shortener service into the ConfigureServices method of the Startup class  

``` csharp
    services.AddLinksShortener<DbContext>();
```

5. Inject the *JCTools.Shortener.Services.ILinkGenerator* service

``` csharp
    public class GanttController : Controller
    {
        /// <summary>
        /// the shortener service to be used for share the gantt charts
        /// </summary>
        private readonly ILinkGenerator _shortenerService;

        public GanttController(ILinkGenerator shortenerService)
        {
            ...
    
            this._shortenerService = shortenerService;
    
            ...
        }

        ...

    }
```

6. Generate the short link

``` csharp

    ...

    // only generate a token
    var token = await _shortenerService.GenerateTokenAsync();

    // Generate a new entry with a short link
    var realUrl = "https://www.google.com/doodles/"
    var link = await _shortenerServices.GenerateAsync(realUrl);

    // Generate a new entry with a short link and store into the database
    var other = await _shortenerServices. GenerateAndSaveAsync(realUrl); 

    ...

```    

    

## Other settings

In the configuration of the Shortener services into the ConfigureServices method of the Startup class, is possible change the default configuration of the short links.

``` csharp

...
    ...

    services.AddLinksShortener<DbContext>(o => {
        // 1. Change the collection of the characters to be used for generate the short links
        o.ValidCharacters = "1234567890-_qwerty";
        // 2. The min longitude (default 2) of the short links
        o.MinLength = 3; 
        // 2. The max longitude (default 6) of the short links
        o.MaxLength = 10;
        // 3. By default, the access to the redirection action is anonymous
        // It's possible change using a authorization policy  
        o.ConfigurePolicy = p => {
            p.RequireAuthenticatedUser();
            p.RequireRole("admin");
            
            ...
        
        };
    });

    ...
```

## License

This package is released under the [MIT](https://github.com/jeancarlo13/JCTools.Shortener/blob/master/LICENSE) license

