﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PaperMID.DAL;

namespace PaperMID.Controllers
{
    public class PublicoController : Controller
    {
        // GET: Publico
        LoginDAL oLoginDAL;
        MensajeDAL oMensajeDAL;
        UsuarioDAL oUsuarioDAL;
        ConfiguracionDAL oConfiguracionDAL;
        public ActionResult Inicio()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerificarUsuario(string usuario, string contraseña)
        {
            oLoginDAL = new LoginDAL();
            int cliente = 0; cliente = oLoginDAL.verificarCliente(usuario, contraseña);
            int admin = 0; admin = oLoginDAL.verificarAdmin(usuario, contraseña);
            if (ModelState.IsValid)
            {
                if (cliente == 1)
                {
                    Session["Usuario"] = usuario;
                    return RedirectToAction("Inicio", "Cliente");
                }
                else if (admin == 1)
                {
                    Session["Usuario"] = usuario;
                    return RedirectToAction("Inicio", "Admin");
                }
                else
                {
                    IniciarSesión();
                    return View("IniciarSesión");
                }
            }
            else
            {
                IniciarSesión();
                return View("IniciarSesón");
            }
        }

        public ActionResult QuiénesSomos()
        {
            oConfiguracionDAL = new ConfiguracionDAL();
            return View(oConfiguracionDAL.Obtener_Empresa());
        }

        public ActionResult Contacto()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnviarMensaje(string nombre, string correo, string asunto, string telefono, string mensaje)
        {
            oMensajeDAL = new MensajeDAL();
            if (ModelState.IsValid)
            {
                int Resp = 0;
                Resp = oMensajeDAL.AgregarEnMerida(nombre, correo, asunto, telefono, mensaje);
                if (Resp == 1)
                {
                    TempData["Mensaje"] = "Los Datos se han actualizado con éxito";
                    return RedirectToAction("Contacto", "Publico");
                }
                else
                {
                    ViewBag.error = "Al parecer hubo un Error";
                    return RedirectToAction("Contacto", "Publico");
                }

            }
            else
            {
                ViewBag.error = "Al parecer hubo un Error";
                return RedirectToAction("Contacto", "Publico");
            }
        }

        public ActionResult IniciarSesión()
        {
            return View();
        }

        public ActionResult Registro()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarUsuario(string usuario, string correo, string contra, string recontra)
        {
            oUsuarioDAL = new UsuarioDAL();
            if (ModelState.IsValid)
            {
                if (contra == recontra)
                {
                    if (oUsuarioDAL.AgregarCliente(usuario, correo, contra) == 1)
                    {
                        return RedirectToAction("Registro", "Publico");
                    }
                    else
                    {
                        return RedirectToAction("Registro", "Publico");
                    }
                }
                else
                {
                    return RedirectToAction("Registro", "Publico");
                }
            }
            else
            {
                return View();
            }

        }
    }
}