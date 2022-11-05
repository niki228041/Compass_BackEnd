using AutoMapper;
using Compass.Data.Data.AutoMapper;
using Compass.Data.Data.Context;
using Compass.Data.Data.Interfaces;
using Compass.Data.Data.Models;
using Compass.Data.Data.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Compass.Data.Data.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;


        public UserRepository(UserManager<AppUser> userManager,IMapper mapper, SignInManager<AppUser> singInManager,IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _singInManager = singInManager;
            _configuration = configuration;
        }

        public async Task<AppUser> LoginUserAsync(LoginUserVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            return user;
        }

        public async Task<IdentityResult> RegisterUserAsync(AppUser model, string password)
        {
            //model.UserName = "user";
            var result = await _userManager.CreateAsync(model, password);
            return result;
        }

        public async Task<IdentityResult> AddRoleToUserAsync(AppUser model)
        {
            var role = GetRoleByUser(model);
            var result = await _userManager.AddToRoleAsync(model, role);
            return result;
        }

        public async Task<bool> ValidatePasswordAsync(LoginUserVM model, string password)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _userManager.CheckPasswordAsync(user, password);
            return result;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser appUser)
        {
            var result = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            return result;
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            var result = await _userManager.FindByIdAsync(id);
            return result;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(AppUser model, string token)
        {
            var result = await _userManager.ConfirmEmailAsync(model, token);
            return result;
        }

        public async Task<AppUser> GetUserByEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser model)
        {
            var result = await _userManager.GeneratePasswordResetTokenAsync(model);
            return result;
        }

        public async Task<IdentityResult> ResetPasswordAsync(AppUser model, string token, string password)
        {
            var result = await _userManager.ResetPasswordAsync(model, token, password);
            return result;
        }

        public async Task<IdentityResult> DeleteUserByIdAsync(DeleteUserVM model)
        {
            var user = await GetUserByIdAsync(model.Id);
            var result = await _userManager.DeleteAsync(user);
            return result;
        }

        public List<AppUser> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            return users;
        }

        public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            using(var _context = new AppDbContext())
            {
                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<RefreshToken> CheckRefreshTokenAsync(string refreshToken)
        {
            using (var _context = new AppDbContext())
            {
                var result = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshToken);
                if (result != null)
                {
                    return result;
                }
            }
            return new RefreshToken();
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            using (var _context = new AppDbContext())
            {
                _context.RefreshTokens.Update(refreshToken);
                await _context.SaveChangesAsync();
            }
        }

        public IList<string> GetRoles(AppUser model)
        {
            var roles = new List<string>();
            using (var _context = new AppDbContext())
            {
                var result = _context.Roles;
                foreach(var role in result)
                {
                    roles.Add(role.Name);
                }
            }
            return roles;
        }

        public string GetRoleByUser(AppUser model)
        {
            string roles = "";
            using (var _context = new AppDbContext())
            {
                var result = _context.Roles;
                foreach (var role in result)
                {
                    if(role.Name == model.Role)
                    {
                        roles = role.Name;
                    }
                }
            }
            return roles;
        }

        public async Task<IdentityResult> ChangeUser(AppUser model)
        {
            var old_user = await _userManager.FindByIdAsync(model.Id);

            old_user.Name = model.Name;
            old_user.UserName = model.UserName;
            old_user.Surname = model.Surname;
            old_user.Email = model.Email;

            var result = await _userManager.UpdateAsync(old_user);

            return result;
        }

        
    }
}
