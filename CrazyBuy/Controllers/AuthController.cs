using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CrazyBuy.DAO;
using CrazyBuy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CrazyBuy.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration configuration)
        {
            _config = configuration;
        }

        // GET api/auth/login
        [HttpPost]
        public ActionResult Login([FromBody]Dictionary<string, string> data)
        {
            ReturnMessage result = new ReturnMessage();
            try
            {
                Dictionary<string, string> rm = new Dictionary<string, string>();
                // STEP0: 在產生 JWT Token 之前，可以依需求做身分驗證
                if (data.ContainsKey("tenantId") && data.ContainsKey("account") && data.ContainsKey("pwd"))
                {
                    string userId = data.GetValueOrDefault("account");
                    string pwd = data.GetValueOrDefault("pwd");
                    string defaultTenant = data.GetValueOrDefault("tenantId");
                    string userName;
                    string userUuid;
                    string tenantId;
                    string tenantType;
                    string type;
                    string userNameId;
                    string userType = UserType.GUEST;

                    Member member = DataManager.memberDao.getMember(userId, pwd);
                    if (member != null)
                    {
                        // login
                        userName = member.name;
                        userUuid = member.memberCode.ToString();
                        userNameId = member.memberId.ToString();
                        tenantType = member.tenantType;
                        type = LoginType.LOGIN_USER;
                        userType = CTenantManager.isOwner(userNameId) ? UserType.ADMIN : UserType.MEMBER;
                        tenantId = DataManager.tenantMemberDao.getTenantMemberByMemberId(member.memberId).tenantId.ToString();

                        if (tenantId != null)
                        {
                            // updateLoginTime
                            member.dLastLogin = DateTime.Now;
                            DataManager.memberDao.updateMember(member);
                        }
                        else
                        {
                            tenantId = defaultTenant;
                        }
                    }
                    else
                    {
                        // not login for guest
                        string id = Guid.NewGuid().ToString();
                        userName = LoginType.GUEST;
                        userUuid = id;
                        userNameId = id;
                        tenantType = "";
                        type = LoginType.GUEST;
                        tenantId = defaultTenant;
                    }

                    // STEP1: 建立使用者的 Claims 聲明，這會是 JWT Payload 的一部分
                    var userClaims = new ClaimsIdentity(new[] {
                    new Claim(JwtRegisteredClaimNames.NameId, userNameId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, userUuid),
                    new Claim("MemberName", userName),
                    new Claim("MemberTenantType", tenantType),
                    new Claim("type", type),
                    new Claim("userType", userType),
                    new Claim("tenantId", tenantId)
                    });

                    // STEP2: 取得對稱式加密 JWT Signature 的金鑰
                    // 這部分是選用，但此範例在 Startup.cs 中有設定 ValidateIssuerSigningKey = true 所以這裡必填
                    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    // STEP3: 建立 JWT TokenHandler 以及用於描述 JWT 的 TokenDescriptor
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Issuer = _config["Jwt:Issuer"],
                        Audience = _config["Jwt:Issuer"],
                        Subject = userClaims,
                        Expires = DateTime.Now.AddMinutes(30),
                        SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                    };
                    // 產出所需要的 JWT Token 物件
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    // 產出序列化的 JWT Token 字串
                    var serializeToken = tokenHandler.WriteToken(securityToken);
                    rm.Add("code", MessageCode.SUCCESS.ToString());
                    rm.Add("token", serializeToken);
                    rm.Add("type", type);
                    rm.Add("userType", userType);
                    rm.Add("tenantId", tenantId);
                    return Ok(rm);
                }
                else
                {
                    rm.Add("code", MessageCode.ERROR.ToString());
                    rm.Add("message", "data input error.");
                    return BadRequest(rm);
                }
            }
            catch (Exception e)
            {
                result.code = MessageCode.ERROR;
                result.data = e.Message;
            }
            return Ok(result);
        }
    }
}
