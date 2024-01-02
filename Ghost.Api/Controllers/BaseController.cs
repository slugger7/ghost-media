using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Ghost.Api;

public abstract class BaseController : Controller
{
  protected int UserId => int.Parse(User.FindFirst(ClaimTypes.PrimarySid)?.Value ?? "0");
}