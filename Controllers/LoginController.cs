using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using APIPresupuestos.Models;
using ConectarDatos;

namespace APIPresupuestos.Controllers
{
    public class LoginController : ApiController
    {

        public static string base64Encode(string sData) // Encode    
        {
            try
            {
                byte[] encData_byte = new byte[sData.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public static string base64Decode(string sData) //Decode    
        {
            try
            {
                var encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecodeByte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                string result = new String(decodedChar);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Decode" + ex.Message);
            }
        }

        [HttpPost]
        public Reply Login([FromBody] LoginViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;


            try
            {
                using (PresupuestosPruebaEntities db = new PresupuestosPruebaEntities())
                {
                    var lst = db.usuarios.Where(d => d.correo_electronico == model.UsuarioCorreo && d.estado == 1);
                    if (lst.Count() > 0)
                    {
                        usuario oUsuario = lst.First();

                        var contrasenia_decode = base64Decode(oUsuario.contrasenia);

                        if (contrasenia_decode == model.contrasenia)
                        {
                            //Se va a buscar la sesiones abiertas
                            var CodigoUsuarioSesion = oUsuario.id;

                            usuario oUsuarioLogin = db.usuarios.Find(oUsuario.id);
                            oR.result = oUsuarioLogin.id;
                            oR.data = Guid.NewGuid().ToString();
                            oR.message = oUsuarioLogin.correo_electronico;
                            oR.NombreUsuario = oUsuarioLogin.nombre + " " + oUsuario.apellido;
                            oR.RolUsuario = oUsuarioLogin.roles_usuarios.nombre;
                            oR.CorreoElectronico = oUsuarioLogin.correo_electronico;
                            oUsuarioLogin.token = oR.data.ToString();
                            oUsuarioLogin.sesion_activa = 1;
                            oUsuarioLogin.fecha_login = DateTime.Now.ToString();
                            db.Entry(oUsuarioLogin).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            oR.message = "El usuario o contraseña son incorrectos";
                        }
                    }
                    else
                    {
                        oR.message = "El usuario o contraseña son incorrectos";
                    }
                }
            }
            catch (Exception ex)
            {
                oR.result = 0;
                oR.message = Convert.ToString("Algo Salio Mal, Intente de Nuevo");
            }
            return oR;
        }
    }
}
