using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using MvcNetCoreSessionEmpleados.Extensions;
using MvcNetCoreSessionEmpleados.Models;
using MvcNetCoreSessionEmpleados.Repositories;

namespace MvcNetCoreSessionEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;
        private IMemoryCache memoryCache;
        public EmpleadosController(RepositoryEmpleados repo,IMemoryCache memoryCache)
        {
            this.repo = repo;
            this.memoryCache = memoryCache;
        }

        public IActionResult EmpleadosFavoritos(int? ideliminar)
        {
            if(ideliminar != null)
            {
                List<Empleado> favoritos = this.memoryCache.Get<List<Empleado>>("FAVORITOS");

                Empleado empDelete = 
                    favoritos.Find(i => i.IdEmpleado == ideliminar.Value);
                favoritos.Remove(empDelete);
                if(favoritos.Count == 0)
                {
                    this.memoryCache.Remove("FAVORITOS");
                }
                else
                {
                    this.memoryCache.Set("FAVORITOS",favoritos);
                }
            }
            return View();
        }

        public async Task<IActionResult> SessionEmpleadosV5(int? idEmpleado,int? idfavorito,int? ideliminar)
        {
            if(idfavorito != null)
            {
                List<Empleado> empleadosFavoritos;
                if(this.memoryCache.Get("FAVORITOS") == null)
                {
                    empleadosFavoritos = new List<Empleado>();

                }
                else
                {
                    empleadosFavoritos = this.memoryCache.Get<List<Empleado>>("FAVORITOS");

                }
                Empleado emp = await this.repo.FindEmpleadoAsync(idfavorito.Value);
                empleadosFavoritos.Add(emp);
                this.memoryCache.Set("FAVORITOS", empleadosFavoritos);
            }
            
            if (idEmpleado != null)
            {
                List<int> idsEmpleados;

                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOSV5") == null)
                {
                    idsEmpleados = new List<int>();
                }
                else
                {
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOSV5");

                }
                idsEmpleados.Add(idEmpleado.Value);
                HttpContext.Session.SetObject("IDSEMPLEADOSV5", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: " + idsEmpleados.Count;
            }
            if (ideliminar != null)
            {
                List<Empleado> favoritos = this.memoryCache.Get<List<Empleado>>("FAVORITOS");

                Empleado empDelete =
                    favoritos.Find(i => i.IdEmpleado == ideliminar.Value);
                favoritos.Remove(empDelete);
                if (favoritos.Count == 0)
                {
                    this.memoryCache.Remove("FAVORITOS");
                }
                else
                {
                    this.memoryCache.Set("FAVORITOS", favoritos);
                }
            }

            List<Empleado> empleados = await this.repo.GetEmpleadoAsync();
            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosAlmacenadosV5(int? idempleado)
        {
            
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOSV5");
            if (idempleado != null)
            {
                if (idsEmpleados != null && idsEmpleados.Contains((int)idempleado))
                {
     
                    idsEmpleados.Remove((int)idempleado);

                    HttpContext.Session.SetObject("IDSEMPLEADOSV5", idsEmpleados);
                }
            }
            if (idsEmpleados == null)
            {
                ViewData["mensaje"] = "no existen empleados almacenados en Session";
                return View();
            }
            else
            {
                List<Empleado> empleados = await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
            
        }

        public async Task<IActionResult> SessionEmpleadosV4(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                List<int> idsEmpleados;
                if (HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOSV4") == null)
                {
                    idsEmpleados = new List<int>();
                }
                else
                {
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOSV4");

                }
                idsEmpleados.Add(idEmpleado.Value);
                HttpContext.Session.SetObject("IDSEMPLEADOSV4", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: " + idsEmpleados.Count;
            }
            List<int> idsEmpleadosEnSesion = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOSV4");
            List<Empleado> empleados;
            if (idsEmpleadosEnSesion != null)
            {
                empleados = await this.repo.GetEmpleadosNotInSessionAsync(idsEmpleadosEnSesion);
            }
            else
            {
                empleados = await this.repo.GetEmpleadoAsync();
            }

            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosAlmacenadosV4()
        {
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOSV4");
            if (idsEmpleados == null)
            {
                ViewData["mensaje"] = "no existen empleados almacenados en Session";
                return View();
            }
            else
            {
                List<Empleado> empleados = await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }


        public async Task<IActionResult>SessionEmpleadosOK(int? idEmpleado)
        {
            if(idEmpleado != null)
            {
                List<int> idsEmpleados;
                if(HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") == null)
                {
                    idsEmpleados = new List<int>();
                }
                else
                {
                    idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");

                }
                idsEmpleados.Add(idEmpleado.Value);
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: " + idsEmpleados.Count;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadoAsync();
            return View(empleados);
        }

        public async Task<IActionResult> EmpleadosAlmacenadosOK()
        {
            List<int> idsEmpleados =
                HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if(idsEmpleados == null)
            {
                ViewData["mensaje"] = "no existen empleados almacenados en Session";
                return View();
            }
            else
            {
                List<Empleado> empleados = await this.repo.GetEmpleadosSessionAsync(idsEmpleados);
                return View(empleados);
            }
        }

        public async Task<IActionResult>SessionEmpleados(int? idEmpleado)
        {
            if(idEmpleado != null)
            {
                Empleado empleado =
                    await this.repo.FindEmpleadoAsync(idEmpleado.Value);
                List<Empleado> empleadosList;
                if(HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS") != null)
                {
                    empleadosList = HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS");

                }
                else
                {
                    empleadosList = new List<Empleado>();
                }
                empleadosList.Add(empleado);
                HttpContext.Session.SetObject("EMPLEADOS", empleadosList);
                ViewData["mensaje"] = "Empleado " + empleado.Apellido +
                    "almacenado correctamente.";
            }
            List<Empleado> empleados =
                await this.repo.GetEmpleadoAsync();
            return View(empleados);
        }
        public IActionResult EmpleadosAlmacenados()
        {
            return View();
        }

        public async Task<IActionResult> SessionSalarios(int? salario)
        {
            if(salario != null)
            {
                int sumaSalarial = 0;
                if(HttpContext.Session.GetString("SUMASALARIAL") != null)
                {
                    sumaSalarial = HttpContext.Session.GetObject<int>("SUMASALARIAL");
                }
                sumaSalarial += (int)salario;
                HttpContext.Session.SetObject("SUMASALARIAL", sumaSalarial);
                ViewData["mensaje"] = "salario almacenado: " + salario.Value;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadoAsync();
            return View(empleados);
        }

        public IActionResult SumaSalarial()
        {
            return View();
        }
    }
}

