using Application.Dtos;
using Application.Dtos.User;
using Application.Interfaces;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthService(ITokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        /// <summary>
        /// hàm dùng để đăng nhập/ đăng ký bằng google:
        /// nếu user chưa có trong hệ thống thì tạo mới user và trả về token (sign up).
        /// nếu user đã có trong hệ thống thì trả về token luôn (login)
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<object> LoginAsync(GoogleJsonWebSignature.Payload payload, GoogleLoginRequestDto loginRequest)
        {
            //if create user first, then create user, then generate jwt token and return to the client
            if (loginRequest.IsCreateUser)
            {                 
                var user = new UserDto()
                {
                    Email = payload.Email,
                    IsMc = loginRequest.IsMc ?? false,
                    IsNewbie = loginRequest.IsMc == true ? loginRequest.IsNewbie : null,
                    AvatarUrl = payload.Picture,
                    NickName = payload.Name,
                    FullName = payload.Name,
                    Age = 20,
                    IsVerified=false,
                };

                var userId = await _userService.AddAsync(user);
                if (userId == 0)
                {
                    throw new Exception("Couldn't create new user");
                }

                var token = _tokenService.GenerateToken(user);

                return new GoogleLoginResponseDto()
                {
                    AccessToken = token,
                    IsNewUser = false
                };
            }

            // if not create user, then check if user exists, if not exists, return key to indicate client need to set IsMc field and retry
            else
            {
                var existingUser = await _userService.FindByFieldAsync("Email", payload.Email);
                // if user not exists, return key to indicate client need to set IsMc field and retry
                //else, generate jwt token and return to the client
                if (existingUser == null)
                {
                    return new GoogleLoginResponseDto()
                    {
                        AccessToken = "",
                        IsNewUser = true
                    };
                }
                else
                {
                    var token = _tokenService.GenerateToken(existingUser);

                    return new GoogleLoginResponseDto()
                    {
                        AccessToken = token,
                        IsNewUser = false
                    };
                }

            }
        }
    }
}
