using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using API.Data.Models;
using API.Features.Identity.Models;
using API.Infrastructure.Exceptions;
using API.Infrastructure.Exceptions.Identity;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Features.Identity.Commands
{
    public class LoginCommand : IRequest<LoginResponseModel>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseModel>
    {
        private readonly UserManager<User> userManager;
        private readonly AppSettings appSettings;

        public LoginCommandHandler(UserManager<User> userManager, IOptions<AppSettings> appSettings)
        {
            this.userManager = userManager;
            this.appSettings = appSettings.Value;
        }

        public async Task<LoginResponseModel> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await this.userManager.FindByNameAsync(request.UserName);

            await ValidateUserInfo(request, user);

            var encryptedToken = GenerateJwtToken(user.Id, user.UserName, this.appSettings.Secret);

            var response = new LoginResponseModel
            {
                Token = encryptedToken
            };

            return response;
        }

        private async Task ValidateUserInfo(LoginCommand request, User user)
        {
            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.UserName);
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordValid)
            {
                throw new IncorrectPasswordException($"The provided password was incorrect.");
            }
        }

        private string GenerateJwtToken(string userId, string userName, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(l => l.UserName)
                .NotEmpty()
                .NotNull();

            RuleFor(l => l.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}
