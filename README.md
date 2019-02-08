# ServiceProxy
Leverages Refit to create strongly-typed REST service proxies that include authorization and message correlation.


## Motivation

I was motivated to write this component to simplify the configuration of [Refit](https://github.com/reactiveui/refit) service proxies.


## Features

* **Targets .NET Standard 2.0** so it may be used in .NET Core or .NET Framework runtimes.
* **Automatically passes authentication token and correlation ID** to service so client is authenticated and code execution can be traced accross process boundaries, related by correlation ID. 


## Related Solution

If you're developing an ASP.NET Core MVC website or WebAPI service, I highly recommend installing my [AspNetCore.Middleware](https://github.com/ekmadsen/AspNetCore.Middleware) package, which uses this component and others to eliminate a lot of boilerplate code.


## Installation

Reference this component in your application via its [NuGet package](https://www.nuget.org/packages/ErikTheCoder.ServiceProxy/).


## Usage

Create and inject proxy dependency in Startup.ConfigureServices:

```C#
// Create service proxies.
Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
IHttpContextAccessor httpContextAccessor = Services.BuildServiceProvider().GetRequiredService<IHttpContextAccessor>();
string accountServiceUrl = Program.AppSettings.ServiceProxies[Keys.IdentityServiceName].Url;
string accountServiceToken = Program.AppSettings.ServiceProxies[Keys.IdentityServiceName].Token;
IAccountService accountService = Proxy.For<IAccountService>(accountServiceUrl, accountServiceToken, () => httpContextAccessor.HttpContext.GetCorrelationId());
// Configure dependency injection.
Services.AddSingleton(typeof(IAppSettings), Program.AppSettings);
Services.AddSingleton(typeof(ILogger), logger);
Services.AddSingleton(typeof(IAccountService), accountService);
```

Inject and use proxy in a controller class:

```C#
namespace ErikTheCoder.MadPoker.Website.Controllers
{
    [Authorize(Policy = Policy.Admin)]
    public class AccountController : ControllerBase
    {
        private const string _invalidCredentialsMessage = "Invalid username or password.";
        private readonly IAccountService _accountService;


        public AccountController(IAppSettings AppSettings, ILogger Logger, IAccountService AccountService) :
            base(AppSettings, Logger)
        {
            _accountService = AccountService;
        }
```

```C#
[AllowAnonymous]
[HttpPost]
public async Task<IActionResult> Login(LoginModel Model)
{
    LoginRequest request = new LoginRequest
    {
        Username = Model.Username,
        Password = Model.Password
    };
    User user = await _accountService.LoginAsync(request);
```


## Benefits

You can create, configure, and dependency-inject thread-safe proxies in a single location- the .NET Core application's Startup class.  Then use the proxies in the application's MVC or WebAPI controllers by simply calling their methods.  You do not need to perform any further configuration, cleanup, or [code ceremony](http://thinkrelevance.com/blog/2008/04/23/refactoring-from-ceremony-to-essence) in the controller code.
