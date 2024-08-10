using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Store.API.Dtos;
using Store.API.Extensions;
using Store.Core.Entities.Identity;
using Store.Core.Services.Contract;
using AutoMapper;
using Store.API.Errors;


namespace Store.API.Controllers
{
    public class AccountController : BaseApiController
    {
		private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAuthService _authService;
		private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,
            IAuthService authService,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                var user=await _userManager.FindByEmailAsync(model.Email);
                if(user is null)
                    return Unauthorized(new ApiResponse(401));

                var result= await _signInManager.CheckPasswordSignInAsync(user, model.Password,false);

                if(!result.Succeeded)
                    return Unauthorized(new ApiResponse(401));

                return Ok(new UserDto
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await _authService.CreateTokenAsync(user, _userManager)
                }); ;
            }

            return Unauthorized(new ApiResponse(401));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if (CheckEmailExists(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors= new string[] {"This email already exists!"} });

            var user = new AppUser
            {   
                Email = model.Email,
                DisplayName = model.DisplayName,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
            };

            var result=await _userManager.CreateAsync(user,model.Password);

            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400));

            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });

        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>>GetCurrentUser()
        {
            var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);  

            var user= await _userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                DisplayName= user.DisplayName,
                Email = user.Email,
                Token= await _authService.CreateTokenAsync(user, _userManager)
            });
        }


        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {

            var user = await _userManager.FindUserWithAddressAsync(User);     // used to get current user from claimprinciple

            var address=  _mapper.Map<AddressDto>(user.Address);

            return Ok(address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            var address = _mapper.Map<Address>(updatedAddress);
            var user = await _userManager.FindUserWithAddressAsync(User);
            address.Id = user.Address.Id;
            user.Address = address;
            var result=await _userManager.UpdateAsync(user);
            if(!result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(updatedAddress);
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            var found = await _userManager.FindByEmailAsync(email) is not null ;
            return found;
        }

    }
}
