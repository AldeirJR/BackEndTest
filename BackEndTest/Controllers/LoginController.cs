using BackEndTest.Domain.Entity;
using BackEndTest.Domain.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BackEndTest.Controllers
{
    public class LoginController : Controller
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginController> _logger;
        //public readonly ResultadoProcess processResultado;
        private readonly IEmailSender _emailSender;

        public LoginController(SignInManager<ApplicationUser> signInManager,
           ILogger<LoginController> logger,
           UserManager<ApplicationUser> userManager
           /*ResultadoProcess pResultadoProcess*/, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            //processResultado = pResultadoProcess;
            _emailSender = emailSender;
        }


        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {



            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Lembrar a senha ?")]
            public bool RememberMe { get; set; }

            public bool IsActive { get; set; }
        }



        public IActionResult Login()
        {
            return View();


        }

       


        [HttpPost]
        public async Task<IActionResult> Login(InputModel input)
        {
            Resultado resultado = new Resultado();

             if (ModelState.IsValid)
            {
                   
                    var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password,false,false);

            
                    if (result.Succeeded)
                {

                    _logger.LogInformation("User logged in.");
                    return RedirectToAction("Create", "Clientes");

                }
                ModelState.AddModelError(string.Empty, "Login inválido");
            }
            return View();
            }
           


        


        public async Task<IActionResult> EsqueceuaSenha(string email)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code ,email},
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    email,
                    "Reset de senha EasySup",
                   
                    $" <div align='center'> <img src='https://i.ibb.co/K9fhnfk/Email-easysupcopia.png' /> </div>  <div align='center'> <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'  > <button style='width:150px;height:50px; background: #00D0FF 0% 0% no-repeat padding-box'>Sim Fui Eu</button> </a>. </div>"



                    );

                return RedirectToPage("./ForgotPasswordConfirmation");
            }
            return View();

        }

    }
}
