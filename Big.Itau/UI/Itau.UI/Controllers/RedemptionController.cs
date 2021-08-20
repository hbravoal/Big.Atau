using Mastercard.Common.DTO.Response;
using Mastercard.Common.DTO.Response.MarketPlace;
using Mastercard.Common.Enums;
using Mastercard.Common.Utils;
using Mastercard.Common.Wrapper;
using Mastercard.UI.Business.Helpers;
using Mastercard.UI.Business.Interface;
using Mastercard.UI.Business.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using static Mastercard.Common.Utils.UtilitiesCommon;

namespace Mastercard.UI.Interface.Controllers
{
    [Authorize]
    public class RedemptionController : Controller
    {
        private readonly IRedemptionUIBl _redemptionBl;
        private readonly ICatalogBL _customerCatalog;
        private readonly IMasterQuantumBl _masterQuantumBl;
        private readonly IMasterBL _masterBl;
        private static object _balanceLock = new object();

        public RedemptionController(IRedemptionUIBl redemptionBl, ICatalogBL customerCatalog, IMasterQuantumBl masterQuantumBl, IMasterBL masterBl)
        {
            _redemptionBl = redemptionBl;
            _customerCatalog = customerCatalog;
            _masterQuantumBl = masterQuantumBl;
            _masterBl = masterBl;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="a">Será el Guid del Producto encriptado.</param>
        /// <param name="h">Será el tipo del Producto. Enumerable</param>
        /// <returns></returns>
        public async Task<ActionResult> Index(string a, string h)
        {
            var response = new RedemptionViewModel().Inicializate();
            if (TempData["StartUpScript"] != null)
            {
                var solve = TempData["StartUpScript"].ToString();
                ViewBag.StartUpScript = solve;
            }

            if (!SessionHelper.LoginComplete)
            {
                return RedirectToAction("Index", "Catalog");
            }

            if (SessionHelper.Token != null)
            {
                try
                {
                    response.Identification = SessionHelper.Identification;
                    response.Name = SessionHelper.Name;
                    //TODO:Si el usuario ya había redimo un bono, el sistema lo precargará estos campos con  posibilidad de ser editados.
                    var type = (EnumCatalogNetCommerceType)Convert.ToInt32(Cryptrography.Decrypt(h));
                    response.CatalogType = type;

                    var productGuid = Cryptrography.Decrypt(a);
                    var result = await _customerCatalog.GetProduct(productGuid, type);
                    if (result == null)
                        return RedirectToAction("Index", $"Catalog");

                    //Generico para departamentos (Pasala o quantum)
                    var departments = new List<SelectListItem>
                            {new SelectListItem {Value = "-1", Text = "--Departamento--"}};

                    var product = result.Result;
                    response.Id = product.Id;
                    response.BrandId = product.BrandId;
                    switch (type)
                    {
                        case EnumCatalogNetCommerceType.Digital:
                            //Llenamos Departamentos
                            var departmentsPgResponse = _masterBl.GetDeparmentsByCountry(
                                Convert.ToInt32(UtilitiesCommon.GetKeyAppSettings("PasalaGanando.CountryId")));
                            if (departmentsPgResponse != null && departmentsPgResponse.Result != null)
                            {
                                departments.AddRange(departmentsPgResponse.Result.Select(dep => new SelectListItem
                                {
                                    Text = dep.Name.ToString(),
                                    Value = dep.Id.ToString()
                                }));
                            }

                            break;

                        case EnumCatalogNetCommerceType.PCO:
                            var types = new List<SelectListItem>();
                            types.Add(new SelectListItem { Value = "-1", Text = "Tipo de documento" });
                            types.Add(new SelectListItem { Value = "1", Text = "Cédula" });
                            ViewBag.IdentificationTypes = types;
                            response.Message = "Esta es la información de la cuenta a la que se cargaran los Puntos Colombia.";
                            break;

                        case EnumCatalogNetCommerceType.External:

                            response.Message = $"Estos son los datos que enviaremos a {product.Brand} para tu redención.";
                            //if (!product.Location && product.AditionalInformation)
                            //{
                            //    response.Message = $"Estos son los datos que enviaremos a [Nombre de la marca] para tu redención.";
                            //    response.AditionalInformation = true;
                            //    return View(response);
                            //}
                            //Si es quantum y no tiene Location, no necesita Ningún product.
                            if (!product.Location)
                            {
                                if (product.AditionalInformation)
                                {
                                    response.Message = $"Estos son los datos que enviaremos a [Nombre de la marca] para tu redención.";
                                    response.AditionalInformation = true;
                                    return View(response);
                                }

                                response.Message = "¿Este seguro que desea redimir este producto ?";
                                return View(response);
                            }
                            else
                            {
                                response.Location = true;
                                ViewBag.Sites = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "--Establecimiento--" } };

                            }

                            

                            var departmentsResponse = await _masterQuantumBl.GetDepartment(
                                new Common.DTO.Request.MarketPlace.GetMasterRequest { brand_id = product.BrandId });

                            departments.AddRange(departmentsResponse.Result.Select(dep => new SelectListItem
                            {
                                Text = dep.dep_name.ToString(),
                                Value = dep.dep_id.ToString()
                            }));

                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    ViewBag.Departments = departments;
                    ViewBag.Cities = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "--Ciudad--" } };

                    return View(response);
                }
                catch (System.Exception ex)
                {
                    TempData["StartUpScript"] = $"showError('Error','Lo sentimos, ocurrio un error intentnado redimir el producto que escogiste')";
                    return RedirectToAction("Index", $"Catalog");
                }
            }
            else
                return RedirectToAction("LogOut", $"Account");
        }

        [HttpPost]
        public ActionResult Index(RedemptionViewModel model)
        {
            if (!SessionHelper.LoginComplete)
            {
                return RedirectToAction("Index", "Catalog");
            }
            lock (_balanceLock)
            {
                if (SessionHelper.Token != null)
                {
                    //Generico para departamentos (Pasala o quantum)
                    var departments = new List<SelectListItem>
                        {new SelectListItem {Value = "-1", Text = "--Departamento--"}};
                    var cities = new List<SelectListItem> { new SelectListItem { Value = "-1", Text = "--Ciudad--" } };
                    model.UserToken = SessionHelper.Token;
                    try
                    {
                        switch (model.CatalogType)
                        {
                            case EnumCatalogNetCommerceType.Digital:
                                ValidationForDigital(model);
                                if (!ModelState.IsValid)
                                {
                                    var departmentsPgResponse = _masterBl.GetDeparmentsByCountry(
                                        Convert.ToInt32(UtilitiesCommon.GetKeyAppSettings("PasalaGanando.CountryId")));
                                    if (departmentsPgResponse?.Result != null)
                                    {
                                        departments.AddRange(departmentsPgResponse.Result.Select(dep =>
                                            new SelectListItem
                                            {
                                                Text = dep.Name.ToString(),
                                                Value = dep.Id.ToString()
                                            }));
                                    }

                                    //Obtienemos ciudad.
                                    if (model.DepartmentId > 0)
                                    {
                                        var citiesPgResponse = _masterBl.GetCitiesByDeparmentId(model.DepartmentId);
                                        if (citiesPgResponse?.Result != null)
                                        {
                                            cities.AddRange(citiesPgResponse.Result.Select(dep => new SelectListItem
                                            {
                                                Text = dep.Name.ToString(),
                                                Value = dep.Id.ToString()
                                            }));
                                        }
                                    }

                                    ViewBag.Departments = departments;
                                    ViewBag.Cities = cities;
                                    return View(model);
                                }

                                break;

                            case EnumCatalogNetCommerceType.PCO:
                                ValidationForPco(model);
                                var types = new List<SelectListItem>();
                                types.Add(new SelectListItem { Value = "-1", Text = "Tipo de documento" });
                                types.Add(new SelectListItem { Value = "1", Text = "Cédula" });
                                ViewBag.IdentificationTypes = types;
                                if (!ModelState.IsValid)
                                {
                                    return View(model);
                                }
                                

                                break;

                            case EnumCatalogNetCommerceType.External:
                                
                                    ValidationForExternal(model);
                                

                                if (!ModelState.IsValid)
                                {
                                    if (!model.Location && model.AditionalInformation)
                                    {
                                        return View(model);
                                    }
                                    var citiesResponse = new Response<List<CityQuantumResponse>>();
                                    if (model.Location && model.SiteId <= 0)
                                    {
                                        Response<List<DepartmentQuantumResponse>> departmentsResponse;
                                        using (var task = Task.Run(async () => await _masterQuantumBl.GetDepartment(new Common.DTO.Request.MarketPlace.GetMasterRequest { brand_id = model.BrandId })))
                                        {
                                            departmentsResponse = task.Result;
                                        }

                                        departments.AddRange(departmentsResponse.Result.Select(dep => new SelectListItem
                                        {
                                            Text = dep.dep_name.ToString(),
                                            Value = dep.dep_id.ToString()
                                        }));

                                        if (model.DepartmentId > 0)
                                        {
                                            using (var task = Task.Run(async () => await _masterQuantumBl.GetCity(new Common.DTO.Request.MarketPlace.GetMasterRequest { brand_id = model.BrandId, dep_id = Convert.ToString(model.DepartmentId) })))
                                            {
                                                citiesResponse = task.Result;
                                            }

                                            cities.AddRange(citiesResponse.Result.Select(dep => new SelectListItem
                                            {
                                                Text = dep.city_name.ToString(),
                                                Value = dep.city_id.ToString()
                                            }));
                                        }

                                        ViewBag.Departments = departments;
                                        ViewBag.Cities = cities;
                                        ViewBag.Sites = new List<SelectListItem>
                                        {new SelectListItem {Value = "-1", Text = "--Establecimiento--"}};
                                        return View(model);
                                    }
                                }

                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        Response<RedemptionResponse> redemptionResult;
                        model.UserGuid = SessionHelper.Guid;
                        model.SegmentId = SessionHelper.SegmentId;
                        model.UserToken = SessionHelper.Token;
                        model.MarketPlaceToken = SessionHelper.MarketPlaceToken;
                        using (var task = Task.Run(async () => await _redemptionBl.Redemption(model)))
                        {
                            redemptionResult = task.Result;
                        }

                        if (redemptionResult.IsSuccess)
                        {
                            ViewBag.Result = redemptionResult.Result;
                            //function ShowRedemption(type, message, title, linkPdf, code, expiration)
                            ViewBag.StartUpScript =
                                $"jQuery(ShowRedemption('{(int)model.CatalogType}','{redemptionResult.Result.Message}','{redemptionResult.Result.Title}','{redemptionResult.Result.LinkPdf}','{redemptionResult.Result.Code}','{redemptionResult.Result.ExpirationDate}'))";
                        }
                        else
                        {
                            ViewBag.StartUpScript =
                                $"jQuery(ShowRedemptionError('¡Opciones para redimir!','En el momento este producto no está disponible, selecciona otro o inténtalo más tarde. Recuerda que también puedes redimir y acumular cupones para participar por el premio especial que quieras.'))";

                            ViewBag.Departments = departments;
                            ViewBag.Cities = cities;
                            ViewBag.Sites = new List<SelectListItem>
                                {new SelectListItem {Value = "-1", Text = "--Establecimiento--"}};
                            return View(model);
                            //Mostrar errores:
                        }

                        ViewBag.StartUpScript =
                            $"jQuery(ShowRedemption('{(int)model.CatalogType}','{redemptionResult.Result.Message}','{redemptionResult.Result.Title}','{redemptionResult.Result.LinkPdf}','{redemptionResult.Result.Code}','{redemptionResult.Result.ExpirationDate}'))";
                        ViewBag.Departments = departments;
                        ViewBag.Cities = cities;
                        ViewBag.Sites = new List<SelectListItem>
                            {new SelectListItem {Value = "-1", Text = "--Establecimiento--"}};
                        return View(model);
                    }
                    catch (System.Exception ex)
                    {
                        TempData["StartUpScript"] = $"showError('Error','Lo sentimos, ocurrio un error intentnado redimir el producto que escogiste')";
                        return RedirectToAction(nameof(Index), "Catalog");
                    }
                }
            }

            return View();
        }

        #region Validations

        public void ValidationForExternal(RedemptionViewModel model)
        {
            if (model.Location)
            {
                if (model.DepartmentId <= 0)
                {
                    ModelState.AddModelError("DepartmentId", "Es necesario que ingreses el departamento.");
                }
                if (model.CityId <= 0)
                {
                    ModelState.AddModelError("CityId", "Es necesario que ingreses la ciudad.");
                }
                if (model.SiteId <= 0)
                {
                    ModelState.AddModelError("SiteId", "Es necesario que ingreses el establecimiento.");
                }
            }

            if (!model.AditionalInformation) return;

            if (string.IsNullOrEmpty(model.Identification))
            {
                ModelState.AddModelError("Identification", "Esta campaña está dirigida únicamente a clientes Bancolombia con Tarjetas Maestro o Mastercard, débito o crédito, que hayan recibido la comunicación de bienvenida. Si tienes inconvenientes con el ingreso comunícate con la línea de atención 01-800-0912345.");
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", "Nombre es requerido.");
            }
            if (string.IsNullOrEmpty(model.Phone) || model.Phone.Length != 10
                                                  || Regex.Match(model.Phone, @"^(\+[0-9]{9})$").Success)
            {
                ModelState.AddModelError("Phone", "Tu número de celular es necesario. Revisa que los campos estén correctos.");
            }
        }

        public void ValidationForPco(RedemptionViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                ModelState.AddModelError("Name", "Nombre es requerido.");
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                ModelState.AddModelError("LastName", "Apellido  es requerido.");
            }
            if (model.IdentificationType <= 0)
            {
                ModelState.AddModelError("IdentificationType", "Tipo de identificación es requerido.");
            }
            if (string.IsNullOrEmpty(model.Identification))
            {
                ModelState.AddModelError("Identification", "Esta campaña está dirigida únicamente a clientes Bancolombia con Tarjetas Maestro o Mastercard, débito o crédito, que hayan recibido la comunicación de bienvenida. Si tienes inconvenientes con el ingreso comunícate con la línea de atención 01-800-0912345.");
            }
        }

        public void ValidationForDigital(RedemptionViewModel model)
        {
            //if (string.IsNullOrEmpty(model.Phone) || string.IsNullOrEmpty(model.Email)
            //                                      || (model.CityId) <= 0 ||
            //                                      string.IsNullOrEmpty(model.Address))
            //var ps = !Regex.Match(model.Phone, @"^(\+[0-9]{9})$").Success;
            if (string.IsNullOrEmpty(model.Phone) || model.Phone.Length != 10
                                                  || Regex.Match(model.Phone, @"^(\+[0-9]{9})$").Success)
            {
                ModelState.AddModelError("Phone", "Tu número de celular es necesario. Revisa que los campos estén correctos.");
            }
            if (string.IsNullOrEmpty(model.Email) || !IsValidEmail(model.Email))
            {
                ModelState.AddModelError("Email", "Los datos no son válidos. Ingresa correctamente tu correo.");
            }
            if (model.DepartmentId <= 0)
            {
                ModelState.AddModelError("DepartmentId", "Es necesario que ingreses el departamento.");
            }
            if (model.CityId <= 0)
            {
                ModelState.AddModelError("CityId", "Es necesario que ingreses la ciudad.");
            }
            if (string.IsNullOrEmpty(model.Address))
            {
                ModelState.AddModelError("Address", "Es necesario que ingreses correctamente tu dirección.");
            }
        }

        public bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        #endregion Validations

        [Authorize]
        public async Task<ActionResult> MyRedemption()
        {
            try
            {
                if (string.IsNullOrEmpty(SessionHelper.Token))
                {
                    return RedirectToAction("LogOut", "Account");
                }
                if (!SessionHelper.LoginComplete)
                {
                    return RedirectToAction("Index", $"Profile");
                }
                var myRedemption = await _redemptionBl.GetRedemption();
                if (myRedemption.IsSuccess)
                    return View(myRedemption.Result);

                if (myRedemption.Message == "TokenExpired")
                {
                    return RedirectToAction("LogOut", "Account");
                }
            }
            catch (Exception ex)
            {
                Common.Diagnostics.ExceptionLogging.LogException(ex);
            }
            return Redirect("~/error");
        }
    }
}