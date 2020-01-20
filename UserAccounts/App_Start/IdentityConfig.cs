using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using UserAccounts;
using UserAccounts.Models;

namespace UserAccounts
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return SendGridAsync(message);
        }

        private async Task SendGridAsync(IdentityMessage message)
        {
            var myMessage = new SendGridMessage();
            myMessage.From = new EmailAddress(
                "Joe@contoso.com", "Joe S.");
            myMessage.Subject = message.Subject;
            myMessage.PlainTextContent = message.Body;
            myMessage.HtmlContent = message.Body;
            myMessage.AddTo(message.Destination);

            var apiKey = "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";

            var client = new SendGridClient(apiKey);
            var response = await client.SendEmailAsync(myMessage);
        }
    }
}

public class SmsService : IIdentityMessageService
{
    public Task SendAsync(IdentityMessage message)
    {
        // Plug in your SMS service here to send a text message.
        return Task.FromResult(0);
    }
}

// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
public class ApplicationUserManager : UserManager<ApplicationUser>
{
    public ApplicationUserManager(IUserStore<ApplicationUser> store)
        : base(store)
    {
    }

    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
        IOwinContext context)
    {
        var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
        // Configure validation logic for usernames
        manager.UserValidator = new UserValidator<ApplicationUser>(manager)
        {
            AllowOnlyAlphanumericUserNames = false,
            RequireUniqueEmail = true
        };

        // Configure validation logic for passwords
        manager.PasswordValidator = new PasswordValidator
        {
            RequiredLength = 6,
            RequireNonLetterOrDigit = false,
            RequireDigit = false,
            RequireLowercase = false,
            RequireUppercase = false
        };

        // Configure user lockout defaults
        manager.UserLockoutEnabledByDefault = true;
        manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
        manager.MaxFailedAccessAttemptsBeforeLockout = 5;

        // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
        // You can write your own provider and plug it in here.
        manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
        {
            MessageFormat = "Your security code is {0}"
        });
        manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
        {
            Subject = "Security Code",
            BodyFormat = "Your security code is {0}"
        });
        manager.EmailService = new EmailService();
        manager.SmsService = new SmsService();
        var dataProtectionProvider = options.DataProtectionProvider;
        if (dataProtectionProvider != null)
        {
            manager.UserTokenProvider =
                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        }

        return manager;
    }
}

// Configure the application sign-in manager which is used in this application.
public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
{
    public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
        : base(userManager, authenticationManager)
    {
    }

    public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
    {
        return user.GenerateUserIdentityAsync((ApplicationUserManager) UserManager);
    }

    public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options,
        IOwinContext context)
    {
        return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
    }
}