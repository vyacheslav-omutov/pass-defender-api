using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PassDefender.Domain;
using PassDefender.Host.Enums;
using PassDefender.Host.Extensions;
using PassDefender.Host.Requests.Login;
using PassDefender.Host.Services.Email;
using PassDefender.Host.Validators;

namespace PassDefender.Host.Controllers;

[ApiController, Route("api/authorization")]
public class AuthorizationController(
    TimeProvider timeProvider,
    IOptionsMonitor<BearerTokenOptions> bearerTokenOptions,
    UserManager<ApplicationUser> userManager,
    IUserStore<ApplicationUser> userStore,
    SignInManager<ApplicationUser> signInManager,
    IEmailService emailService) : ControllerBase
{
    private readonly EmailAddressAttribute _emailAddressAttribute = new();

    [HttpPost("login/password")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginPasswordRequest request)
    {
        var validator = new LoginPasswordValidator();
        var isValid = await validator.ValidateAsync(request);
        if (!isValid.IsValid && isValid.Errors.Any())
        {
            return Unauthorized(isValid.CreateValidationProblem());
        }

        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Unauthorized();
        }

        var isPassword = await userManager.CheckPasswordAsync(user, request.Password);
        if (!isPassword)
        {
            return Unauthorized();
        }

        var principal = await signInManager.CreateUserPrincipalAsync(user);
        return SignIn(principal, authenticationScheme: IdentityConstants.BearerScheme);
    }

    [HttpPost("login/two-factor")]
    public async Task<IActionResult> LoginEmailAsync([FromBody] LoginTwoFactorRequest factorRequest)
    {
        var validator = new LoginEmailValidator();
        var isValid = await validator.ValidateAsync(factorRequest);
        if (!isValid.IsValid && isValid.Errors.Any())
        {
            return Unauthorized(isValid.CreateValidationProblem());
        }

        var user = await userManager.FindByEmailAsync(factorRequest.Email);

        if (user is null)
        {
            user = new ApplicationUser();
            await userStore.SetUserNameAsync(user, factorRequest.Email, CancellationToken.None);
            var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
            await emailStore.SetEmailAsync(user, factorRequest.Email, CancellationToken.None);

            await userManager.CreateAsync(user);
        }

        var code = await userManager.GenerateTwoFactorTokenAsync(user, nameof(TwoFactorType.Email));

        await emailService.SendEmailAsync(user.Email!, "Код подтверждения", code);
        return Ok();
    }

    [HttpPost("login/two-factor/confirm")]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginEmailConfirmAsync([FromBody] LoginTwoFactorConfirmRequest request)
    {
        var validator = new LoginTwoFactorConfirmValidator();
        var isValid = await validator.ValidateAsync(request);
        if (!isValid.IsValid && isValid.Errors.Any())
        {
            return Unauthorized(isValid.CreateValidationProblem());
        }
        
        if (await userManager.FindByEmailAsync(request.Email) is not { } user)
        {
            return Unauthorized();
        }

        var succeeded = await userManager.VerifyTwoFactorTokenAsync(user, nameof(TwoFactorType.Email), request.Code);
        if (!succeeded)
        {
            return Unauthorized();
        }

        var principal = await signInManager.CreateUserPrincipalAsync(user);
        return SignIn(principal, authenticationScheme: IdentityConstants.BearerScheme);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        if (!userManager.SupportsUserEmail)
        {
            throw new NotSupportedException(
                $"{nameof(AuthorizationController)} requires a user store with email support.");
        }

        var emailStore = (IUserEmailStore<ApplicationUser>)userStore;
        var email = request.Email;
        if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
        {
            var errors = IdentityResult
                .Failed(userManager.ErrorDescriber.InvalidEmail(email))
                .CreateValidationProblem();
            return BadRequest(errors);
        }

        var user = new ApplicationUser();
        await userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded) return Ok();
        {
            var errors = result.CreateValidationProblem();
            return BadRequest(errors);
        }
    }

    [HttpPost("refresh"), Authorize]
    [ProducesResponseType<AccessTokenResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshRequest refreshRequest)
    {
        var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
        var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);
        if (refreshTicket?.Properties.ExpiresUtc is not { } expiresUtc ||
            timeProvider.GetUtcNow() >= expiresUtc ||
            await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not { } user)
        {
            return Challenge();
        }

        var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);
        return SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
    }
}