using Application.Dtos;
using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<object> LoginAsync(GoogleJsonWebSignature.Payload payload, GoogleLoginRequestDto loginRequest);
    }
}
