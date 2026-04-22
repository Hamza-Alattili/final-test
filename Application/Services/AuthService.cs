using Application.DTOs.Auth;
using Application.Interfaces;
using Application.Repositories.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;
        private readonly IRepository<RefreshToken> _refreshTokenRepo;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<User> _passwordHasher;

        public AuthService(
            IRepository<User> userRepo,
            IRepository<Role> roleRepo,
            IRepository<RefreshToken> refreshTokenRepo,
            IConfiguration configuration)
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _refreshTokenRepo = refreshTokenRepo;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<User>();
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var users = await _userRepo.FindAsync(u => u.Email == dto.Email);
            var user = users.FirstOrDefault();

            if (user == null)
                throw new UnauthorizedAccessException("البريد الإلكتروني أو كلمة المرور غير صحيحة.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("البريد الإلكتروني أو كلمة المرور غير صحيحة.");

            var roles = await _roleRepo.FindAsync(r => r.Id == user.RoleId);
            user.Role = roles.FirstOrDefault()!;

            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existing = await _userRepo.FindAsync(u => u.Email == dto.Email);
            if (existing.Any())
                throw new InvalidOperationException("البريد الإلكتروني مستخدم بالفعل.");

            var roles = await _roleRepo.FindAsync(r => r.Code == RoleEnum.Patient);
            var patientRole = roles.FirstOrDefault()
                ?? throw new InvalidOperationException("دور المريض غير موجود في النظام.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                RoleId = patientRole.Id,
                Role = patientRole
            };
            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            await _userRepo.AddAsync(user);
            return await GenerateTokensAsync(user);
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var tokens = await _refreshTokenRepo.FindAsync(rt => rt.Token == refreshToken && rt.Expires > DateTime.UtcNow);
            var storedToken = tokens.FirstOrDefault()
                ?? throw new UnauthorizedAccessException("Refresh token غير صالح أو منتهي الصلاحية.");

            var users = await _userRepo.FindAsync(u => u.Id == storedToken.UserId);
            var user = users.FirstOrDefault()
                ?? throw new UnauthorizedAccessException("المستخدم غير موجود.");

            var roleList = await _roleRepo.FindAsync(r => r.Id == user.RoleId);
            user.Role = roleList.FirstOrDefault()!;

            await _refreshTokenRepo.DeleteAsync(storedToken);

            return await GenerateTokensAsync(user);
        }

        public async Task RevokeTokenAsync(string refreshToken)
        {
            var tokens = await _refreshTokenRepo.FindAsync(rt => rt.Token == refreshToken);
            var storedToken = tokens.FirstOrDefault();
            if (storedToken != null)
                await _refreshTokenRepo.DeleteAsync(storedToken);
        }

        private async Task<AuthResponseDto> GenerateTokensAsync(User user)
        {
            var jwtSection = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresMinutes = int.TryParse(jwtSection["AccessTokenMinutes"], out var m) ? m : 15;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            var refreshTokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            await _refreshTokenRepo.AddAsync(refreshTokenEntity);

            return new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                ExpiresInMinutes = expiresMinutes,
                User = new UserInfoDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role?.Name ?? string.Empty
                }
            };
        }
    }
}
